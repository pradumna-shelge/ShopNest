<script setup lang='js'>
import router from '../Router';
import {getApiData} from '../Services/products'
import { ref, onMounted } from 'vue';

const { getData, data, error, flag } = getApiData();
const url = ref("https://localhost:7059/api/Products");
let intervalId; 

const asyncFunction = async () => {
    await getData(url.value);
    if (flag) {
        clearInterval(intervalId);
        router.push('/');
    }
    console.log("checking for server response");
};

onMounted(() => {
    intervalId = setInterval(() => {
        asyncFunction();
    }, 5000);
});
</script>
<template>
    <div class="mt-10">
        <section class="bg-white dark:bg-gray-900">
            <div class="py-8 px-4 mx-auto max-w-screen-xl lg:py-16 lg:px-6">
                <div class="mx-auto max-w-screen-sm text-center">
                    <h1 class="mb-4 text-7xl tracking-tight font-extrabold lg:text-9xl text-white dark:text-white">500</h1>
                    <p class="mb-4 text-3xl tracking-tight font-bold text-gray-900 md:text-4xl dark:text-white">Internal
                        Server Error.</p>
                    <p class="mb-4 text-lg font-light text-gray-500 dark:text-gray-400">We are already working to solve the
                        problem. </p>
                </div>
            </div>
        </section>
    </div>
</template>
<style scoped></style>
