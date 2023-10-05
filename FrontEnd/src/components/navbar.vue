<script setup lang='js'>
import { ref, onMounted } from 'vue';

import logo from '../assets/logo.png'
import router from '../Router';
import { loginusername, loginRole, setuserData } from '../Services/islogin'
import { defineProps, defineEmits , computed } from 'vue';
import {getCartData} from '../Services/cartServices'
 defineProps(['flag','adminFlag'])
 const emits = defineEmits();
 import { useStore } from 'vuex'; 
const store = useStore();
const cartItemCount = computed(() => store.getters.cartItemCount);;
const cartTotalPrice = store.getters.cartTotalPrice;
const logout=()=>{
  console.log("navabr");
  localStorage.clear();
  emits('login-success1');
  router.push('/')
}
const add=()=>{
console.log(cartItemCount);
}
const restoreData=()=>{
  getCartData()
    .then((data) => {
      store.dispatch('addList', data)

    })
    .catch((error) => {
    });
}
onMounted(() => {
  setuserData();
  restoreData();

});
</script>




<template>
  
  <nav class=" p-3 bg-gray-900 fixed w-full z-20 top-0 left-0 border-b border-gray-200 border-gray-600">
      <div class="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto">
        <router-link to="/" >
      <a   class="flex items-center">
          <img  :src="logo" class=" logo" alt="ShopNest Logo">
          <span class="self-center text-2xl font-semibold whitespace-nowrap text-white"><router-link to="/" >ShopNest</router-link></span>
        </a>  </router-link>
          <div class="flex md:order-2 flex-justify-around gap-3">
            <p  v-if="flag" class="text-white my-2 mx-3">Welcome, <span class="text-cyan-500 text-bold">{{ loginusername.toUpperCase() }}</span> </p>
            <router-link v-if="flag" to="/product-cart" >
                      <div class="flex gap-1  ">
  
                        <svg  xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6 text-white mt-2 ms-3   ">
        <path stroke-linecap="round" stroke-linejoin="round" d="M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 00-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 00-16.536-1.84M7.5 14.25L5.106 5.272M6 20.25a.75.75 0 11-1.5 0 .75.75 0 011.5 0zm12.75 0a.75.75 0 11-1.5 0 .75.75 0 011.5 0z" />
      </svg>
      <p  class="text-center  bg-white  rounded-2xl w-5 h-5 text-sm m-0">{{ store.getters.cartItemCount }}</p>
      
    </div>
  </router-link>
          <button v-if="!flag" type="button" class="text-white bg-gradient-to-r from-cyan-500 to-blue-500 hover:bg-gradient-to-bl focus:ring-4 focus:outline-none focus:ring-cyan-300 dark:focus:ring-cyan-800 font-medium rounded-lg text-sm px-5 py-2.5 text-center "><router-link to="/login" >Login/Signup</router-link></button>
          <button v-if="flag" type="button" class="text-white bg-rose-700 hover:bg-rose-800 focus:ring-4 focus:outline-none focus:ring-rose-300 font-medium rounded-lg text-sm px-4 py-1 text-center mr-3 md:mr-0 bg-rose-600 hover:bg-rose-700 focus:ring-rose-800" @click="logout">Logout</button>


      </div>
      <div class="items-start justify-start hidden w-full md:flex md:w-auto md:order-1" id="navbar-sticky">
        <ul class="text-white flex justify-around gap-4">
           <button  class="hover:border-b"> <router-link to="/">Home</router-link> </button>
          <button  v-if="flag" class="hover:border-b"> <router-link to="/dashboard">Products</router-link> </button>
          <button  v-if="!flag" class="hover:border-b"> <router-link to="/login">Products</router-link> </button>
          <button   v-if="loginRole=='admin' && flag "  class="hover:border-b"> <router-link to="/user-dashboard">Users</router-link> </button>
               

        </ul>
      </div>
    </div>
  </nav>
  
 
</template>



<style scoped>
</style>