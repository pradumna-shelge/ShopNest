

<script setup lang="js">
import { ref, reactive,computed } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import  {CustomValidationMsg,passwordValidator} from '.././Vadidators/index'
import { email, minLength, required } from '@vuelidate/validators';
import { isLogin, setUserInfo, loginUser,AddUser } from '../Services/islogin.js'
let formData = reactive({
    username: '',
    password: '',
    email: '',
});

const rules = computed(() => {
    return {
       username: { required },
        password: { required, passwordValidator },
        email: { required, email },

    };
});
const $v = useVuelidate(rules, formData);

async function signUp() {
      var d = await $v.value.$validate()

    if (d) {
        
         await AddUser(formData.username, formData.email, formData.password);
    }
}



const showPassword = ref(false)
const passwordStrengthText = ref('')
const passwordStrength = ref(-1)
const passwordToggle = () => {
  showPassword.value = !showPassword.value
}
const checkPasswordStrength = () => {
  const password = formData.password;
  const strength = calculatePasswordStrength(password);
  if (password == "") passwordStrength.value = -1
  else {

    passwordStrength.value = strength;
  }


  passwordStrengthText.value = getPasswordStrengthText(strength);
}
const calculatePasswordStrength = (password) => {
  debugger;
  const uppercaseRegex = /[A-Z]/;
  const lowercaseRegex = /[a-z]/;
  const numberRegex = /[0-9]/;
  const specialCharacterRegex = /[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]/;

  const passwordLength = password.length;
  let strength = 0;

  if (passwordLength >= 8 && passwordLength <= 20) strength += 30;
  if (uppercaseRegex.test(password)) strength += 20;
  if (lowercaseRegex.test(password)) strength += 20;
  if (numberRegex.test(password)) strength += 20;
  if (specialCharacterRegex.test(password)) strength += 10;

  return strength;
}

const getPasswordStrengthText = (strength) => {
  if (strength >= 100) {
    return 'Strong';
  } else if (strength >= 50) {
    return 'Moderate';
  } else {
    return 'Weak';
  }
};
const bgcolor = () => {
  if (passwordStrength.value >= 100) {
    return 'bg-green-500';
  } else if (passwordStrength.value >= 50) {
    return 'bg-yellow-500';
  } else {
    return 'bg-red-500';
  }
};
</script>

<template>
    <section>
         <div class="flex flex-col items-center justify-center px-6 py-8 mx-auto  lg:py-0 ">
        <div class="w-full bg-gray-50 hover:bg-gray-100 hover:border-2 hover:shadow-xl rounded-lg border  md:mt-0 sm:max-w-md xl:p-0  ">
          <div class="p-6 space-y-4 md:space-y-6 sm:p-8">
            <h1 class=" text-center text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
              Sign Up
            </h1>
            <form class="space-y-4 md:space-y-6" @submit.prevent="signUp">
              <div>
                <!-- <label for="username" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Your Username</label> -->
                <input v-model="formData.username" type="text" id="username" class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg -gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Enter your username">
                <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.username.$errors">{{ CustomValidationMsg(error.$message,"Username")  }}</span>
              </div>
              <div>                            
<div class="relative border border-gray-300 rounded-lg focus:ring-primary-600 focus:border-primary-600">
        <input
          maxlength="80"
          :class="{ 'border-2 border-rose-600 ': $v.password.$error }"
          id="product"
          v-model="formData.password"
          :type="showPassword ? 'text' : 'password'"
          class="block w-full p-2.5 border-0 text-gray-900 sm:text-sm focus:ring-blue-500 focus:border-blue-500"
          placeholder="Enter your Password"
           @input="checkPasswordStrength()"
        />
   
        <button type="button"
          @click="passwordToggle"
          class="absolute top-1/2 right-3 transform -translate-y-1/2"
        >
          <svg v-if="showPassword" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
              <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0z"/>
      <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8zm8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7z"/>

          </svg>
          <svg v-if="!showPassword" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-slash-fill" viewBox="0 0 16 16">
              <path d="m10.79 12.912-1.614-1.615a3.5 3.5 0 0 1-4.474-4.474l-2.06-2.06C.938 6.278 0 8 0 8s3 5.5 8 5.5a7.029 7.029 0 0 0 2.79-.588zM5.21 3.088A7.028 7.028 0 0 1 8 2.5c5 0 8 5.5 8 5.5s-.939 1.721-2.641 3.238l-2.062-2.062a3.5 3.5 0 0 0-4.474-4.474L5.21 3.089z"/>
      <path d="M5.525 7.646a2.5 2.5 0 0 0 2.829 2.829l-2.83-2.829zm4.95.708-2.829-2.83a2.5 2.5 0 0 1 2.829 2.829zm3.171 6-12-12 .708-.708 12 12-.708.708z"/>

          </svg>
        </button>
      </div>
      <div>

            <div v-if="passwordStrength!=-1"  id="password-strength">
          <div :class="bgcolor()" class="  text-xs text-center text-white" :style="{ width: passwordStrength + '%' }" id="password-strength-bar">{{ passwordStrengthText }}</div>
        </div>
      </div>
                <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.password.$errors">{{ CustomValidationMsg(error.$message, "Password")  }}</span>
              </div>
              <div>
                <!-- <label for="email" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Email</label> -->
                <input v-model="formData.email" type="email" id="email" class="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg -gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Enter your email address">
                <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.email.$errors">{{ CustomValidationMsg(error.$message, "Email address")  }}</span>
              </div>
              <button type="submit" class="w-full text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">Sign Up</button>
            <p class="text-sm font-light text-gray-500 dark:text-gray-400">
    Already have an account? <a class="font-medium text-primary-600 hover:underline dark:text-primary-500" ><router-link to="/login" >Log in</router-link></a>
  </p>

            </form>
          </div>
        </div>
      </div>
    </section>
</template>

