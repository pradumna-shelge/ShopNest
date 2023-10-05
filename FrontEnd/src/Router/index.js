import {createRouter, createWebHistory } from 'vue-router'
import homepage from '../components/HomePage.vue'
import login from '../components/login.vue'
import error from '../components/error.vue'
import signup from '../components/signUp.vue'
import dashboard from '../components/Admin/dashboard.vue'
import ForgotPasswordVue from '../components/ForgotPassword.Vue'
import userDashboard from '../components/Admin/UserDashbord.vue'
import productDetail from '../components/productDetail.vue'
import usercart from '../components/userCart.vue'
const routes = [
    {
        path:'/',
        name:'homepage',
        component:homepage
    },
    {
  path:'/login',
        name:'login',
        component:login,
             beforeEnter: (to, from, next) => {
    
      const isAuthenticated = localStorage.getItem('userInfo') != null; 
      
      if (!isAuthenticated) {
       
        next();
      } else {
       
        next({ name: 'homepage' });
      }
    }
    }, {
  path:'/signup',
        name:'signup',
        component:signup,
             beforeEnter: (to, from, next) => {
    
      const isAuthenticated = localStorage.getItem('userInfo') != null; 
      
      if (!isAuthenticated) {
       
        next();
      } else {
       
        next({ name: 'homepage' });
      }
    }
    },
    {
  path:'/error',
        name:'error',
        component:error
    },
    {
    path: '/:nameOfProduct',
    name: 'product',
    component: productDetail,
    props: true,
  },
    {
         path:'/dashboard',
        name:'dashboard',
        component:dashboard,
        beforeEnter: (to, from, next) => {
    
      const isAuthenticated = localStorage.getItem('userInfo') != null; 
      
      if (isAuthenticated) {
       
        next();
      } else {
       
        next({ name: 'login' });
      }
    }
    },
    {
         path:'/product-cart',
        name:'usercart',
        component:usercart,
        beforeEnter: (to, from, next) => {
    
      const isAuthenticated = localStorage.getItem('userInfo') != null; 
      
      if (isAuthenticated) {
       
        next();
      } else {
       
        next({ name: 'login' });
      }
    }
    },
      {
         path:'/user-dashboard',
        name:'user-dashboard',
        component:userDashboard,
        beforeEnter: (to, from, next) => {
    
      const isAuthenticated = localStorage.getItem('userInfo') != null; 
      
      if (isAuthenticated) {
       
        next();
      } else {
       
        next({ name: 'login' });
      }
    }
    },
    {
      path:'/forgot-password',
      name:ForgotPasswordVue,
      component:ForgotPasswordVue

    }
]

const router=createRouter({
    history: createWebHistory(), routes,
    scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      
      return savedPosition
    } else if (to.hash) {
      
      return { selector: to.hash }
    } else {
    
      return { top: 0 }
    }
  },
})

export default router