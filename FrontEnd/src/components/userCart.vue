<script setup lang="js">
import { PDFDocument } from 'pdf-lib';
import router from '../Router';
import { ref, onMounted, computed, watch, reactive } from 'vue';
import { useStore } from 'vuex';
import { getCartData, ProductCartDelete, addCartData, orderProducts } from '../Services/cartServices'
import { getApiData, discription, productName } from '../Services/products'
import { location } from '../Services/location';
import { CustomValidationMsg, passwordValidator } from '../Vadidators/index'
import { email, minLength, required } from '@vuelidate/validators';
import { useVuelidate } from '@vuelidate/core';
import { getAddresses, UpdateAddress, AddAddress } from '../Services/deliveryAddress'
import { formatDate } from '../Services/FormatDate'
import AddressForm from './userOrder/user-address.vue'
import { createToaster } from "@meforma/vue-toaster";
const toaster = createToaster({ /* options */ });
const store = useStore();
const cartItems = computed(() => store.getters.cartItems)
const orderFlag = ref(1)
const loadCartData = () => {
  getCartData()
    .then((data) => {
      store.dispatch('addList', data)

    })
    .catch((error) => {

    });
}



const openModal = ref(false);
const DeleteId = ref(0);
const open = (id) => {

  openModal.value = true;
  DeleteId.value = id
}
const closeModal = () => {
  openModal.value = false;
  DeleteId.value = -1
  addNewFlag.value = false;
  reset();
}




const removeFromCart = () => {

  ProductCartDelete(DeleteId.value).then((d) => {
    getCartData()
      .then((data) => {
        store.dispatch('addList', data)

      })
      .catch((error) => {

      });
    openModal.value = false;

  }).catch((d) => {
    openModal.value = false;
    console.log(d)
  });
}

const updateCount = (name, quantity, incFlg) => {
  const count = incFlg ? quantity + 1 : quantity - 1;

  addCartData(name, count).then((d) => {
    getCartData()
      .then((data) => {
        store.dispatch('addList', data)
      })
      .catch((error) => {

      });

  }).catch((d) => {
    console.log(d)
  });
}
// ============================add address=========
const addAddress = () => {
  addNewFlag.value = true;
}


// ===================================================checkout======================
const selectedAddress = ref({})
const addNewFlag = ref(false);
const userAddress = ref([])
const ProceedChekout = () => {

  getAddresses().then((d) => {

    orderFlag.value = 2;
    console.log(d);
    userAddress.value = d
    if (userAddress.value.length > 0) {

      selectedAddress.value = userAddress.value[0]
    }
    else {
      addNewFlag.value = true;
    }
  }).catch((d) => {
    console.log(d)
  });
}
function proceedPay() {
  if (userAddress.value.length > 0) {

    orderFlag.value = 3;
  }
  else {
    toaster.error('Please add shipping address!')
  }

}


const ProceedOrder = async () => {

  orderProducts().then((d) => {
    generatePDF(d.invoke);
    loadCartData();
    //  toaster.info("Thank you! Your order has been placed");
    orderPlaced.value = true;

  }).catch((d) => {
    toaster.error("Error while placing order!")
  });


};
const orderPlaced = ref(false)
const closeModal2 = () => {
  orderFlag.value = 1;
  orderPlaced.value = false;
  router.push('/')
}


