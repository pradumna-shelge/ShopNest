<script setup lang='js'>
import { userApi, loginApi,productApi } from '../Endpoints/ApiLinks'
import spinner from './Admin/spinner.vue'
import {  storeScrollPosition, restoreScrollPosition,resetScroll } from '../Services/ScrollManagementService'
import { isLogin } from '../Services/islogin.js'
import { ref, onMounted} from 'vue';
import { onBeforeRouteLeave } from 'vue-router';
import {getApiData, discription , productName } from '../Services/products'
import router from '../Router'
const scrollPosition = ref(0)
import { getCartData } from '../Services/cartServices'
import { useStore } from 'vuex';
import  WelcomeMsg  from './WelcomeMsg.vue'
const store = useStore();

//
onBeforeRouteLeave((to, from) => {
  // Check if the condition for a specific route is met
  if (to.name === 'product' ) {
    storeScrollPosition();
  }
  else{
    resetScroll();
  }

});


const products = ref([]);

const flaglogin = ref(0);
const productsTemp = ref([]);
const { getData, data, error } = getApiData();
const url = ref(productApi);
const flag = ref(true)
const asyncFunction = async () => {
  flag.value = true
  await getData(url.value).then(()=>{
    flag.value=false

  });
  console.log(data);
  products.value = data.value
  productsTemp.value = data.value
 
   await scroll();
    getCartData()
    .then((data) => {
      store.dispatch('addList', data)

    })
    .catch((error) => {
    });
};
const addTocart=()=>{
if(!isLogin){
    router.push({path:'login'})
}
 
  
   
  
}


const search=(key)=>{
  console.log(key);
  key = key.trim();
  key = key.split("  ").join(" ")
  products.value = productsTemp.value.filter((a) => (a.productName.toLowerCase().match(key.toLowerCase())||( a.description.toLowerCase().match(key.toLowerCase()))));
}
const scroll = async () => {
  return new Promise((resolve) => {
    setTimeout(() => {
      window.scrollTo(0, restoreScrollPosition());
      resolve();
    }, 0); 
  });
};



onMounted(() => {
  
  asyncFunction();
  flaglogin.value++;
   
}); 
</script>



<template>
  <WelcomeMsg :flag="flaglogin==0"   />
  <spinner :flag="flag"></spinner>
  <div class="mb-10 flex justify-center">
    <form class="w-full max-w-md">
      <label for="default-search" class="sr-only">Search Products</label>
      <div class="relative">
        <div class=" cursor-pointer absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
         
          <svg
            class="w-4 h-4 text-gray-500 dark:text-gray-400"
            aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 20 20"
            aria-label="Search Icon"
          >
            <path
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
            ></path>
          </svg>
        </div>
        <input
          @input="search($event.target.value)"
          type="search"
          id="default-search"
          class="cursor-pointer
          w-full py-2 pl-10 text-sm text-gray-900 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 placeholder-gray-400 dark:focus:border-blue-500 dark:border-gray-600 dark:focus:ring-blue-500"
          placeholder="Search here..."
          required
        />
      </div>
    </form>
  </div>

  <div class="flex flex-wrap justify-evenly gap-32  mx-10">
    <div class="  " v-if="products" v-for="(p, index) in products" :key="index">
      <!-- <a :href="p.productImage" class="MagicThumb"><img :src="p.productImage"></a> -->
      
     
      <div class="card  p-2 shadow-xl  relative  flex w-full max-w-xs flex-col overflow-hidden rounded-lg border border-gray-100 bg-gray-50 shadow-md">
        <a class="relative mx-3 mt-3 flex h-60 overflow-hidden rounded-xl justify-center ">
        <img class="object-cover" :src="p.productImage" alt="product image" />
      </a>
      
      <div class="mt-4 px-2 pb-2">
        <a >
          <h5 class="text-xl tracking-tight text-slate-900">{{ productName(p.productName)   }}</h5>
        </a>
        <a >
            <p class="text-xs  text-gray-600">{{ discription(p.description)  }}</p>
          </a>
        <div class="mt-2 mb-5 flex items-end justify-end">
          <p>
            <span class="text-xl  font-bold text-slate-900">${{p.price }}</span>
            <span class="text-sm text-red-600  line-through mx-1">${{p.mrprice }}</span>
          </p>
          <!-- <div class="flex items-center">
            <svg aria-hidden="true" class="h-5 w-5 text-yellow-300" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
              <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"></path>
            </svg>
            <svg aria-hidden="true" class="h-5 w-5 text-yellow-300" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
              <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"></path>
            </svg>
            <svg aria-hidden="true" class="h-5 w-5 text-yellow-300" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
              <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"></path>
            </svg>
            <svg aria-hidden="true" class="h-5 w-5 text-yellow-300" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
              <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"></path>
            </svg>
            <svg aria-hidden="true" class="h-5 w-5 text-yellow-300" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
              <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"></path>
            </svg>
            <span class="mr-2 ml-3 rounded bg-yellow-200 px-2.5 py-0.5 text-xs font-semibold">5.0</span>
          </div> -->
        </div>
        <a  class=" cursor-pointer flex items-center justify-center rounded-md bg-rose-700 px-5 py-2.5 text-center text-sm font-medium text-white hover:bg-gray-700 focus:outline-none focus:ring-4 focus:ring-blue-300">

             <router-link :to="{ name: 'product', params: { nameOfProduct: p.productName } }">
           View Product
          </router-link></a
        >
      </div>
    </div>
  
  
  </div>
</div>



   
</template>



<style scoped>
.card{
  min-width: 300px !important;
  max-width: 300px !important;
}
</style>