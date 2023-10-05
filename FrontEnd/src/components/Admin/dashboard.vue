<script setup lang="js">
import * as XLSX from 'xlsx';

import spinner from './spinner.vue'
import { formatDate, addDecimalIfMissing } from '../../Services/FormatDate'

import { reactive, ref, computed, onMounted } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import { CustomValidationMsg } from '../../Vadidators/index'
import { required } from '@vuelidate/validators';
import { createToaster } from "@meforma/vue-toaster";
import { userApi, loginApi, productApi, imageApi } from '../../Endpoints/ApiLinks'
import { getApiData, ProductAdd, ProductDelete, ProductUpdate, discription, productName } from '../../Services/products'
import { loginRole } from './../../Services/islogin'
const currentPage = ref(1);
const DeleteId = ref(-1);
const itemsPerPage = ref(5);
const totalItem = ref(2);
const flag = ref(true)
let productUpdateFlag = ref(false)
const displayedProducts = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage.value;
  const end = start + itemsPerPage.value;
  return products.value.slice(start, end);
});
const changePage = (page) => {
  currentPage.value = page;
};

const perPageOptions = [5, 10, 15, 20, 25];
const products = ref([]);
const productsTemp = ref([]);
const { getData, data, error } = getApiData();


const openModal = ref(false);

const open = (id) => {
  openModal.value = true;
  DeleteId.value = id
}
const handlePageChange = (newPage) => {
  currentPage.value = newPage;
  // asyncFunction();  
};
const url = ref(productApi);
const imgFile = ref();
const asyncFunction = async () => {
  flag.value = true;
  await getData(url.value).then(() => {
    flag.value = false;
  });
  console.log(data.value);
  products.value = data.value;
  productsTemp.value = data.value
  totalItem.value = data.value.length
};
const toaster = createToaster({ /* options */ });
let productData = reactive({
  productId: null,
  productName: '',
  productImage: '',
  description: '',
  price: null,
  mrp: null
});


const rules = computed(() => {
  return {
    productId: {},
    productName: { required },
    productImage: { required },
    description: { required },
    price: { required },
    mrp: { required }
  };
});

const $v = useVuelidate(rules, productData);

// function handleImageChange(event) {
//   const fileData = event.target.files[0];
//   console.log(fileData);

//   const selectedFile = event.target.files[0];
//   const reader = new FileReader();
//   reader.onload = (e) => {
//     const content = new Uint8Array(e.target.result);
//     if (!isJpeg(content)) {
//       toaster.error("Invalid image content. Please select a valid image file.");
//       event.target.value = "";

//     } 
//   };

//   if (selectedFile) {
//     const allowedTypes = ["image/png", "image/jpeg", "image/jpg", "image/bmp"];
//     const fileType = selectedFile.type;
//     const maxSize = 2 * 1024 * 1024;

//     if (!allowedTypes.includes(fileType.toLowerCase())) {
//       toaster.error("Unsupported file type. Please select a PNG, JPEG, JPG, or BMP file.");

//       event.target.value = "";
//     }
//     else if (selectedFile.size > maxSize) {
//       toaster.error("File size exceeds the maximum limit of 2 MB.");
//       event.target.value = "";
//     }
//     else {

//       productData.productImage = fileData;
//     }
//   }
// }
function handleImageChange(event) {
  const fileData = event.target.files[0];
  console.log(fileData);

  const selectedFile = event.target.files[0];
  if (!selectedFile) {
    return;
  }

  const allowedTypes = ["image/png", "image/jpeg", "image/jpg", "image/bmp"];
  const fileType = selectedFile.type;
  const maxSize = 2 * 1024 * 1024; // 2 MB

  if (!allowedTypes.includes(fileType.toLowerCase())) {
    toaster.error("Unsupported file type! Please select a PNG, JPEG, JPG, or BMP file.");
    event.target.value = "";
    return;
  } else if (selectedFile.size > maxSize) {
    toaster.error("File size exceeds the maximum limit of 2 MB!");
    event.target.value = "";
    return;
  }


  const imageUrl = URL.createObjectURL(selectedFile);


  const img = new Image();
  img.onload = () => {

    productData.productImage = fileData;
  };
  img.onerror = () => {

    toaster.error("Invalid image content! Please select a valid image file.");
    event.target.value = "";
  };
  img.src = imageUrl;
}



