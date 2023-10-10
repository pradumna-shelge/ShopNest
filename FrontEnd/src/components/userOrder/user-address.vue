<script setup lang='js'>
import { ref, onMounted, computed, watch, reactive } from 'vue';

import { location } from '../../Services/location';
import { CustomValidationMsg, passwordValidator } from '../../Vadidators/index'
import { email, minLength, required } from '@vuelidate/validators';
import { useVuelidate } from '@vuelidate/core';

const props =defineProps(['addNewFlag', 'UpdateAddress'])
const emit = defineEmits()
const closeModal = () => {
    reset();
    emit('closeModal')
}
const UpdateFlag = ref(false);
const reset = () => {
    UpdateFlag.value = false;
    document.getElementById('addressForm').reset();
    $v.value.$reset();
    formData.country = null;
    formData.state = '';
    formData.city = '';
    formData.firstName = '';
    formData.lastName = '';
    formData.address = '';
    formData.zip = '';
    states.value = null
    cities.value = null

}
const countries = ref(location);

let formData = reactive({
    userId:-1,
    addressId:-1,
    country: '',
    state: '',
    city: '',
    firstName: '',
    lastName: '',
    address: '',
    zip: ''
});

const rules = computed(() => {
    return {
            userId:{},
            addressId:{},
        country: { required },
        state: { required },
        city: { required },
        firstName: { required },
        lastName: { required },
        address: { required },
        zip: { required }

    };
});
const $v = useVuelidate(rules, formData);


const states = ref([]);
const cities = ref([]);

watch(() => formData.country, (newCountry) => {

    const country = countries.value.find((c) => c.Name === newCountry);
    if (country) {
        states.value = country.Children.filter((state) => state.Name);
        console.log(states.value);
        formData.state = '';
        formData.city = '';
        cities.value = null
    }
}, { deep: true });


watch(() => formData.state, (newState) => {

    const countryName = formData.country;
    const country = countries.value.find((c) => c.Name === countryName);
    if (country) {
        const state = country.Children.find((s) => s.Name === newState);
        if (state) {
            cities.value = state.Children.filter((city) => city.Name);
            formData.city = '';
        }
    }
}, { deep: true });


async function SaveAddress() {
    var d = await $v.value.$validate()

    if (d) {
    }
}

const update= () => {
    debugger;
    UpdateFlag.value=true;
console.log(props.UpdateAddress, props.addNewFlag);
    formData= props.UpdateAddress;
}

</script>






<template >
    <button id="updateAddress" @click="update"></button>

    <div v-show="addNewFlag" class="modal ">
        <div class="modal-content1 rounded-xl text-center">
            <span @click="closeModal" class="hover:cursor-pointer text-rose-500" style="float:right;">&times;</span>


            <div class=" text-start ">
                <h2 class="text-xl font-semibold mb-4">Shipping Address</h2>

                <!-- Billing Address Form -->
                <form id="addressForm" @submit.prevent="SaveAddress">
                    <!-- Billing Address Fields -->
                    <div class="grid grid-cols-2 gap-4">
                        <!-- First Name -->
                        <div class=" col-span-1">
                            <label for="first_name" class="block text-sm font-medium text-gray-700">First Name</label>
                            <input v-model="formData.firstName" type="text" id="first_name" name="first_name"
                                class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm py-1 sm:text-sm border rounded-md">
                            <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.firstName.$errors">{{
                                CustomValidationMsg(error.$message, "first name") }}</span>

                        </div>

                        <!-- Last Name -->
                        <div class=" col-span-1">
                            <label for="last_name" class="block text-sm font-medium text-gray-700">Last Name</label>
                            <input v-model="formData.lastName" type="text" id="last_name" name="last_name"
                                class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm py-1 sm:text-sm border rounded-md">
                            <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.lastName.$errors">{{
                                CustomValidationMsg(error.$message, "last name") }}</span>

                        </div>

                        <!-- Address Line 1 -->
                        <div class=" col-span-2">
                            <label for="address" class="block text-sm font-medium text-gray-700">Address</label>
                            <input v-model="formData.address" type="text" id="address" name="address"
                                class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm py-1 sm:text-sm border rounded-md">
                            <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.address.$errors">{{
                                CustomValidationMsg(error.$message, "address") }}</span>

                        </div>

                        <!-- Country -->
                        <div class=" col-span-1">
                            <label for="country" class="block text-sm font-medium text-gray-700">Country</label>
                            <select v-model="formData.country" id="country" name="country"
                                class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm py-1 sm:text-sm border rounded-md">
                                <option v-for="country in countries" :key="country.LocationID" :value="country.Name">{{
                                    country.Name
                                }}</option>
                            </select>
                            <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.country.$errors">{{
                                CustomValidationMsg(error.$message, "country") }}</span>

                        </div>

                        <!-- State/Province -->
                        <div class=" col-span-1">
                            <label for="state" class="block text-sm font-medium text-gray-700">State/Province</label>
                            <select v-model="formData.state" id="state" name="state"
                                class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm py-1 sm:text-sm border rounded-md">
                                <option v-for="state in states" :key="state.LocationID" :value="state.Name">{{ state.Name }}
                                </option>
                            </select>
                            <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.state.$errors">{{
                                CustomValidationMsg(error.$message, "state") }}</span>

                        </div>

                        <!-- City -->
                        <div class=" col-span-1">
                            <label for="city" class="block text-sm font-medium text-gray-700">City</label>
                            <select v-model="formData.city" id="city" name="city"
                                class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm py-1 sm:text-sm border rounded-md">
                                <option v-for="city in cities" :key="city.LocationID" :value="city.Name">{{ city.Name }}
                                </option>
                            </select>
                            <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.city.$errors">{{
                                CustomValidationMsg(error.$message, "city") }}</span>

                        </div>

                        <!-- ZIP/Postal Code -->
                        <div class=" col-span-1">
                            <label for="zip" class="block text-sm font-medium text-gray-700">ZIP/Postal Code</label>
                            <input v-model="formData.zip" type="text" id="zip" name="zip"
                                class="mt-1 focus:ring-cyan-500 focus:border-cyan-500 block w-full shadow-sm py-1 sm:text-sm border rounded-md">
                            <span class="text-red-400 text-xs text-end text-right" v-for="error in $v.zip.$errors">{{
                                CustomValidationMsg(error.$message, "zip") }}</span>

                        </div>


                    </div>
                    <div class="flex justify-center col-span-2 row mt-3">
                        <button v-if="!UpdateFlag" type="submit"
                            class="w-full btn text-green-700 hover:text-white border border-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-green-500 text-green-500 hover:text-white hover:bg-green-600 focus:ring-green-900">
                            Save Address
                        </button>


                        <button @click="updateUser()" v-if="UpdateFlag" type="button"
                            class="w-full  btn text-cyan-700 hover:text-white border border-cyan-700 hover:bg-cyan-800 focus:ring-4 focus:outline-none focus:ring-cyan-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-cyan-500 text-cyan-500 hover:text-white hover:bg-cyan-600 focus:ring-cyan-900">
                            Update Address
                        </button>
                        <button @click="reset" type="button"
                            class="w-full btn text-red-700 hover:text-white border border-red-700 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mr-2 mb-2 border-red-500 text-red-500 hover:text-white hover:bg-red-600 focus:ring-red-900">{{
                                UpdateFlag ? 'Cancel' : 'Reset' }}</button>


                    </div>
                </form>
            </div>
        </div>
    </div>
</template>





<style scoped></style>