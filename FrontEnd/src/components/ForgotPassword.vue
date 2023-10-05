<script setup lang='js'>

import router from '../Router'
import { loginUser } from '../Services/islogin.js'
import { ref, reactive, computed } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import { CustomValidationMsg } from '.././Vadidators/index'

import { email, minLength, required,sameAs } from '@vuelidate/validators';

const showForgotPasswordForm = ref(false);

const forgotPasswordFormData = reactive({
   
    username: '',
    newPassword: '',
    confirmPassword: '',
});


const forgotPasswordRules = computed(() => {
    return {
       
        username: { required },
        newPassword: { required },
        confirmPassword: {
            required,
            sameAsNewPassword: sameAs(forgotPasswordFormData.newPassword),
        },
    };
});

const $vForgotPassword = useVuelidate(forgotPasswordRules, forgotPasswordFormData);

const resetPassword = async () => {

    try {
        const valid = await $vForgotPassword.value.$validate();
        if (valid) {
            // Perform reset password logic here
            // ...
          
        }
    } catch (error) {
        console.error('Error:', error);
    }
};
const img="data:image/jpeg;base64,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"
</script>
<template>
    <a :href="img" class="MagicZoom" data-options="cssClass: yourname"><img :src="img">
    </a>
    <!-- <a href="big-image.jpg" class="MagicZoom"><img src="small-image.jpg"></a> -->
    <!-- <section class="mt-6">
        <div class="flex flex-col items-center justify-center px-6 mx-auto lg:py-0">
            <div
                class="w-full bg-gray-50 hover:bg-gray-100 hover:border-2 hover:shadow-xl rounded-lg border md:mt-0 sm:max-w-md xl:p-0">
                <div class="p-6 space-y-4 md:space-y-6 sm:p-8">
                    <h1
                        class="text-center text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
                        Reset Password
                    </h1>

                    <form class="space-y-4 md:space-y-6" @submit.prevent="resetPassword">
                       
                        <div>
                            <input type="text" v-model="forgotPasswordFormData.username" 
                                class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                placeholder="Enter your email">
                            <span class="text-red-400 text-xs block" v-for="error in $vForgotPassword.username.$errors" :key="error">
                                {{ CustomValidationMsg(error.$message, "Username"),error.$message }}
                            </span>
                        </div>

                    
                        <div>
                            <input type="password" v-model="forgotPasswordFormData.newPassword" id="newPassword"
                                class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                placeholder="Enter new password">
                            <span class="text-red-400 text-xs block" v-for="error in $vForgotPassword.newPassword.$errors" :key="error">
                                {{ CustomValidationMsg(error.$message, "New Password") }}
                            </span>
                        </div>

                      
                        <div>
                            <input type="password" v-model="forgotPasswordFormData.confirmPassword" id="confirmPassword"
                                class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                placeholder="Confirm new password">
                            <span class="text-red-400 text-xs block" v-for="error in $vForgotPassword.confirmPassword.$errors"
                                :key="error">
                                {{ CustomValidationMsg(error.$message, "Confirm Password") }}
                            </span>
                        </div>

                        <button type="submit"
                            class="w-full text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                            Reset Password
                        </button>
                        <p class="text-sm font-light text-gray-500 dark:text-gray-400">
                              Return to  <a class="font-medium text-primary-600 hover:underline dark:text-primary-500"> <router-link to="/signup" >login</router-link> </a>
                          </p>
                    </form>
                </div>
            </div>
        </div>
    </section> -->
</template>

<style scoped>
</style>