const generatePDF = async (no) => {
  const address = selectedAddress.value;

  const pdfDoc = await PDFDocument.create();
  const page = pdfDoc.addPage([600, 600]);

  page.drawText('ShopNest', {
    x: 250,
    y: 560,
    size: 25,
  });

  const date = new Date()
  page.drawText(`Date:${formatDate(date)}`, {
    x: 400,
    y: 500,
    size: 12,
  });

  page.drawText(`Invoice No: ${no}`, {
    x: 400,
    y: 480,
    size: 12,
  });
  page.drawText('Shipping Address:', {
    x: 50,
    y: 500,
    size: 18,
  });
  page.drawText(`Name: ${address.firstName} ${address.lastName}`, {
    x: 50,
    y: 475,
    size: 12,
  });
  page.drawText(`Address: ${address.address}`, {
    x: 50,
    y: 455,
    size: 12,
  });
  page.drawText(`City: ${address.city}, State: ${address.state}, Country: ${address.country}, Zip: ${address.zip}`, {
    x: 50,
    y: 435,
    size: 12,
  });

  page.drawText('Product Details:', {
    x: 50,
    y: 380,
    size: 18,
  });

  const cellWidth = 275;
  const cellHeight = 20;
  page.drawText(`___________________________________________________________`, {
    x: 45,
    y: 370,
    size: 14,
  });
  page.drawText("PRODUCT", {
    x: 50,
    y: 355,
    size: 12,
  });
  page.drawText("QUANTITY", {
    x: 50 + cellWidth + 20,
    y: 355,
    size: 12,
  });
  page.drawText("PRICE", {
    x: 50 + cellWidth + 110,
    y: 355,
    size: 12,
  });
  page.drawText(`___________________________________________________________`, {
    x: 45,
    y: 350,
    size: 14,
  });


  let totaly = 0;
  cartItems.value.forEach((product, index) => {
    const rowY = 331 - index * cellHeight;
    totaly = rowY;
    let pName = product.productName.length > 50 ? (product.productName).substring(0, 45) + "..." : product.productName
    page.drawText(pName, {
      x: 50,
      y: rowY,
      size: 12,
    });
    page.drawText((product.quantity).toString(), {
      x: 50 + cellWidth + 40,
      y: rowY,
      size: 12,
      maxWidth: cellWidth,
    });
 const formattedPrice = parseFloat(product.price * product.quantity).toFixed(2);
    const paddedFormattedPrice = stringLevel(formattedPrice.toString());

    
    page.drawText(`${paddedFormattedPrice}`, {
      x: 50 + cellWidth + 60,
      y: rowY,
      size: 12,
      maxWidth: cellWidth,
    });
  });
let tprice = String(parseFloat(store.getters.cartTotalPrice).toFixed(2))
  let Totalprice =stringLevel(tprice.toString())
  page.drawText(`___________________________________________________________`, {
    x: 45,
    y: totaly - 30,
    size: 14,
  });
  page.drawText(`Order Amount: ${Totalprice}`, {
    x: 276,
    y: totaly - 50,
    size: 14
  });
  page.drawText(`___________________________________________________________`, {
    x: 45,
    y: totaly - 60,
    size: 14,
  });

  page.drawText(`Payment Mode :`, {
    x: 45,
    y: totaly - 120,
    size: 18,
  });
  page.drawText(` Cash On Delivery`, {
    x: 180,
    y: totaly - 120,
    size: 12,
  });


  const pdfBytes = await pdfDoc.save();
  const pdfBlob = new Blob([pdfBytes], { type: 'application/pdf' });

  const downloadLink = document.createElement('a');
  downloadLink.href = URL.createObjectURL(pdfBlob);
  downloadLink.download = `${(no).toString()}.pdf`;
  downloadLink.click();
};

const stringLevel=(data)=>{
  var newData= '$'+data;
for(var i=0;i<14;i++){
  if(data.length<i){
          newData="  "+newData
  }
}
return newData
}

const PatchAddress = (item) => {

  addNewFlag.value = true;
  UpdateFlag.value = true;
  console.log(item);
  formData.city = item.city;
  formData.state = item.state;
  formData.country = item.country;
  formData.addressId = item.addressId;
  formData.firstName = item.firstName;
  formData.lastName = item.lastName;
  formData.address = item.address;
  formData.zip = item.zip;
  formData.userId = item.userId

  console.log(formData);
}

// ------------------------------address form =============
const UpdateFlag = ref(false);
const reset = () => {
  UpdateFlag.value = false;
  document.getElementById('addressForm').reset();
  $v.value.$reset();
  formData.country = '';
  formData.state = '';
  formData.city = '';
  formData.firstName = '';
  formData.lastName = '';
  formData.address = '';
  formData.zip = '';
  states.value = null
  cities.value = null

}

