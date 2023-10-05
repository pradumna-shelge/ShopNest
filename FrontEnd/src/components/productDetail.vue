<script setup lang="js">
import { userApi, loginApi, productApi } from '../Endpoints/ApiLinks'
import spinner from './Admin/spinner.vue'
import { useRoute } from 'vue-router';
import { isLogin } from '../Services/islogin.js'
import { ref, onMounted } from 'vue';
import { getApiData } from '../Services/products'
import { onBeforeRouteLeave } from 'vue-router';
import router from '../Router'
import { storeScrollPosition, restoreScrollPosition, resetScroll } from '../Services/ScrollManagementService'
import { useStore } from 'vuex';
import { addCartData, getCartData } from '../Services/cartServices'
import { createToaster } from "@meforma/vue-toaster";

const toaster = createToaster({
  /* options */
});


const store = useStore();

onBeforeRouteLeave((to, from) => {

  if (to.name != 'homepage') {

    resetScroll();
  }

});
function loginUser() {
  return localStorage.getItem("userInfo") != null
}

const route = useRoute();
const products = ref([]);
const product = ref();
const { getData, data, error } = getApiData();
const url = ref(productApi);
const flag = ref(true)
const asyncFunction = async () => {
  debugger;
  const productName = route.params.nameOfProduct
  flag.value = true
  await getData(url.value).then(() => {
    flag.value = false
  });
  console.log(data);
  products.value = data.value


  product.value = products.value.find(item => item.productName === productName)
  console.log(product.value)

};

const goback = () => {
  router.go(-1)
}

const addtoCart = () => {
  addCartData(product.value.productName, 1, product).then((d) => {
    getCartData()
      .then((data) => {
        store.dispatch('addList', data);
        toaster.success('Product is added to cart successfully.');
      })
      .catch((error) => {

      });

  }).catch((d) => {
    console.log(d)
    toaster.error('Error  product added to cart!');

  });
}
const cartFlag = () => {

}

onMounted(() => {
  asyncFunction();
});

