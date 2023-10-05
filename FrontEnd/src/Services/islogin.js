import { ref } from "vue";
import { userApi, loginApi,secretKey } from "../Endpoints/ApiLinks";
import axios from "axios";
import router from "../Router";
import CryptoJS from "crypto-js";

const loginusername = ref(null);
const loginRole = ref(null)

export { loginusername,loginRole };
export const isLogin = localStorage.getItem("userInfo") != null;

export const setuserData = () => {
  try {
    let data = localStorage.getItem("userInfo");
    var base64Payload = data.split(".")[1];
    var payload = decodeURIComponent(
      atob(base64Payload)
        .split("")
        .map(function (c) {
          return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join("")
    );
    console.log(JSON.parse(payload));
    const decodedToken = JSON.parse(payload);

    loginusername.value =
      decodedToken[
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
      ];

      loginRole.value =decodedToken[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];

    console.log("Username:", loginusername.value);
    console.log("userRole",loginRole.value);
  } catch (error) {
    console.error("Token decoding failed:", error);
  }
};
export let setUserInfo = (data,loginData) => {
  localStorage.setItem("userInfo", data);
  localStorage.setItem("loginInfo", JSON.stringify(loginData) )
  setuserData();
};

import { createToaster } from "@meforma/vue-toaster";

const toaster = createToaster({
  /* options */
});
export let loginUser = async (username, password) => {
  const url = loginApi;
const key =CryptoJS.enc.Utf8.parse(secretKey) ;

  
  const encryptedUsername = CryptoJS.AES.encrypt(username, key, {
    mode: CryptoJS.mode.ECB,
    padding: CryptoJS.pad.Pkcs7,
  }).toString();

  const encryptedPassword = CryptoJS.AES.encrypt(password, key, {
    mode: CryptoJS.mode.ECB,
    padding: CryptoJS.pad.Pkcs7,
  }).toString();
  try {
    const res = await axios.post(url, {
      username: encryptedUsername,
      password: encryptedPassword ,
    });

    console.log("set user data after login");
    setUserInfo(res.data.token,res.data.userInfo);
    console.log("user info ");
    console.log(res.data.userInfo);


   router.push("/");
  } catch (err) {
    console.error("error", err);
    toaster.error(err.response.data + "!");
  }
};

export let AddUser = async (username, email, password) => {
  const url = userApi;

  try {
    const res = await axios.post(url, {
      username: username,
      email: email,
      passwordHash: password,
    });
    debugger;
    console.log(res.data);
    toaster.info(
      "Congratulations! 🎉 You've successfully signed up for ShopNest."
    );

    router.push("/login");
  } catch (err) {
    console.error("error", err);

    toaster.error(err.response.data + "!");
  }
};

export let forgotPassword = async (username) => {
  const url = loginApi+`/forgot-password?userName=${username}`; 

  try {
    const res = await axios.post(url);
    debugger;
    console.log(res.data);
    // toaster.success("Password reset instructions sent to your email.");
  } catch (err) {
    // console.error("error", err);
    
  }
};
