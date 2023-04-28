import { useRecoilState, useRecoilValue } from 'recoil';
import { userState } from '../state/user-state';
import { url } from './endpoints';
import axios, { AxiosError } from 'axios';
import { getRecoil, setRecoil } from 'recoil-nexus'
import authService from './auth-service';
import { RefreshTokenRequest } from '../contracts/auth/RefreshTokenRequest';
import { LoginResponse } from '../contracts/auth/LoginResponse';

const axiosApiInstance = axios.create({
  baseURL: url
});

axiosApiInstance.interceptors.request.use(function (config) {
  const userInfo = getRecoil(userState);
  console.log('[\u001B[37mMaking \u001b[1;36m'+ config.method?.toUpperCase() +'\u001B[37m request \u001b[1;36m' + config.baseURL +'/'+ config.url + '\u001B[37m]'); 
  config.headers.Authorization = `Bearer ${userInfo.token}`;
  return config;
}, function (error) {
  return Promise.reject(error);
});

axiosApiInstance.interceptors.response.use((res) => { 
  console.log('[\u001b[1;32m' + res.request.responseURL + ' \u001b[1;36mresponded ' + res.request.status + '\u001B[37m]'); 
  return res; 
}, async function (error) {
  const originalRequest = error.config;
  if (axios.isAxiosError(error))  {
    if (error.response?.status == 401) {
      const userInfo = getRecoil(userState);
      const request: RefreshTokenRequest = {
        token: userInfo.token ?? '',
        refreshToken: userInfo.refreshToken ?? ''
      }
      const token = await refreshToken(request);
      
      if (!token) {
        return Promise.reject(error);
      }
      
      axios.defaults.headers.Authorization = `Bearer ${token}`;
      return axiosApiInstance(originalRequest);
    }
  } 
  return Promise.reject(error);
});

async function refreshToken(request: RefreshTokenRequest): Promise<string> {
  const fullRoute = 'auth/refresh-token';
  const axiosInstance = axios.create({
    baseURL: url
  });

  const response = await axiosInstance.post(fullRoute, request, { headers: {}, validateStatus: (status) => status < 500  });
  if (response.status >= 300) {
    setRecoil(userState, { id: 1, token: '', loggedInDocuSign: false });
    return '';
  } else {
    setRecoil(userState, (x=> { return {...x, token: response.data.token, refreshToken: response.data.refreshToken} }));
  }
  return response.data.token;
}

export default axiosApiInstance;