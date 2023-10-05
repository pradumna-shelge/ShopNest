import {ref} from 'vue'
import axios from 'axios';
import router from '../Router';
const data=ref([])
const error=ref([])
const flag =ref(false) ;
import { userApi, loginApi,secretKey } from "../Endpoints/ApiLinks";
export  function getUsers()
{
     const url = userApi;
    data.value=[]
    error.value=[]

    const getData=async ()=>{
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


export async function UserDelete(id) {
    const url = userApi;
  return new Promise(async (resolve, reject) => {
    try {
      
      const res = await axios.delete(`${url}/${id}`);

      if (res.status === 200) {
        resolve('User deleted successfully.');
      } else {
        reject('Error deleting user!');
      }
    } catch (err) {
      console.error(err);
      reject(err);
    }
  });
}


export async function AddUser(username, email, password,UserRole) {
  const url = userApi;
debugger;
return new Promise(async (resolve,reject)=>{

    try {
      const res = await axios.post(url, {
        username: username,
        email: email,
        passwordHash: password,
           userRole:(UserRole==-1||UserRole==2)?2:1
      });
     
      if (res.status === 200) {
          const data = res.data.imageUrl;
          resolve(data); 
        } else {
          reject('Error adding user!'); 
        }
      } catch (err) {
        console.error(err);
        reject(err); 
      }
})
  };


  export async function UpdateUser(id,username, email, password,UserRole) {
  const url = userApi;
debugger;
return new Promise(async (resolve,reject)=>{

    try {
      const res = await axios.put(url, {
        userId : id,
        username: username,
        email: email,
        passwordHash: password,
        userRole:(UserRole==-1||UserRole==2)?2:1
      });
     
      if (res.status === 200) {
          const data = res.data.imageUrl;
          resolve(data); 
        } else {
          reject('Error adding user!'); 
        }
      } catch (err) {
        console.error(err);
        reject(err); 
      }
})
  };