async function addProduct() {

  var d = await $v.value.$validate()
  if (d) {
    flag.value = true;
    console.log(productData)
    const formData = new FormData();
    formData.append('productName', productData.productName.replace(/\s{2,}/g, ' ').trim() ?? "");
    formData.append('description', productData.description.replace(/\s{2,}/g, ' ').trim() ?? "");
    formData.append('price', productData.price ?? 0);
    formData.append('mrp', productData.mrp ?? 0);
    formData.append('productImage', productData.productImage);
    ProductAdd(formData, productApi)
      .then((imageUrl) => {
        asyncFunction();
        toaster.success("Product added successfully.");

        cancelUpdate()
      })
      .catch((error) => {
        flag.value = false;
        toaster.error(error.response.data + "!");
      });
  }

}
async function updateProduct() {


  var d = await $v.value.$validate()
  if (d) {
    flag.value = true;
    console.log(productData)
    const formData = new FormData();
    formData.append('productId', productData.productId ?? -1);
    formData.append('productName', productData.productName.replace(/\s{2,}/g, ' ').trim() ?? "");
    formData.append('description', productData.description.replace(/\s{2,}/g, ' ').trim() ?? "");
    formData.append('price', productData.price ?? 0);
    formData.append('mrp', productData.mrp ?? 0);
    formData.append('productImage', productData.productImage ?? "");
    // formData.append('IsproductImageUpadte', productData.productImage?true:false);
    ProductUpdate(formData, productApi)
      .then((imageUrl) => {
        asyncFunction();
        productUpdateFlag.value = false
        toaster.success("Product updated successfully.");
        cancelUpdate()

      })
      .catch((error) => {
        toaster.error(error.response.data + "!");
        flag.value = false;
      });
  }

}

async function deleteProduct() {
  flag.value = true;
  openModal.value = false
  try {

    await ProductDelete(DeleteId.value, productApi);

    toaster.success("Product deleted successfully.");
    asyncFunction();
  } catch (error) {
    console.error(error.response.data + "!");
    flag.value = false;
    toaster.error(error.response.data + "!");
  }
}

const resetpageNo = () => {
  handlePageChange(1)
}


const patchValue = (ob) => {
  console.log(ob);
  productData.productId = ob.productId
  productData.productName = ob.productName;
  productData.price = ob.price;
  productData.description = ob.description;
  productData.productImage = ob.productImage;
  productData.mrp = ob.mrprice;
  productUpdateFlag.value = true
}


const cancelUpdate = () => {
  productData.productName = null;
  productData.productImage = null;
  productData.description = null;
  productData.price = null;
  imgFile.value = ''
  productData.mrp = null
  document.getElementById('productForm').reset();
  productUpdateFlag.value = false;
  $v.value.$reset();

}


const validatePrice = (flag) => {
  let sanitizedPrice = flag ? productData.price.replace(/[^0-9.]/g, '') : productData.mrp.replace(/[^0-9.]/g, '');
  const parts = sanitizedPrice.split('.');
  if (parts.length > 2) {
    sanitizedPrice = parts[0] + '.' + parts.slice(1).join('');
  }
  if (parts[0].length > 6) {
    sanitizedPrice = parts[0].slice(0, 9) + '.' + (parts[1] || '');
  }
  if (parts[1] && parts[1].length > 2) {
    sanitizedPrice = parts[0] + '.' + parts[1].slice(0, 2);
  }
  if (flag) {

    productData.price = sanitizedPrice;
  }
  else {
    productData.mrp = sanitizedPrice;
  }
};


const restrictNonNumeric = (event) => {
  const allowedChars = /[0-9.]/;
  const allowedKeys = ['Backspace', 'ArrowLeft', 'ArrowRight', 'Delete', 'Home', 'End'];

  if (!allowedChars.test(event.key) && !allowedKeys.includes(event.key)) {
    event.preventDefault();
  }
};

const closeModal = () => {
  openModal.value = false;
  DeleteId.value = -1
}

