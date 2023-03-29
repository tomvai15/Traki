import { url } from './endpoints';
import axios from 'axios';

const axiosApiInstance = axios.create({
  baseURL: url
});

axiosApiInstance.interceptors.request.use(function (config) {
  console.log('[\u001B[37mMaking \u001b[1;36m'+ config.method?.toUpperCase() +'\u001B[37m request \u001b[1;36m' + config.baseURL +'/'+ config.url + '\u001B[37m]'); 
  return config;
}, function (error) {
  return Promise.reject(error);
});

axiosApiInstance.interceptors.response.use((res) => { 
  console.log('[\u001b[1;32m' + res.request.responseURL + ' \u001b[1;36mresponded ' + res.request.status + '\u001B[37m]'); 
  return res; 
}, function (error) {
  return Promise.reject(error);
});

export default axiosApiInstance;