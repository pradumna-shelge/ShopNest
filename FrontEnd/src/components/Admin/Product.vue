<script setup lang="js">
import { ref } from 'vue';

const openModal = ref(false);

const open = ()=>{
    openModal.value = !openModal.value
}


</script>

<template>
   <button @click="open()">Open Modal</button>

   
    <div  v-if="openModal" class="modal">

     
      <div class="modal-content">
        <span @click="open()" style="float:right;">&times;</span>
        <h2>Modal Header</h2>
        <p>This is a basic modal example.</p>
      </div>

    </div>
</template>

<style scoped>

</style>
