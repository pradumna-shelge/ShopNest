<script setup lang='js'>
import { ref, onMounted } from 'vue';
import navbar from './components/navbar.vue'
import footer1 from './components/footer.vue'


const loginFlag = ref(false)
const handleLoginSuccess = () => {
console.log("called in main vue app")
console.log(dis());
if(dis()){
  loginFlag.value=true;
  console.log(loginFlag.value);
}
  else{
    loginFlag.value= false;
     console.log(loginFlag.value);
  }
}

function  dis(){
return (localStorage.getItem('userInfo') != null);
}
onMounted(() => {
  handleLoginSuccess();
});
</script>

<template>

    <navbar :flag="loginFlag" @login-success1="handleLoginSuccess"></navbar>

    <div class="min-h-screen flex flex-col " >
      <main class="flex-grow py-20 ">
        <router-view  @login-success="handleLoginSuccess"></router-view> 
        
      </main>
      
      <footer class="bg-gray-300 py-4">
        
        <footer1></footer1> 
    </footer>
  </div>

</template>



<style scoped>
</style>