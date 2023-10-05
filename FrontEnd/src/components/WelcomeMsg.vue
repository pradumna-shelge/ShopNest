<script setup lang='js'>
const { flag } = defineProps(['flag'])
import { ref, reactive, computed, onMounted } from 'vue';
import { formatDate } from '../Services/FormatDate'

const flagTemp = ref(true);
const openModalFlag = ref()
import logo from '../assets/logo.png'

const closeModal = () => {

  localStorage.setItem("loginFlag", false)
  flagTemp.value = false;
}

const getData = () => {
  debugger;
  const data = JSON.parse(localStorage.getItem("loginInfo"));
  openModalFlag.value = data
  flagTemp.value = localStorage.getItem("loginFlag") == "true" ? true : false
}


onMounted(() => {
  getData()
})

</script>


<template>
  <div v-if="openModalFlag && openModalFlag.lastLogin && openModalFlag.pcno && flagTemp" class="modal " id="modal">
    <div class="modal-content rounded-xl text-start ">
      <span @click="closeModal" class="hover:cursor-pointer text-rose-500" style="float:right;">&times;</span>

      <div class="my-10">
        <div class="flex justify-center ">

          <h2 class=" text-2xl text-gray-800 mx-2">Welcome to </h2>
          <span class=" mb-6 text-rose-500 flex justify-center text-2xl"> <img :src="logo" class="mx-1 w-10 h-10"
              alt="ShopNest Logo">ShopNest</span>
        </div>
        <p class="text-md text-gray-700"> <span class="text-black">Last Login: </span> {{ formatDate(new
          Date(openModalFlag.lastLogin)) }}</p>
        <p class="text-md text-gray-700"><span class="text-black">Login Ip: </span> {{ openModalFlag.pcno }}</p>
      </div>


    </div>


  </div>
</template>


<style scoped></style>