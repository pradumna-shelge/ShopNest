import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import router from './Router';
import Toaster from "@meforma/vue-toaster";
import {Vue3TailwindPagination} from "vue-3-tailwind-css-pagination";
import homepage from './components/HomePage.vue';
import  provideCart from './state/cart.js'
const app = createApp(App);


app.component('Vue3TailwindPagination', Vue3TailwindPagination);
app.component('your-component', homepage); 
app.use(provideCart)
app.use(router); 
app.use(Toaster, {
  position: "top",
});


app.mount('#app');