</script>
<template v-show="false">
  <!-- <div @click="goback" class="mx-10 flex item-center text-xl">
       <a > <svg xmlns="http://www.w3.org/2000/svg" width="56" height="36" fill="currentColor" class="bi bi-arrow-left-circle-fill" viewBox="0 0 16 16">
     <path d="M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0zm3.5 7.5a.5.5 0 0 1 0 1H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5z"/>
   </svg></a>
   Back
   </div> -->
  <div v-if="product && product.productName && product.productImage && product.description && product.price">


    <div class="flex  justify-center  p-10">

      <div class=" card flex     rounded shadow-xl   bg-gray-700  ">
        <div class="left ">

          <img class=" " :src="product.productImage">




        </div>
        <div class=" p-10 mt-4  pb-5 text-white flex  flex-col justify-around right ">
          <a>
            <h5 class="text-3xl tracking-tight text-gray-200 text-center">{{ product.productName }}</h5>
          </a>
          <a class=" text-left">
            <p class="text-sm  text-gray-300">{{ product.description }}</p>
          </a>
          <div class="mt-2 mb-5    text-right">
            <p>
              <span class="text-3xl  font-bold text-sky-400 ">${{ product.price }}</span>
              <span class="text-md text-red-400 line-through mx-1">${{ product.mrprice }}</span>
            </p>

          </div>

        </div>
      </div>
    </div>
    <div class="flex  justify-center  w-100 ">

      <button @click="goback"
        class=" btn text-rose-700 hover:text-white border border-rose-700 hover:bg-rose-800 focus:ring-4 focus:outline-none focus:ring-rose-300 font-medium rounded-lg text-sm px-11 py-2.5 text-center mr-2 mb-2 border-rose-500 text-rose-500 hover:text-white hover:bg-rose-600 focus:ring-rose-900">
        Back
      </button>
      <div v-if="!loginUser()">

        <button
          class=" flex gap-1  item-center btn text-sky-700 hover:text-white border border-sky-700 hover:bg-sky-800 focus:ring-4 focus:outline-none focus:ring-sky-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-sky-500 text-sky-500 hover:text-white hover:bg-sky-600 focus:ring-sky-900">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-cart"
            viewBox="0 0 16 16">
            <path
              d="M0 1.5A.5.5 0 0 1 .5 1H2a.5.5 0 0 1 .485.379L2.89 3H14.5a.5.5 0 0 1 .491.592l-1.5 8A.5.5 0 0 1 13 12H4a.5.5 0 0 1-.491-.408L2.01 3.607 1.61 2H.5a.5.5 0 0 1-.5-.5zM3.102 4l1.313 7h8.17l1.313-7H3.102zM5 12a2 2 0 1 0 0 4 2 2 0 0 0 0-4zm7 0a2 2 0 1 0 0 4 2 2 0 0 0 0-4zm-7 1a1 1 0 1 1 0 2 1 1 0 0 1 0-2zm7 0a1 1 0 1 1 0 2 1 1 0 0 1 0-2z" />
          </svg>
          <router-link to="/login"> Add to Cart.</router-link>

        </button>

      </div>
      <div v-if="loginUser()">
        <button v-if="!store.getters.isProductflag(product.productName)" @click="addtoCart"
          class="flex  gap-1 item-center btn text-green-700 hover:text-white border border-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-green-500 text-green-500 hover:text-white hover:bg-green-600 focus:ring-green-900">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-cart"
            viewBox="0 0 16 16">
            <path
              d="M0 1.5A.5.5 0 0 1 .5 1H2a.5.5 0 0 1 .485.379L2.89 3H14.5a.5.5 0 0 1 .491.592l-1.5 8A.5.5 0 0 1 13 12H4a.5.5 0 0 1-.491-.408L2.01 3.607 1.61 2H.5a.5.5 0 0 1-.5-.5zM3.102 4l1.313 7h8.17l1.313-7H3.102zM5 12a2 2 0 1 0 0 4 2 2 0 0 0 0-4zm7 0a2 2 0 1 0 0 4 2 2 0 0 0 0-4zm-7 1a1 1 0 1 1 0 2 1 1 0 0 1 0-2zm7 0a1 1 0 1 1 0 2 1 1 0 0 1 0-2z" />
          </svg>
          Add To Cart
        </button>
        <button v-else
          class=" flex gap-1  item-center btn text-sky-700 hover:text-white border border-sky-700 hover:bg-sky-800 focus:ring-4 focus:outline-none focus:ring-sky-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-sky-500 text-sky-500 hover:text-white hover:bg-sky-600 focus:ring-sky-900">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-cart-check-fill"
            viewBox="0 0 16 16">
            <path
              d="M.5 1a.5.5 0 0 0 0 1h1.11l.401 1.607 1.498 7.985A.5.5 0 0 0 4 12h1a2 2 0 1 0 0 4 2 2 0 0 0 0-4h7a2 2 0 1 0 0 4 2 2 0 0 0 0-4h1a.5.5 0 0 0 .491-.408l1.5-8A.5.5 0 0 0 14.5 3H2.89l-.405-1.621A.5.5 0 0 0 2 1H.5zM6 14a1 1 0 1 1-2 0 1 1 0 0 1 2 0zm7 0a1 1 0 1 1-2 0 1 1 0 0 1 2 0zm-1.646-7.646-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 1 1 .708-.708L8 8.293l2.646-2.647a.5.5 0 0 1 .708.708z" />
          </svg>
          <router-link to="/product-cart"> Go to cart</router-link>

        </button>
      </div>






    </div>
  </div>
</template>

<style scoped>
.card {
  min-width: 500px !important;
  max-width: 800px !important;
  max-height: 300px !important;
}


img {


  min-width: 300px;
  height: 300px;
}
</style>./state/cart.js