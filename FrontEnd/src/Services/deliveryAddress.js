import {ref} from 'vue'
import { createToaster } from "@meforma/vue-toaster";
import axios from 'axios';
import {loginusername} from './isLogin'
import {deliveryAddApi,orderProductApi} from '../Endpoints/ApiLinks';


export const getAddresses = () => {
   debugger;
  return new Promise(async (resolve, reject) => {
    const userName = loginusername.value;
    const url = deliveryAddApi + `/${userName}`;
    try {
      const res = await axios.get(url);
      console.log(res.data);
      resolve(res.data); 
    } catch (err) {
      console.error("error", err);
      reject(err); 
    }
  });
};

  export async function UpdateAddress(addressObj) {
    debugger;
  const url = deliveryAddApi;

return new Promise(async (resolve,reject)=>{

    try {
      const res = await axios.put(url, {
         userId: addressObj.userId,
    addressId: addressObj.addressId, 
    country: addressObj.country.trim(),
    state: addressObj.state.trim(),
    city: addressObj.city.trim(),
    firstName: addressObj.firstName.trim(),
    lastName: addressObj.lastName.trim(),
    address: addressObj.address.trim(),
    zip: addressObj.zip
      });
     
      if (res.status === 200) {
          
          resolve(data); 
        } else {
          reject('Error adding address!'); 
        }
      } catch (err) {
        console.error(err);
        reject(err); 
      }
})
  };



    export async function AddAddress(addressObj) {
    debugger;
  const url = deliveryAddApi;
 const userName = loginusername.value;
return new Promise(async (resolve,reject)=>{

    try {
      const res = await axios.post(url, {
         username: userName.trim(),
    country: addressObj.country.trim(),
    state: addressObj.state.trim(),
    city: addressObj.city.trim(),
    firstName: addressObj.firstName.trim(),
    lastName: addressObj.lastName.trim(),
    address: addressObj.address.trim(),
    zip: addressObj.zip
      });
     
      if (res.status === 200) {
          
          resolve(data); 
        } else {
          reject('Error adding address!'); 
        }
      } catch (err) {
        console.error(err);
        reject(err); 
      }
})
  };


  
