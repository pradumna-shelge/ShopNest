import {ref} from 'vue'
import axios from 'axios';
import router from '../Router';
const data=ref([])
const error=ref([])
const flag =ref(false) ;

export  function getApiData()
{
    data.value=[]
    error.value=[]

    const getData=async (url)=>{
        try{
            let res= await axios(url)
            data.value= res.data;
            flag.value=true
        }
        catch(err)
        {
            console.log("error", err);
            error.value=err.message;
        }
    }

    return {data, error, getData,flag}
}


export  async function ProductAdd(obj,url) {
  return new Promise(async (resolve, reject) => {
    try {
      

      const res = await axios.post(url, obj);

      if (res.status === 200) {
        const data = res.data.imageUrl;
        resolve(data); 
      } else {
        reject('Error adding product!'); 
      }
    } catch (err) {
      console.error(err);
      reject(err); 
    }
  });
}


export  async function ProductUpdate(obj,url) {
  return new Promise(async (resolve, reject) => {
    try {
      

      const res = await axios.put(url, obj);

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

export async function ProductDelete(id, url) {
  return new Promise(async (resolve, reject) => {
    try {
      
      const res = await axios.delete(`${url}/${id}`);

      if (res.status === 200) {
        resolve('Product deleted successfully.');
      } else {
        reject('Error deleting product!');
      }
    } catch (err) {
      console.error(err);
      reject(err);
    }
  });
}
export function discription(dis){

  if(dis.length>70){
    return dis.substring(0,70)+"...";
  }
  else{
    return dis;
  }

   
}

export function productName(dis){

  if(dis.length>20){
    return dis.substring(0,20)+"...";
  }
  else{
    return dis;
  }

   
}