const searchColumn = (col, key) => {
  key = key.trim();
  key = key.split("  ").join(" ")
  console.log(key);
  switch (col) {
    case 'productName':
      products.value = productsTemp.value.filter((a) => a.productName.toLowerCase().match(key.toLowerCase()));
      break;
    case 'description':
      products.value = productsTemp.value.filter((a) => a.description.toLowerCase().match(key.toLowerCase()));
      break;
    case 'price':
      if (key === 'all') {
        products.value = productsTemp.value;
      }
      else {

        const [minPrice, maxPrice] = key.split('-').map(Number);
        products.value = productsTemp.value.filter(product => product.price >= minPrice && product.price <= maxPrice);
      }

      break;
    case 'mrp':
      if (key === 'all') {
        products.value = productsTemp.value;
      }
      else {

        const [minPrice, maxPrice] = key.split('-').map(Number);
        products.value = productsTemp.value.filter(product => product.mrprice >= minPrice && product.mrprice <= maxPrice);
      }

      break;
  }
}


const sortColumn = (col, sortFlag) => {

  switch (col) {
    case 'productName':
      products.value.sort((a, b) => {
        const nameA = a.productName.toUpperCase();
        const nameB = b.productName.toUpperCase();
        if (nameA < nameB) return sortFlag ? -1 : 1;
        if (nameA > nameB) return sortFlag ? 1 : -1;
        return 0;
      });
      break;
    case 'description':
      products.value.sort((a, b) => {
        const nameA = a.description.toUpperCase();
        const nameB = b.description.toUpperCase();
        if (nameA < nameB) return sortFlag ? -1 : 1;
        if (nameA > nameB) return sortFlag ? 1 : -1;
        return 0;
      });
      break;
    case 'price':
      products.value.sort((a, b) => {
        const nameA = a.price;
        const nameB = b.price;
        if (nameA < nameB) return sortFlag ? -1 : 1;
        if (nameA > nameB) return sortFlag ? 1 : -1;
        return 0;
      });
      break;
    case 'mrp':
      products.value.sort((a, b) => {
        const nameA = a.mrprice;
        const nameB = b.mrprice;
        if (nameA < nameB) return sortFlag ? -1 : 1;
        if (nameA > nameB) return sortFlag ? 1 : -1;
        return 0;
      });
      break;
  }
}
const printTable = () => {
  const data = products.value;


  const filteredData = data.map(item => {
    const { productName, description, price, mrprice } = item;
    return { Product: productName, Description: description, Price: price, MRP: mrprice };
  });


  const ws = XLSX.utils.json_to_sheet(filteredData);
  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
  XLSX.writeFile(wb, 'Product Data.xlsx');
};



onMounted(() => {
  asyncFunction();
});





</script>

