<script setup lang="js">
import { ref, onMounted, computed, watch, reactive } from 'vue';
import { useStore } from 'vuex';
import { getCartData, ProductCartDelete, addCartData } from '../Services/cartServices'
import { getApiData, discription, productName } from '../Services/products'

import { getAddresses } from '../Services/deliveryAddress'
import AddressForm from './userOrder/user-address.vue'
const store = useStore();
const cartItems = computed(() => store.getters.cartItems)
const orderFlag = ref(1)
const loadCartData = () => {
  getCartData()
    .then((data) => {
      store.dispatch('addList', data)

    })
    .catch((error) => {

    });
}



const openModal = ref(false);
const DeleteId = ref(0);
const open = (id) => {

  openModal.value = true;
  DeleteId.value = id
}
const closeModal = () => {
  openModal.value = false;
  DeleteId.value = -1
  addNewFlag.value=false;
}




const removeFromCart = () => {

  ProductCartDelete(DeleteId.value).then((d) => {
    getCartData()
      .then((data) => {
        store.dispatch('addList', data)

      })
      .catch((error) => {

      });
    openModal.value = false;

  }).catch((d) => {
    openModal.value = false;
    console.log(d)
  });
}

const updateCount = (name, quantity, incFlg) => {
  const count = incFlg ? quantity + 1 : quantity - 1;

  addCartData(name, count).then((d) => {
    getCartData()
      .then((data) => {
        store.dispatch('addList', data)
      })
      .catch((error) => {

      });

  }).catch((d) => {
    console.log(d)
  });
}
// ============================add address=========
const addAddress=()=>{
  addNewFlag.value=true;
}


// ===================================================checkout======================
const selectedAddress = ref({})
const addNewFlag = ref(false);
const userAddress = ref({})
const ProceedChekout = () => {

  getAddresses().then((d) => {

    orderFlag.value = 2;
    console.log(d);
    userAddress.value = d
  }).catch((d) => {
    console.log(d)
  });
}
async function proceedPay() {
  var d = await $v.value.$validate()

  if (d) {
    orderFlag.value = 3;

    
  }
}
async function ProceedOrder() {
  orderFlag.value = 1;

}

const UpdateAddress=ref({})
const PatchAddress=(item)=>{


  const button = document.getElementById('updateAddress');
  if (button) {
    addNewFlag.value=true;
    UpdateAddress.value=item
    button.click();
  }
}
onMounted(() => {

  loadCartData();


});
</script>