const countries = ref(location.sort((a, b) => a.Name - b.Name));

let formData = reactive({
  userId: -1,
  addressId: -1,
  country: '',
  state: '',
  city: '',
  firstName: '',
  lastName: '',
  address: '',
  zip: ''
});

const rules = computed(() => {
  return {
    userId: {},
    addressId: {},
    country: { required },
    state: { required },
    city: { required },
    firstName: { required },
    lastName: { required },
    address: { required },
    zip: { required }
  };
});

const $v = useVuelidate(rules, formData);
const states = ref([]);
const cities = ref([]);

watch(() => formData.country, (newCountry) => {
  debugger;

  const country = countries.value.find((c) => c.Name === newCountry);
  if (country) {
    states.value = country.Children
      .filter((state) => state.Name) // Filter states with a truthy Name property
      .sort((a, b) => a.Name.localeCompare(b.Name));
    console.log(states.value);
    if (!UpdateFlag) {

      formData.state = '';
      formData.city = '';
    }
    cities.value = null
  }


}, { deep: true });


watch(() => formData.state, (newState) => {
  const countryName = formData.country;
  const country = countries.value.find((c) => c.Name === countryName);
  if (country) {
    const state = country.Children.find((s) => s.Name === newState);
    if (state) {
      cities.value = state.Children
        .filter((city) => city.Name)
        .sort((a, b) => a.Name.localeCompare(b.Name));

      if (!UpdateFlag) {
        formData.city = '';
      }
      else if (UpdateFlag) {
      }
    }
  }
}, { deep: true });

async function SaveAddress() {
  var d = await $v.value.$validate()

  if (d) {
    AddAddress(formData).catch((d) => {

      toaster.success("New address is updated.")
      closeModal();
      ProceedChekout()
    }).catch((err) => {
      toaster.error("Error while adding new address!")
    })
  }
}


async function updateAddress() {
  var d = await $v.value.$validate()

  if (d) {

    UpdateAddress(formData).catch((d) => {

      toaster.success("Address is updated.")
      closeModal();
      ProceedChekout()
    }).catch((err) => {
      toaster.error("Error while updating!")
    })

  }
}

const back = () => {
  orderFlag.value = orderFlag.value - 1
}

const resetAll = () => {
  orderFlag.value = 1;
}


const upadtereset = (flag) => {
  if (UpdateFlag) {
    if (flag) {

      formData.state = ''
    }
    else {
      formData.city = ''
    }
  }
}
const validateZip = () => {

  formData.zip = formData.zip.replace(/\D/g, '');


  if (formData.zip.length > 8) {
    formData.zip = formData.zip.slice(0, 8);
  }
};
onMounted(() => {

  loadCartData();


});
</script>