<template>
  <spinner :flag="flag"></spinner>


  <div class="flex flex-row justify-center gap-10">

    <div v-if="loginRole == 'admin'" class="basis-1/4  ">
      <div class="w-full border bg-white rounded-lg hover:shadow-xl  md:mt-0 sm:max-w-md xl:p-0 ">
        <div class="p-6 space-y-4 md:space-y-6 sm:p-8 ">
          <h1 class="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl ">
            {{ productUpdateFlag ? "Update Product" : "Product Information" }}
          </h1>
          <form id="productForm" class="space-y-4 md:space-y-6 text-end" @submit.prevent="addProduct"
            enctype="multipart/form-data">
            <div>
              <!-- <label for="productName"
                                class="block mb-2 text-sm font-medium text-gray-900 ">Product Name</label> -->
              <input maxlength="80" :class="{ 'border-2 border-rose-600 ': $v.productName.$error }" id="product"
                v-model="productData.productName" type="text"
                class=" border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5  border-gray-600   focus:ring-blue-500 focus:border-blue-500"
                placeholder="Product Name" />
              <span class="text-red-400 text-xs text-end" v-for="error in $v.productName.$errors">{{
                CustomValidationMsg(error.$message, "Product Name") }}</span>
            </div>
            <div>
              <label for="productImage" class="block mb-2 text-sm font-medium text-gray-900  text-left">Product
                Image</label>
              <input id="productImage" :class="{ 'border-2 border-rose-600 ': $v.productImage.$error }"
                accept=".png, .jpeg, .jpg, .bmp" :size="2 * 1024 * 1024" type="file" @change="handleImageChange($event)"
                class="cursor-pointer  block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100" />
              <span class="text-red-400 text-xs text-end" v-for="error in $v.productImage.$errors">{{
                CustomValidationMsg(error.$message, "Product Image") }}</span>
            </div>
            <div>
              <!-- <label for="description"
                                class="block mb-2 text-sm font-medium text-gray-900 ">Description</label> -->
              <textarea maxlength="200" :class="{ 'border-2 border-rose-600 ': $v.description.$error }"
                v-model="productData.description" id="description"
                class=" border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5  border-gray-600   focus:ring-blue-500 focus:border-blue-500"
                placeholder="Product Description"></textarea>
              <span class="text-red-400 text-xs text-end" v-for="error in $v.description.$errors">{{
                CustomValidationMsg(error.$message, "Product Description") }}</span>
            </div>
            <div>
              <input :class="{ 'border-2 border-rose-600 ': $v.mrp.$error }" @keydown="restrictNonNumeric"
                @input="validatePrice(false)" v-model="productData.mrp" type="text" id="mrp"
                class=" border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5  border-gray-600   focus:ring-blue-500 focus:border-blue-500"
                placeholder="MRP Price" />
              <span class="text-red-400 text-xs text-end" v-for="error in $v.mrp.$errors">{{
                CustomValidationMsg(error.$message, "Product MRP") }}</span>
            </div>
            <div>
              <!-- <label for="price"
                                class="block mb-2 text-sm font-medium text-gray-900 ">Price</label> -->
              <input :class="{ 'border-2 border-rose-600 ': $v.price.$error }" @keydown="restrictNonNumeric"
                @input="validatePrice(true)" v-model="productData.price" type="text" id="price"
                class=" border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5  border-gray-600   focus:ring-blue-500 focus:border-blue-500"
                placeholder="Sell Price" />
              <span class="text-red-400 text-xs text-end" v-for="error in $v.price.$errors">{{
                CustomValidationMsg(error.$message, "Product Price") }}</span>
            </div>

            <!-- <button
                  type="submit"
                  class="w-full text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center bg-blue-600 hover:bg-blue-700 focus:ring-blue-800"
                >
                  Add Product
                </button> -->
            <div class="flex items-center justify-center gap-5">
              <button v-if="!productUpdateFlag" type="submit"
                class=" btn text-green-700 hover:text-white border border-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-green-500 text-green-500 hover:text-white hover:bg-green-600 focus:ring-green-900">
                Save Product
              </button>


              <button @click="updateProduct()" v-if="productUpdateFlag" type="button"
                class="  btn text-cyan-700 hover:text-white border border-cyan-700 hover:bg-cyan-800 focus:ring-4 focus:outline-none focus:ring-cyan-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-cyan-500 text-cyan-500 hover:text-white hover:bg-cyan-600 focus:ring-cyan-900">
                Update
              </button>
              <button @click="cancelUpdate" type="button"
                class=" btn text-red-700 hover:text-white border border-red-700 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-red-500 text-red-500 hover:text-white hover:bg-red-600 focus:ring-red-900">{{
                  productUpdateFlag ? 'Cancel' : 'Reset' }}</button>


            </div>

          </form>
        </div>
      </div>
    </div>




    <div class="basis-1/2">
      <div class="flex gap-3">

        <table id="productable" class=" shadow-xl  text-left bg-white">
          <thead class="w-100">
            <tr class="border-y ">
              <th class="  px-6 py-3  text-left text-sm leading-4 font-medium text-gray-500 tracking-wider">
                <span class="flex justify-center"> <span>Product</span> <svg @click="sortColumn('productName', true)"
                    xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                    class="cursor-pointer hover:text-gray-900 bi bi-caret-up-fill" viewBox="0 0 16 16">
                    <path
                      d="m7.247 4.86-4.796 5.481c-.566.647-.106 1.659.753 1.659h9.592a1 1 0 0 0 .753-1.659l-4.796-5.48a1 1 0 0 0-1.506 0z" />
                  </svg><svg @click="sortColumn('productName', false)" xmlns="http://www.w3.org/2000/svg" width="16"
                    height="16" fill="currentColor" class="cursor-pointer hover:text-gray-900 bi bi-caret-down-fill"
                    viewBox="0 0 16 16">
                    <path
                      d="M7.247 11.14 2.451 5.658C1.885 5.013 2.345 4 3.204 4h9.592a1 1 0 0 1 .753 1.659l-4.796 5.48a1 1 0 0 1-1.506 0z" />
                  </svg></span>
                <div class="flex justify-center">

                  <input placeholder="search  by Name" class="border-2  rounded px-1" type="text"
                    @input="searchColumn('productName', $event.target.value)">
                </div>
              </th>
              <th class="   px-3 py-3  text-left text-sm leading-4 font-medium text-gray-500 tracking-wider">

                <span class="flex justify-center"> <span>Description</span><svg @click="sortColumn('description', true)"
                    xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                    class="cursor-pointer hover:text-gray-900 bi bi-caret-up-fill" viewBox="0 0 16 16">
                    <path
                      d="m7.247 4.86-4.796 5.481c-.566.647-.106 1.659.753 1.659h9.592a1 1 0 0 0 .753-1.659l-4.796-5.48a1 1 0 0 0-1.506 0z" />
                  </svg><svg @click="sortColumn('description', false)" xmlns="http://www.w3.org/2000/svg" width="16"
                    height="16" fill="currentColor" class="cursor-pointer hover:text-gray-900 bi bi-caret-down-fill"
                    viewBox="0 0 16 16">
                    <path
                      d="M7.247 11.14 2.451 5.658C1.885 5.013 2.345 4 3.204 4h9.592a1 1 0 0 1 .753 1.659l-4.796 5.48a1 1 0 0 1-1.506 0z" />
                  </svg></span>
                <div class="flex justify-center">

                  <input placeholder="search  by Description" class="mx-auto border-2 rounded px-1 w-100" type="text"
                    @input="searchColumn('description', $event.target.value)">
                </div>

              </th>
              <th class="  px-2 py-3  text-left text-sm leading-4 font-medium text-gray-500 ">

                <span class="flex justify-center"><span> Price</span><svg @click="sortColumn('price', true)"
                    xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                    class="cursor-pointer hover:text-gray-900 bi bi-caret-up-fill" viewBox="0 0 16 16">
                    <path
                      d="m7.247 4.86-4.796 5.481c-.566.647-.106 1.659.753 1.659h9.592a1 1 0 0 0 .753-1.659l-4.796-5.48a1 1 0 0 0-1.506 0z" />
                  </svg><svg @click="sortColumn('price', false)" xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                    fill="currentColor" class="cursor-pointer hover:text-gray-900 bi bi-caret-down-fill"
                    viewBox="0 0 16 16">
                    <path
                      d="M7.247 11.14 2.451 5.658C1.885 5.013 2.345 4 3.204 4h9.592a1 1 0 0 1 .753 1.659l-4.796 5.48a1 1 0 0 1-1.506 0z" />
                  </svg></span>
                <!-- <input class="border" type="text"  @input="searchColumn('price', $event.target.value)">
               -->
                <div class="flex justify-center">

                  <select id="price-filter" @change="searchColumn('price', $event.target.value)">
                    <option value="all" selected>All Prices</option>
                    <option value="0-500">0-500</option>
                    <option value="500-1000">500-1000</option>
                    <option value="1000-9999999">1000-more</option>
                  </select>
                </div>
              </th>
              <th class="  px-2 py-3  text-left text-sm leading-4 font-medium text-gray-500 ">

                <span class="flex justify-center"><span>MRP</span><svg @click="sortColumn('mrp', true)"
                    xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                    class="cursor-pointer hover:text-gray-900 bi bi-caret-up-fill" viewBox="0 0 16 16">
                    <path
                      d="m7.247 4.86-4.796 5.481c-.566.647-.106 1.659.753 1.659h9.592a1 1 0 0 0 .753-1.659l-4.796-5.48a1 1 0 0 0-1.506 0z" />
                  </svg><svg @click="sortColumn('mrp', false)" xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                    fill="currentColor" class="cursor-pointer hover:text-gray-900 bi bi-caret-down-fill"
                    viewBox="0 0 16 16">
                    <path
                      d="M7.247 11.14 2.451 5.658C1.885 5.013 2.345 4 3.204 4h9.592a1 1 0 0 1 .753 1.659l-4.796 5.48a1 1 0 0 1-1.506 0z" />
                  </svg></span>
                <!-- <input class="border" type="text"  @input="searchColumn('price', $event.target.value)">
               -->
                <div class="flex justify-center">

                  <select id="price-filter" @change="searchColumn('mrp', $event.target.value)">
                    <option value="all" selected>All Prices</option>
                    <option value="0-500">0-500</option>
                    <option value="500-1000">500-1000</option>
                    <option value="1000-9999999">1000-more</option>
                  </select>
                </div>
              </th>
              <td v-if="loginRole == 'admin'">
                <div class="flex justify-center item-center">
                  <button @click="printTable()" class="Btn">
                    <svg class="svgIcon" viewBox="0 0 384 512" height="1em" xmlns="http://www.w3.org/2000/svg">
                      <path
                        d="M169.4 470.6c12.5 12.5 32.8 12.5 45.3 0l160-160c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L224 370.8 224 64c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 306.7L54.6 265.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l160 160z">
                      </path>
                    </svg>
                    <span class="icon2"></span>
                    <span class="tooltip">Download</span>
                  </button>
                </div>
              </td>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(p, index) in displayedProducts" :key="index"
              class="cursor-pointer hover:bg-gray-100 cursor-pointer">
              <td class="px-5 py-2  border-b  ">
                <div class="flex items-center justify-start gap-3  ">
                  <div class=" ">
                    <img class="h-10 w-10 rounded-full object-cover" :src="p.productImage" alt="Product Image" />
                  </div>

                  <div class=" text-black     text-sm">
                    {{ productName(p.productName) }}
                  </div>

                </div>
              </td>
              <td class="px-6 py-2 whitespace-no-wrap border-b">
                <div class=" text-xs  text-gray-800">
                  {{ discription(p.description) }}
                </div>
              </td>
              <td class="px-2 py-2 whitespace-no-wrap border-b text-right">
                <div class=" leading-2 text-gray-900">
                  ${{ p.price }}
                </div>
              </td>
              <td class="px-2 py-2 whitespace-no-wrap border-b text-right">
                <div class=" leading-2 text-gray-900">
                  ${{ p.mrprice }}
                </div>
              </td>
              <td v-if="loginRole == 'admin'"
                class="flex gap-2 px-3  whitespace-no-wrap text-right text-sm leading-5 font-medium">

                <button @click="patchValue(p)" class="Btn py-3 text-indigo-600 hover:text-indigo-900">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="w-6 h-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L10.582 16.07a4.5 4.5 0 01-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 011.13-1.897l8.932-8.931zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0115.75 21H5.25A2.25 2.25 0 013 18.75V8.25A2.25 2.25 0 015.25 6H10" />
                  </svg>

                  <span class="tooltip">Edit</span>
                </button>

                <button @click="open(p.productId)" class="Btn py-3  text-red-600 hover:text-red-900">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="w-6 h-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0" />
                  </svg>

                  <span class="tooltip">Delete</span>
                </button>
                <button @click="open(p.productId)" class="Btn py-3 hover:text-sky-900 text-sky-700 mt-1">
                  <router-link :to="{ name: 'product', params: { nameOfProduct: p.productName } }">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                      class="bi bi-arrows-angle-expand" viewBox="0 0 16 16">
                      <path fill-rule="evenodd"
                        d="M5.828 10.172a.5.5 0 0 0-.707 0l-4.096 4.096V11.5a.5.5 0 0 0-1 0v3.975a.5.5 0 0 0 .5.5H4.5a.5.5 0 0 0 0-1H1.732l4.096-4.096a.5.5 0 0 0 0-.707zm4.344-4.344a.5.5 0 0 0 .707 0l4.096-4.096V4.5a.5.5 0 1 0 1 0V.525a.5.5 0 0 0-.5-.5H11.5a.5.5 0 0 0 0 1h2.768l-4.096 4.096a.5.5 0 0 0 0 .707z" />
                    </svg>
                  </router-link>

                  <span class="tooltip">View</span>
                </button>

              </td>
            </tr>
          </tbody>
        </table>
        <!-- <button v-if="loginRole!='admin'" @click="printTable()" class="Btn mt-2">
           <svg class="svgIcon" viewBox="0 0 384 512" height="1em" xmlns="http://www.w3.org/2000/svg"><path d="M169.4 470.6c12.5 12.5 32.8 12.5 45.3 0l160-160c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L224 370.8 224 64c0-17.7-14.3-32-32-32s-32 14.3-32 32l0 306.7L54.6 265.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l160 160z"></path></svg>
           <span class="icon2"></span>
           <span class="tooltip">Download</span>
        </button> -->


      </div>

      <div class="flex justify-end mt-10">
        <div class=" ">
          <label for="itemsPerPage" class=" mb-2 text-sm font-medium text-gray-900">Items Per Page</label>
          <select @change="resetpageNo" id="itemsPerPage" v-model="itemsPerPage"
            class="block bg-transparent  py-2.5 px-0 w-full text-sm text-gray-500 border-0 border-b-2 border-gray-200 appearance-none text-gray-400 border-gray-700 focus:outline-none focus:ring-0 focus:border-gray-200 peer">
            <option v-for="option in perPageOptions" :key="option" :value="option">{{ option }}</option>
          </select>
        </div>
        <vue3-tailwind-pagination class="bg-new text-black" :total="totalItem" :per_page="itemsPerPage"
          :active_color="'text-white'" :active_background="'bg-rose-500'" :current_page="currentPage"
          @change="handlePageChange" />
      </div>
    </div>



  </div>


  <div v-if="openModal" class="modal ">
    <div class="modal-content rounded-xl text-center">
      <span @click="closeModal" class="hover:cursor-pointer text-rose-500" style="float:right;">&times;</span>
      <div class="flex justify-center  p-5">

        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"
          class="w-16 h-16 text-rose-500 ">
          <path stroke-linecap="round" stroke-linejoin="round"
            d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0" />
        </svg>
      </div>
      <h2 class="text-xl">Confirm Delete</h2>
      <p class="text-gray-500 ">Are you sure you want to delete this product?</p>
      <div class="modal-buttons flex justify-center p-5">
        <button @click="deleteProduct"
          class="text-white bg-gradient-to-r from-red-400 via-red-500 to-red-600 hover:bg-gradient-to-br focus:ring-4 focus:outline-none focus:ring-red-300 focus:ring-red-800 shadow-lg shadow-red-500/50 shadow-lg shadow-red-800/80 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2">Yes,
          delete it!</button>
        <button @click="closeModal"
          class="text-white bg-gradient-to-r from-gray-400 via-gray-500 to-gray-600 hover:bg-gradient-to-br focus:ring-4 focus:outline-none focus:ring-gray-300 focus:ring-gray-800 shadow-lg shadow-gray-500/50 shadow-lg shadow-gray-800/80 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2">Cancel</button>
      </div>

    </div>
  </div>
