import {ref} from 'vue'
import { createToaster } from "@meforma/vue-toaster";
import axios from 'axios';
import {loginusername} from './isLogin'
import {cartApi} from '../Endpoints/ApiLinks';


const toaster = createToaster({
  /* options */
});

export const getCartData = () => {
  return new Promise(async (resolve, reject) => {
    const userName = loginusername.value;
    const url = cartApi + `?userName=${userName}`;
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



export const addCartData = (productName, quantity) => {
  return new Promise(async (resolve, reject) => {
    const userName = loginusername.value;
    const url = cartApi;
 try {
    const res = await axios.post(url, {
    username: userName,
    productName: productName,
    quantity: quantity,
  });
if (res.status === 200) {
        const data = res.data.imageUrl;
        resolve(data); 
      } else {
        reject('Error updating product!'); 
      }
    } catch (err) {
      console.error(err);
      reject(err); 
    }
  });
}



export async function ProductCartDelete(id) {
  const url = cartApi;
  debugger;
  return new Promise(async (resolve, reject) => {
    try {
      
      const res = await axios.delete(`${url}/${id}`);

      if (res.status === 200) {
        resolve('Item removed from cart successfully.');
        toaster.success('Item removed from cart successfully.');
      } else {
        reject('Error removed item from cart !');
        toaster.error('Error removed item from cart !');
      }
    } catch (err) {
      console.error(err);
      toaster.error(err.response.data + "!");
      reject(err);
    }
  });
}