<template>
  <div v-show="orderFlag > 1" class="mx-10 flex justify-center w-full">


    <ol class="flex items-center w-96">
      <li
        class="flex w-full items-center text-blue-600 dark:text-blue-500 after:content-[''] after:w-full after:h-1 after:border-b after:border-blue-100 after:border-4 after:inline-block dark:after:border-blue-800">
        <span
          class="flex items-center justify-center w-10 h-10 bg-blue-100 rounded-full lg:h-12 lg:w-12 dark:bg-blue-800 shrink-0">
          <svg class="w-3.5 h-3.5 text-blue-600 lg:w-4 lg:h-4 dark:text-blue-300" aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 16 12">
            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M1 5.917 5.724 10.5 15 1.5" />
          </svg>
        </span>
      </li>
      <li
        class="flex w-full items-center after:content-[''] after:w-full after:h-1 after:border-b after:border-gray-100 after:border-4 after:inline-block dark:after:border-gray-700">
        <span v-if="orderFlag > 2"
          class="flex items-center justify-center w-10 h-10 bg-blue-100 rounded-full lg:h-12 lg:w-12 dark:bg-blue-800 shrink-0">
          <svg class="w-3.5 h-3.5 text-blue-600 lg:w-4 lg:h-4 dark:text-blue-300" aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 16 12">
            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M1 5.917 5.724 10.5 15 1.5" />
          </svg>
        </span>
        <span v-else
          class="flex items-center justify-center w-10 h-10 bg-gray-100 rounded-full lg:h-12 lg:w-12 dark:bg-gray-700 shrink-0">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-house-fill"
            viewBox="0 0 16 16">
            <path
              d="M8.707 1.5a1 1 0 0 0-1.414 0L.646 8.146a.5.5 0 0 0 .708.708L8 2.207l6.646 6.647a.5.5 0 0 0 .708-.708L13 5.793V2.5a.5.5 0 0 0-.5-.5h-1a.5.5 0 0 0-.5.5v1.293L8.707 1.5Z" />
            <path d="m8 3.293 6 6V13.5a1.5 1.5 0 0 1-1.5 1.5h-9A1.5 1.5 0 0 1 2 13.5V9.293l6-6Z" />
          </svg>
        </span>
      </li>
      <li class="flex items-center w-full">

        <span
          class="flex items-center justify-center w-10 h-10 bg-gray-100 rounded-full lg:h-12 lg:w-12 dark:bg-gray-700 shrink-0">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-cart-fill"
            viewBox="0 0 16 16">
            <path
              d="M0 1.5A.5.5 0 0 1 .5 1H2a.5.5 0 0 1 .485.379L2.89 3H14.5a.5.5 0 0 1 .491.592l-1.5 8A.5.5 0 0 1 13 12H4a.5.5 0 0 1-.491-.408L2.01 3.607 1.61 2H.5a.5.5 0 0 1-.5-.5zM5 12a2 2 0 1 0 0 4 2 2 0 0 0 0-4zm7 0a2 2 0 1 0 0 4 2 2 0 0 0 0-4zm-7 1a1 1 0 1 1 0 2 1 1 0 0 1 0-2zm7 0a1 1 0 1 1 0 2 1 1 0 0 1 0-2z" />
          </svg>
        </span>
      </li>
    </ol>

  </div>

  <div v-if="store.getters.cartItemCount > 0" class="  flex justify-center gap-10 ">
    <!-- Cart Count at the Top -->

    <!-- Card Grid -->
    <div class="w-1/3 ">
      <section v-show="orderFlag == 1">


        <p class="text-gray-500 mx-3">You have {{ store.getters.cartItemCount }} items in your cart</p>

        <div class="  flex flex-col ">
          <div v-for="(item, index) in cartItems" :key="item.cartId" class="">
            <div class="flex bg-white shadow-md rounded-md overflow-hidden  m-1 w-card">
              <!-- Left Div with Image -->
              <div class="flex flex-col justify-center">
                <img :src="item.productImage" alt="Product Image" class=" img-w w-full object-cover">
              </div>

              <!-- Right Div with Product Info -->
              <div class=" p-5 flex flex-col justify-between w-96">
                <div class="flex justify-between">
                  <h2 class="text-lg font-semibold text-gray-800">{{ item.productName }}</h2>

                  <div class="flex flex-row h-fit rounded-lg relative bg-transparent mt-1 border rounded">
                    <button :disabled="item.quantity == 1" @click="updateCount(item.productName, item.quantity, false)"
                      data-action="decrement"
                      class=" bg-red-300 text-red-600 hover:text-red-700 hover:bg-red-400  rounded-l cursor-pointer outline-none">
                      <span class="m-auto text-xl px-2">−</span>
                    </button>
                    <p class="px-5">{{ item.quantity }}</p>
                    <button :disabled="item.quantity == 10" @click="updateCount(item.productName, item.quantity, true)"
                      data-action="increment"
                      class="bg-green-300 text-green-600 hover:text-green-700 hover:bg-green-400  rounded-r cursor-pointer">
                      <span class="m-auto text-xl px-2">+</span>
                    </button>
                  </div>
                </div>
                <div class="text-sm text-gray-600">{{ discription(item.description) }}</div>
                <!-- <div class="mt-2 flex justify-between items-center">
                        <div class="text-md font-semibold text-gray-800">${{ item.price }}</div>
                      </div> -->
                <div class="mt-2 flex justify-between items-center">
                  <button @click="open(item.cartId)"
                    class=" items-center flex gap-1 text-xs text-gray-900 hover:text-white border border-gray-800 hover:bg-red-500 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1 text-center   text-gray-400 hover:text-white  ">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                      stroke="currentColor" class="w-5 h-5">
                      <path stroke-linecap="round" stroke-linejoin="round"
                        d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0" />
                    </svg>Remove item</button>
                  <div class="text-md font-semibold text-gray-800">${{ item.price }}</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
      <section v-show="orderFlag == 2">

        <div class="mt-5">


          <div>
            <div v-show="addNewFlag" class="modal ">
              <div class="modal-content1 rounded-xl text-center">
                <span @click="closeModal" class="hover:cursor-pointer text-rose-500" style="float:right;">&times;</span>


                <div class=" text-start ">
                  <h2 class="text-xl font-semibold mb-4 text-center">{{ UpdateFlag ? 'Update Shipping Address' : 'Shipping Address'}}</h2>

                  <!-- Billing Address Form -->
                  <form id="addressForm" @submit.prevent="SaveAddress">
                    <!-- Billing Address Fields -->
                    <div class="grid grid-cols-2 gap-4">
                      <!-- First Name -->
                      <div class=" col-span-1">
                        <label for="first_name" class="block text-sm font-medium text-gray-700">First Name</label>
                        <input v-model="formData.firstName" type="text" id="first_name" name="first_name"
                          class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm p-1 sm:text-sm border rounded-md">
                        <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.firstName.$errors">{{
                          CustomValidationMsg(error.$message, "First Name") }}</span>

                      </div>

                      <!-- Last Name -->
                      <div class=" col-span-1">
                        <label for="last_name" class="block text-sm font-medium text-gray-700">Last Name</label>
                        <input v-model="formData.lastName" type="text" id="last_name" name="last_name"
                          class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm p-1 sm:text-sm border rounded-md">
                        <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.lastName.$errors">{{
                          CustomValidationMsg(error.$message, "Last Name") }}</span>

                      </div>

                      <!-- Address Line 1 -->
                      <div class=" col-span-2">
                        <label for="address" class="block text-sm font-medium text-gray-700">Address</label>
                        <input v-model="formData.address" type="text" id="address" name="address"
                          class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm p-1 sm:text-sm border rounded-md">
                        <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.address.$errors">{{
                          CustomValidationMsg(error.$message, "Address") }}</span>

                      </div>

                      <!-- Country -->
                      <div class="col-span-1">
                        <label for="country" class="block text-sm font-medium text-gray-700">Country</label>
                        <select @change="upadtereset(true)" v-model="formData.country" id="country" name="country"
                          class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm p-1 sm:text-sm border rounded-md">
                          <option disabled value="">Select a Country</option>
                          <option v-for="country in countries" :key="country.LocationID" :value="country.Name">{{
                            country.Name }}</option>
                        </select>
                        <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.country.$errors">{{
                          CustomValidationMsg(error.$message, "Country") }}</span>
                      </div>


                      <!-- State/Province -->
                      <div class=" col-span-1">
                        <label for="state" class="block text-sm font-medium text-gray-700">State/Province</label>
                        <select @change="upadtereset(false)" v-model="formData.state" id="state" name="state"
                          class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm p-1 sm:text-sm border rounded-md">
                          <option v-for="state in states" :key="state.LocationID" :value="state.Name">{{ state.Name }}
                          </option>
                        </select>
                        <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.state.$errors">{{
                          CustomValidationMsg(error.$message, "State") }}</span>

                      </div>

                      <!-- City -->
                      <div class=" col-span-1">
                        <label for="city" class="block text-sm font-medium text-gray-700">City</label>
                        <select v-model="formData.city" id="city" name="city"
                          class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm p-1 sm:text-sm border rounded-md">
                          <option v-for="city in cities" :key="city.LocationID" :value="city.Name">{{ city.Name }}
                          </option>
                        </select>
                        <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.city.$errors">{{
                          CustomValidationMsg(error.$message, "City") }}</span>

                      </div>

                      <!-- ZIP/Postal Code -->
                      <div class=" col-span-1">
                        <label for="zip" class="block text-sm font-medium text-gray-700">ZIP/Postal Code</label>
                        <input @input="validateZip" v-model="formData.zip" type="text" id="zip" name="zip"
                          class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm p-1 sm:text-sm border rounded-md">
                        <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.zip.$errors">{{
                          CustomValidationMsg(error.$message, "ZIP") }}</span>

                      </div>


                    </div>
                    <div class="flex justify-center col-span-2 row mt-3">
                      <button v-if="!UpdateFlag" type="submit"
                        class="w-full btn text-green-700 hover:text-white border border-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-green-500 text-green-500 hover:text-white hover:bg-green-600 focus:ring-green-900">
                        Save Address
                      </button>


                      <button @click="updateAddress()" v-if="UpdateFlag" type="button"
                        class="w-full  btn text-cyan-700 hover:text-white border border-cyan-700 hover:bg-cyan-800 focus:ring-4 focus:outline-none focus:ring-cyan-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-cyan-500 text-cyan-500 hover:text-white hover:bg-cyan-600 focus:ring-cyan-900">
                        Update Address
                      </button>
                      <button v-if="!UpdateFlag" @click="reset" type="button"
                        class="w-full btn text-red-700 hover:text-white border border-red-700 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-red-500 text-red-500 hover:text-white hover:bg-red-600 focus:ring-red-900">
                        Reset</button>

                      <button v-else @click="closeModal" type="button"
                        class="w-full btn text-red-700 hover:text-white border border-red-700 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-red-500 text-red-500 hover:text-white hover:bg-red-600 focus:ring-red-900">Cancel</button>


                    </div>
                  </form>
                </div>
              </div>
            </div>
          </div>

          <div>
            <div class="p-4 border rounded-lg bg-white">
              <div class="flex justify-between m-2  ">
                <h2 class="text-xl font-semibold ">Shipping Address</h2>
                <button v-show="!addNewFlag" @click="() => { addNewFlag = true }"
                  class="cursor-pointer hover:text-white hover:bg-green-800 cursor-pointer border px-1 border-green-900  rounded  text-xs">+
                  New Address</button>
              </div>
              <div v-if="userAddress && selectedAddress" v-for="(item, index) in userAddress" :key="index">
                <div :class="item.addressId == selectedAddress.addressId ? 'shadow-lg bg-rose-100' : ''"
                  class=" cursor-pointer p-3  rounded  my-3 flex justify-between item-center">
                  <label class="inline-flex items-center">
                    <input name="delivery" :checked="index == 0" type="radio" v-model="selectedAddress" :value="item"
                      class="form-radio text-blue-500" />
                    <span class="ml-2 font-semibold text-sm">{{ item.firstName }} {{ item.lastName }},</span>
                    <span class="ml-2 text-gray-700 text-xs">{{ (item.address + item.city + item.state + item.country +
                      item.zip).length > 50 ? (item.address + "," + item.city + "," + item.state + "," + item.country +
                        "," +
                        item.zip).substring(0, 50) + "..." : (item.address + "," + item.city + "," + item.state + "," +
                          item.country + "," + item.zip) }}</span>
                  </label>

                  <!-- Edit button -->
                  <button v-show="item.addressId == selectedAddress.addressId" @click="PatchAddress(item)"
                    class="text-blue-500 text-xs underline"><svg xmlns="http://www.w3.org/2000/svg" fill="none"
                      viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-4 h-4">
                      <path stroke-linecap="round" stroke-linejoin="round"
                        d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L10.582 16.07a4.5 4.5 0 01-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 011.13-1.897l8.932-8.931zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0115.75 21H5.25A2.25 2.25 0 013 18.75V8.25A2.25 2.25 0 015.25 6H10" />
                    </svg></button>

                  <!-- Add other billing address details as needed -->
                </div>
              </div>
            </div>


          </div>

          <div class="mt-6 flex gap-5 bg-white p-2 rounded">
            <button @click="back()" class="bg-rose-600 text-white hover:bg-rose-700 py-2 px-4 rounded-md w-full">


              Go Back

            </button>
            <button @click="proceedPay" class="bg-cyan-600 text-white hover:bg-cyan-700 py-2 px-4 rounded-md w-full">
              Deliver to this address
            </button>
          </div>
        </div>

      </section>
      <section v-show="orderFlag == 3">
        <div class="mt-5">
          <!-- Billing Address -->
          <div class="p-4 border rounded-lg bg-white">

            <h2 class="text-xl font-semibold mb-4">Shipping Details</h2>
            <div class="flex justify-between">

              <p class="font-semibold">{{ selectedAddress.firstName }} {{ selectedAddress.lastName }} </p>
              <button @click="back"
                class=" font-semibold hover:text-white hover:bg-blue-800 cursor-pointer border px-1 border-gray-200  rounded text-blue-400  text-xs">Change</button>
            </div>
            <div class="">
              <p class="text-gray-600"> {{ selectedAddress.address }},</p>
              <p class="text-gray-600"> {{ selectedAddress.city }}, {{ selectedAddress.state }}, {{
                selectedAddress.country }}, {{
    selectedAddress.zip }}</p>

              <!-- Add other billing address details as needed -->
            </div>
          </div>


        </div>
        <div class="mt-5 w-full">

          <div class=" border rounded-lg bg-white w-full  p-2 ">
            <h2 class="text-xl font-semibold mb-4">Order Details</h2>

            <table class="w-full border-collapse border border-gray-200">
              <thead>
                <tr>
                  <th class="px-4 py-2 text-left text-sm font-medium text-gray-500 uppercase text-center">
                    Product
                  </th>
                  <th class="px-4 py-2 text-left text-sm font-medium text-gray-500 uppercase text-center">
                    Quantity
                  </th>
                  <th class="px-4 py-2 text-left text-sm font-medium text-gray-500 uppercase text-center">
                    Price
                  </th>
                </tr>
              </thead>
              <tbody>
                <!-- Loop through cartItems -->
                <template v-for="(item, index) in cartItems" :key="index">
                  <tr>
                    <td class="px-4 py-2 text-left text-sm border border-gray-200 ">
                      {{ item.productName }}
                    </td>
                    <td class="px-4 py-2 text-left text-sm border border-gray-200 text-center">
                      {{ item.quantity }}
                    </td>
                    <td class="px-4 py-2 text-right text-sm border border-gray-200">
                      ${{ item.price }}
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>

          </div>

        </div>



      </section>
    </div>
    <div class="mt-5 w-1/6">

      <div class="   p-4 border  rounded-lg bg-white">
        <h2 class="text-xl font-semibold mb-4">{{ orderFlag < 2 ? 'Checkout' : 'Price Details' }}</h2>

            <!-- Subtotal -->
            <div class="flex justify-between mb-2">
              <span class="text-gray-700">Price:</span>
              <span class="text-gray-900">${{ parseFloat(store.getters.cartMrpPrice).toFixed(2) }}</span>
            </div>

            <!-- Discount (if applicable) -->
            <div class="flex justify-between mb-2">
              <span class="text-gray-700">Discount:</span>
              <span class="text-green-600 text-xs">-${{ store.getters.cartMrpPrice - store.getters.cartTotalPrice < 1 ? 0
                : parseFloat(store.getters.cartMrpPrice - store.getters.cartTotalPrice).toFixed(2) }}</span>
            </div>

            <!-- Total Amount -->
            <div class="flex justify-between mt-4">
              <span class="text-lg font-semibold">Total Amount:</span>
              <span class="text-lg font-semibold">${{ parseFloat(store.getters.cartTotalPrice).toFixed(2) }}</span>
            </div>

            <!-- Checkout Button -->
            <button v-show="orderFlag == 1" @click="ProceedChekout()"
              class="mt-4 bg-cyan-600 text-white hover:bg-cyan-700 py-2 px-4 rounded-md w-full">
              Proceed to Buy
            </button>

            <!-- <button v-show="orderFlag == 2" @click="ProceedPay()" class="mt-4 bg-cyan-600 text-white hover:bg-cyan-700 py-2 px-4 rounded-md w-full">
            Proceed to Pay
          </button> -->
      </div>
      <div v-show="orderFlag == 3" class="mt-6 flex gap-5 bg-white p-2 rounded">
        <button @click="resetAll" class="bg-rose-600 text-white hover:bg-rose-700 text-xs py-2 rounded-md w-full">
          Cancel
        </button>
        <button @click="ProceedOrder" class="bg-cyan-600 text-white hover:bg-cyan-700 text-xs py-2 rounded-md w-full">
          Place your order
        </button>
      </div>
    </div>
  </div>
  <div v-else class="flex flex-col gap-5">

    <div class="  flex justify-center gap-10 ">
      <img src="../assets/empty-cart.png" alt="">


    </div>
    <div class="  flex justify-center gap-10  ">
      <button class="bg-sky-500 hover:bg-sky-700 text-white py-2 rounded px-10 "> <router-link to="/">Shop
          Now</router-link> </button>

    </div>
  </div>


  <div v-if="openModal" class="modal ">
    <div class="modal-content rounded-xl text-center">
      <span @click="closeModal" class="hover:cursor-pointer text-rose-500" style="float:right;">&times;</span>

      <h2 class="text-xl">Remove Item</h2>
      <p class="text-gray-500 ">Are you sure you want to remove this item?</p>
      <div class="modal-buttons flex justify-center p-5">
        <button @click="removeFromCart"
          class="text-white bg-gradient-to-r from-red-400 via-red-500 to-red-600 hover:bg-gradient-to-br focus:ring-4 focus:outline-none focus:ring-red-300 focus:ring-red-800 shadow-lg shadow-red-500/50 shadow-lg shadow-red-800/80 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2">Remove</button>
        <button @click="closeModal"
          class="text-white bg-gradient-to-r from-gray-400 via-gray-500 to-gray-600 hover:bg-gradient-to-br focus:ring-4 focus:outline-none focus:ring-gray-300 focus:ring-gray-800 shadow-lg shadow-gray-500/50 shadow-lg shadow-gray-800/80 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2">Cancel</button>
      </div>
    </div>
  </div>

  <div v-if="orderPlaced" class="modal">
    <div class="modal-content rounded-xl text-center p-3">
      <span @click="closeModal2" class="hover:cursor-pointer text-rose-500" style="float:right;">&times;</span>
      <!-- <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check-circle" viewBox="0 0 16 16">
    <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
    <path d="M10.97 4.97a.235.235 0 0 0-.02.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-1.071-1.05z"/>
  </svg> -->
      <h2 class="text-2xl text-green-500">Thank you!</h2>
      <p class="text-gray-500 my-2">Your order has been successfully placed.</p>
      <div class="modal-buttons flex justify-center ">
        <button @click="closeModal2"
          class="text-white bg-gradient-to-r from-gray-400 via-gray-500 to-gray-600 hover:bg-gradient-to-br focus:ring-4 focus:outline-none focus:ring-gray-300 focus:ring-gray-800  font-medium rounded-lg text-sm px-10 py-2.5 text-center mr-2 mb-2">OK</button>
      </div>
    </div>
  </div>
</template>


<style scoped>
.img-w {
  width: 150px;
  height: 146px;

}

.left {
  width: 170px;
  height: 146px;
}

.w-card {
  max-width: 100%;
  height: auto;
  padding: 3px;
}

.left-div {
  width: -webkit-fill-available;
  overflow-y: scroll;
  /* Enable vertical scrolling for the left div */
}

.right-div {
  width: 300px;
  /* Set a fixed width for the right div */
  position: fixed;
  left: 70%;
  top: 19%;
  /* Stick the right div to the top of the container */
  /* You can also add other styles like background color, padding, etc. */
}
</style>