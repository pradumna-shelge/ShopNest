import {ref} from 'vue'
import { createToaster } from "@meforma/vue-toaster";
import axios from 'axios';
import {loginusername} from './isLogin'
import {deliveryAddApi} from '../Endpoints/ApiLinks';







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

