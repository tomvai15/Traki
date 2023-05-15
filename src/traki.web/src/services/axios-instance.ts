import { url } from './endpoints';
import axios from 'axios';

const axiosApiInstance = axios.create({
  baseURL: url,
  withCredentials: true
});

axiosApiInstance.interceptors.request.use(function (config) { 
  return config;
}, function (error) {
  return Promise.reject(error);
});

axiosApiInstance.interceptors.response.use((res) => {  
  return res; 
}, function (error) {
  return Promise.reject(error);
});

export default axiosApiInstance;