<template>

  <div v-show="orderFlag > 1" class="mx-10 flex justify-center w-full">


    <ol class="flex items-center w-96">
      <li
        class="flex w-full items-center text-blue-600 dark:text-blue-500 after:content-[''] after:w-full after:h-1 after:border-b after:border-blue-100 after:border-4 after:inline-block dark:after:border-blue-800">
        <span
          class="flex items-center justify-center w-10 h-10 bg-blue-100 rounded-full lg:h-12 lg:w-12 dark:bg-blue-800 shrink-0">
          <svg class="w-3.5 h-3.5 text-blue-600 lg:w-4 lg:h-4 dark:text-blue-300" aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 16 12">
            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M1 5.917 5.724 10.5 15 1.5" />
          </svg>
        </span>
      </li>
      <li
        class="flex w-full items-center after:content-[''] after:w-full after:h-1 after:border-b after:border-gray-100 after:border-4 after:inline-block dark:after:border-gray-700">
        <span v-if="orderFlag > 2"
          class="flex items-center justify-center w-10 h-10 bg-blue-100 rounded-full lg:h-12 lg:w-12 dark:bg-blue-800 shrink-0">
          <svg class="w-3.5 h-3.5 text-blue-600 lg:w-4 lg:h-4 dark:text-blue-300" aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 16 12">
            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M1 5.917 5.724 10.5 15 1.5" />
          </svg>
        </span>
        <span v-else
          class="flex items-center justify-center w-10 h-10 bg-gray-100 rounded-full lg:h-12 lg:w-12 dark:bg-gray-700 shrink-0">
          <svg class="w-4 h-4 text-gray-500 lg:w-5 lg:h-5 dark:text-gray-100" aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 16">
            <path
              d="M18 0H2a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2ZM6.5 3a2.5 2.5 0 1 1 0 5 2.5 2.5 0 0 1 0-5ZM3.014 13.021l.157-.625A3.427 3.427 0 0 1 6.5 9.571a3.426 3.426 0 0 1 3.322 2.805l.159.622-6.967.023ZM16 12h-3a1 1 0 0 1 0-2h3a1 1 0 0 1 0 2Zm0-3h-3a1 1 0 1 1 0-2h3a1 1 0 1 1 0 2Zm0-3h-3a1 1 0 1 1 0-2h3a1 1 0 1 1 0 2Z" />
          </svg>
        </span>
      </li>
      <li class="flex items-center w-full">

        <span
          class="flex items-center justify-center w-10 h-10 bg-gray-100 rounded-full lg:h-12 lg:w-12 dark:bg-gray-700 shrink-0">
          <svg class="w-4 h-4 text-gray-500 lg:w-5 lg:h-5 dark:text-gray-100" aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 18 20">
            <path
              d="M16 1h-3.278A1.992 1.992 0 0 0 11 0H7a1.993 1.993 0 0 0-1.722 1H2a2 2 0 0 0-2 2v15a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V3a2 2 0 0 0-2-2ZM7 2h4v3H7V2Zm5.7 8.289-3.975 3.857a1 1 0 0 1-1.393 0L5.3 12.182a1.002 1.002 0 1 1 1.4-1.436l1.328 1.289 3.28-3.181a1 1 0 1 1 1.392 1.435Z" />
          </svg>
        </span>
      </li>
    </ol>

  </div>

  <div v-if="store.getters.cartItemCount > 0" class="  flex justify-center gap-10 ">
    <!-- Cart Count at the Top -->

    <!-- Card Grid -->
    <div class="w-1/3 ">
      <section v-show="orderFlag == 1">


        <p class="text-gray-500 mx-3">You have {{ store.getters.cartItemCount }} items in your cart</p>

        <div class="  flex flex-col ">
          <div v-for="(item, index) in cartItems" :key="item.cartId" class="">
            <div class="flex bg-white shadow-md rounded-md overflow-hidden  m-1 w-card">
              <!-- Left Div with Image -->
              <div class="flex flex-col justify-center">
                <img :src="item.productImage" alt="Product Image" class=" img-w w-full object-cover">
              </div>

              <!-- Right Div with Product Info -->
              <div class=" p-5 flex flex-col justify-between w-96">
                <div class="flex justify-between">
                  <h2 class="text-lg font-semibold text-gray-800">{{ item.productName }}</h2>

                  <div class="flex flex-row h-fit rounded-lg relative bg-transparent mt-1 border rounded">
                    <button :disabled="item.quantity == 1" @click="updateCount(item.productName, item.quantity, false)"
                      data-action="decrement"
                      class=" bg-red-300 text-red-600 hover:text-red-700 hover:bg-red-400  rounded-l cursor-pointer outline-none">
                      <span class="m-auto text-xl px-2">−</span>
                    </button>
                    <p class="px-5">{{ item.quantity }}</p>
                    <button @click="updateCount(item.productName, item.quantity, true)" data-action="increment"
                      class="bg-green-300 text-green-600 hover:text-green-700 hover:bg-green-400  rounded-r cursor-pointer">
                      <span class="m-auto text-xl px-2">+</span>
                    </button>
                  </div>
                </div>
                <div class="text-sm text-gray-600">{{ discription(item.description) }}</div>
                <!-- <div class="mt-2 flex justify-between items-center">
                        <div class="text-md font-semibold text-gray-800">${{ item.price }}</div>
                      </div> -->
                <div class="mt-2 flex justify-between items-center">
                  <button @click="open(item.cartId)"
                    class=" items-center flex gap-1 text-xs text-gray-900 hover:text-white border border-gray-800 hover:bg-red-500 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1 text-center   text-gray-400 hover:text-white  ">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                      stroke="currentColor" class="w-5 h-5">
                      <path stroke-linecap="round" stroke-linejoin="round"
                        d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0" />
                    </svg>Remove item</button>
                  <div class="text-md font-semibold text-gray-800">${{ item.price }}</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
      <section v-show="orderFlag == 2">

        <div class="mt-5">
         

          <div>
           <AddressForm :UpdateAddress="UpdateAddress"  :addNewFlag="addNewFlag" @closeModal="closeModal()" @updateAddress='updateAddress()' ></AddressForm>
          </div>

          <div>
            <div class="flex justify-end m-2  ">
              <button v-show="!addNewFlag" @click="()=>{ addNewFlag=true}" class="bg-gray-800 p-1 rounded text-white text-xs">+ New Address</button>
            </div>
            <div class="p-4 border rounded-lg bg-white">
              <h2 class="text-xl font-semibold mb-4">Shipping Address</h2>
              <div v-for="(item, index) in userAddress" :key="index">
                <div class=" p-3 shadow-md rounded  my-3 flex justify-between item-center">
                  <label class="inline-flex items-center">
                    <input name="delivery" :checked="index==0" type="radio" v-model="selectedAddress" :value="item" class="form-radio text-blue-500" />
                    <span class="ml-2 text-gray-900 text-sm">{{ item.firstName }} {{ item.lastName }},</span>
                    <span class="ml-2 text-gray-700 text-xs">{{ item.address }}, {{ item.city }}, {{ item.state }}, {{
                      item.country }}, {{ item.zip }}</span>
                  </label>

                  <!-- Edit button -->
                  <button v-show="selectedAddress.addressId == item.addressId" @click="PatchAddress(item)"
                    class="text-blue-500 text-xs underline"><svg xmlns="http://www.w3.org/2000/svg" fill="none"
                      viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6">
                      <path stroke-linecap="round" stroke-linejoin="round"
                        d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L10.582 16.07a4.5 4.5 0 01-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 011.13-1.897l8.932-8.931zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0115.75 21H5.25A2.25 2.25 0 013 18.75V8.25A2.25 2.25 0 015.25 6H10" />
                    </svg></button>

                  <!-- Add other billing address details as needed -->
                </div>
              </div>
            </div>


          </div>

          <div class="mt-6">
            <button @click="proceedPay" class="bg-cyan-600 text-white hover:bg-cyan-700 py-2 px-4 rounded-md w-full">
              Proceed to Pay
            </button>
          </div>
        </div>

      </section>
      <section v-show="orderFlag == 3">
        <div class="mt-5 w-full">

          <div class="p-4 border rounded-lg bg-white w-full">
            <h2 class="text-xl font-semibold mb-4">Order Details</h2>


            <div v-for="(item, index) in cartItems" :key="item.cartId">

              <div class="flex items-center bg-white rounded-lg p-4 shadow-md mb-4 ">
                <img class="w-16 h-16 rounded-full mr-4" :src="item.productImage" alt="">
                <div class="w-full">
                  <p class="text-lg font-semibold">{{ item.productName }}</p>
                  <p class="text-gray-600 flex justify-between"> <span>{{ item.quantity }}</span> <span>${{ item.price
                  }}</span> </p>
                </div>
              </div>

            </div>
          </div>
        </div>

        <div class="mt-5">
          <!-- Billing Address -->
          <div class="p-4 border rounded-lg bg-white">
            <h2 class="text-xl font-semibold mb-4">Delivery Address</h2>

            <div class="space-y-2">
              <p><span class="font-semibold"></span> {{ selectedAddress.firstName }} {{ selectedAddress.lastName }},</p>
              <p><span class="text-gray-600"></span> {{ selectedAddress.address }},</p>
              <p><span class="text-gray-600"></span> {{ selectedAddress.city }}, {{ selectedAddress.state }}, {{ selectedAddress.country }}, {{
                selectedAddress.zip }}</p>

              <!-- Add other billing address details as needed -->
            </div>
          </div>


        </div>
      </section>
    </div>
    <div class="mt-5 w-1/6">

      <div class="   p-4 border  rounded-lg bg-white">
        <h2 class="text-xl font-semibold mb-4">Checkout</h2>

        <!-- Subtotal -->
        <div class="flex justify-between mb-2">
          <span class="text-gray-700">Sub Total:</span>
          <span class="text-gray-900">${{ store.getters.cartMrpPrice }}</span>
        </div>

        <!-- Discount (if applicable) -->
        <div class="flex justify-between mb-2">
          <span class="text-gray-700">Discount:</span>
          <span class="text-red-600 text-xs">-${{ store.getters.cartMrpPrice - store.getters.cartTotalPrice < 1 ? 0 :
            parseFloat(store.getters.cartMrpPrice - store.getters.cartTotalPrice).toFixed(2) }}</span>
        </div>

        <!-- Total Amount -->
        <div class="flex justify-between mt-4">
          <span class="text-lg font-semibold">Total:</span>
          <span class="text-lg font-semibold">${{ parseFloat(store.getters.cartTotalPrice.toFixed(1)) }}</span>
        </div>

        <!-- Checkout Button -->
        <button v-show="orderFlag == 1" @click="ProceedChekout()"
          class="mt-4 bg-cyan-600 text-white hover:bg-cyan-700 py-2 px-4 rounded-md w-full">
          Proceed to Checkout
        </button>

        <!-- <button v-show="orderFlag == 2" @click="ProceedPay()" class="mt-4 bg-cyan-600 text-white hover:bg-cyan-700 py-2 px-4 rounded-md w-full">
            Proceed to Pay
          </button> -->
        <button v-show="orderFlag == 3" @click="ProceedOrder()"
          class="mt-4 bg-cyan-600 text-white hover:bg-cyan-700 py-2 px-4 rounded-md w-full">
          Order Now
        </button>
      </div>
    </div>
  </div>
  <div v-else class="flex flex-col gap-5">

    <div class="  flex justify-center gap-10 ">
      <img src="../assets/empty-cart.png" alt="">


    </div>
    <div class="  flex justify-center gap-10  ">
      <button class="bg-sky-500 hover:bg-sky-700 text-white py-2 rounded px-10 "> <router-link to="/">Shop
          Now</router-link> </button>

    </div>
  </div>


  <div v-if="openModal" class="modal ">
    <div class="modal-content rounded-xl text-center">
      <span @click="closeModal" class="hover:cursor-pointer text-rose-500" style="float:right;">&times;</span>

      <h2 class="text-xl">Remove Item</h2>
      <p class="text-gray-500 ">Are you sure you want to remove this item?</p>
      <div class="modal-buttons flex justify-center p-5">
        <button @click="removeFromCart"
          class="text-white bg-gradient-to-r from-red-400 via-red-500 to-red-600 hover:bg-gradient-to-br focus:ring-4 focus:outline-none focus:ring-red-300 focus:ring-red-800 shadow-lg shadow-red-500/50 shadow-lg shadow-red-800/80 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2">Remove</button>
        <button @click="closeModal"
          class="text-white bg-gradient-to-r from-gray-400 via-gray-500 to-gray-600 hover:bg-gradient-to-br focus:ring-4 focus:outline-none focus:ring-gray-300 focus:ring-gray-800 shadow-lg shadow-gray-500/50 shadow-lg shadow-gray-800/80 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2">Cancel</button>
      </div>
    </div>
  </div>
</template>


<style scoped>
.img-w {
  width: 150px;
  height: 146px;

}

.left {
  width: 170px;
  height: 146px;
}

.w-card {
  max-width: 100%;
  height: auto;
  padding: 3px;
}

.left-div {
  width: -webkit-fill-available;
  overflow-y: scroll;
  /* Enable vertical scrolling for the left div */
}

.right-div {
  width: 300px;
  /* Set a fixed width for the right div */
  position: fixed;
  left: 70%;
  top: 19%;
  /* Stick the right div to the top of the container */
  /* You can also add other styles like background color, padding, etc. */
}</style>