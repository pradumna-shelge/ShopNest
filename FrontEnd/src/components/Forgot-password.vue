<script setup lang="js">
import { ref, reactive, computed,provide } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import { CustomValidationMsg } from '../Vadidators/index';
import { required } from '@vuelidate/validators';
import { forgotPassword } from '../Services/islogin'
const forgotPasswordFormData = reactive({
    username: '',
});
const modalForgot = ref(false);
let timer = null
const { childRef }=defineProps(['childRef']);
const emits = defineEmits();
const sentFlag = ref(true);
const closeModal = () => {
    forgotPasswordFormData.username=''
    $vForgotPassword.value.$reset();
    if(timer)clearInterval(timer);
    remainingTime.value = 5;
    sentFlag.value =true;
    emits("closeModal")

};
const forgotPasswordRules = computed(() => {
    return {
        username: { required },
    };
});

const remainingTime = ref(5); 


const startCountdown = () => {
    const intervalId = setInterval(() => {
        remainingTime.value--; 
        if (remainingTime.value <= 0 ) {
            clearInterval(intervalId);
            closeModal(); 
        }
    }, 1000); 
    timer = intervalId
};
const $vForgotPassword = useVuelidate(forgotPasswordRules, forgotPasswordFormData);

const resetPassword = async () => {
    try {
        const valid = await $vForgotPassword.value.$validate();
        if (valid) {
            await forgotPassword(forgotPasswordFormData.username).then((d)=>{
                sentFlag.value=false

                startCountdown()
            });
        }
        forgotPasswordFormData.username = ''
    } catch (error) {
        console.error('Error:', error);
    }
};



</script>


<template>
 

     <div  class="modal "  v-show="childRef">
                      <div class="modal-content rounded-xl text-center ">
                             <span @click="closeModal" class="hover:cursor-pointer text-rose-500" style="float:right;">&times;</span>
           
                             <div class="">
                                <div class="" v-show="sentFlag">
                  <div class="flex flex-col items-center justify-center px-6 mx-auto lg:py-0">
                <div
                    class="w-full ">
                    <div class="p-6 space-y-4 md:space-y-6 sm:p-8">
                        <h1
                            class="text-center text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
                            Reset Password
                        </h1>

                        <form class="space-y-4 md:space-y-6" @submit.prevent="resetPassword">
                            <div>
                                <input type="text" v-model="forgotPasswordFormData.username"
                                    class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                    placeholder="Enter your username">
                                <span class="text-red-400 text-xs block" v-for="error in $vForgotPassword.username.$errors"
                                    :key="error">
                                    {{ CustomValidationMsg(error.$message, "Username"), error.$message }}
                                </span>
                            </div>

                            <button type="submit"
                                class="w-full text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                                Continue
                            </button>
                        </form>
                    </div>  
                </div>
        </div>

                                </div>
                                <div v-show="!sentFlag"> 

                                    <h1 class="mb-5 text-2xl text-gray-800">Forgot Password</h1>
                                    <p class=" mb-3 text-md text-gray-700">Please check your email to Reset Password</p>
    
                                    <!-- <p class="text-xs text-sky-500">This email may take a few minutes to arrive in your inbox. </p> -->
          <p class="text-xs text-dark">This page will close in <span class="text-red-500">{{ remainingTime }}</span>  seconds.</p>
                             </div>
                             </div>
                             <div class="m-3 flex justify-end">
                                <span class="text-sky-500 text-xs cursor-pointer" @click="closeModal">Return to login page</span>
                             </div>

                      </div>
                    
                
                    </div>

</template>