</template>

<style scoped>
th {
  max-width: 200px !important;
  min-width: 50px !important;

}

.bg-new {
  background-color: rgba(255, 0, 0, 0);
}

.btn {
  width: 150px;
}

.Btn {
  width: 37px;
  height: 34px;
  border: none;
  border-radius: 50%;

  display: flex;
  flex-direction: column;
  align-items: center;

  cursor: pointer;
  position: relative;
  transition-duration: .3s;
  /* box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.11); */
}

.svgIcon {
  fill: rgb(110, 3, 233);
}

.icon2 {
  width: 11px;
  height: 5px;
  border-bottom: 2px solid rgb(182, 143, 255);
  border-left: 2px solid rgb(182, 143, 255);
  border-right: 2px solid rgb(182, 143, 255);
}

.tooltip {
  right: 0px;
  margin-top: -63px;
  opacity: 0;
  background-color: rgb(12, 12, 12);
  color: white;
  padding: 5px 10px;
  border-radius: 5px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition-duration: .2s;
  pointer-events: none;
  letter-spacing: 0.5px;
}

.tooltip::before {
  position: absolute;
  content: "";
  width: 10px;
  height: 10px;

  background-size: 1000%;
  background-position: center;
  transform: rotate(45deg);
  left: -5%;
  transition-duration: .3s;
}

.Btn:hover .tooltip {
  opacity: 1;
  transition-duration: .3s;
}

.Btn:hover {

  transition-duration: .3s;
}

.Btn:hover .icon2 {
  border-bottom: 2px solid rgb(235, 235, 235);
  border-left: 2px solid rgb(235, 235, 235);
  border-right: 2px solid rgb(235, 235, 235);
}

.Btn:hover .svgIcon {

  animation: slide-in-top 0.6s cubic-bezier(0.250, 0.460, 0.450, 0.940) both;
}

@keyframes slide-in-top {
  0% {
    transform: translateY(-10px);
    opacity: 0;
  }

  100% {
    transform: translateY(0px);
    opacity: 1;
  }
}</style>