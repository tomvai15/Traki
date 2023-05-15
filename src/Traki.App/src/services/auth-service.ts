import axiosApiInstance from './axios-instance';
import { LoginRequest } from '../contracts/auth/LoginRequest';
import { GetUserResponse } from '../contracts/auth/GetUserResponse';
import { LoginResponse } from '../contracts/auth/LoginResponse';
import { RegisterDeviceRequest } from '../contracts/auth/RegisterDeviceRequest';
import { GetUserInfoResponse } from '../contracts/auth/GetUserInfoResponse';
import { AxiosResponse } from 'axios';

const route = 'auth';

class AuthService {
  /* eslint-disable */
  async login(loginRequest: LoginRequest): Promise<AxiosResponse<LoginResponse, any>> {
    const fullRoute = route + '/jwt-login';
    const response = await axiosApiInstance.post<LoginResponse>(fullRoute, loginRequest, { headers: {} ,  validateStatus: (status) => status < 500 });
    return response;
  }
   /* eslint-disable */

  async logout(): Promise<void> {
    const fullRoute = route + '/jwt-logout';
    await axiosApiInstance.post(fullRoute, { headers: {} });
  }

  async registerDevice(request: RegisterDeviceRequest): Promise<void> {
    const fullRoute = route + '/registerdevice';
    await axiosApiInstance.post(fullRoute, request, { headers: {} });
  }

  async getUserInfo(): Promise<GetUserResponse> {
    const fullRoute = route + '/userstate';
    const response = await axiosApiInstance.get<GetUserResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async getUserFullInfo(): Promise<GetUserInfoResponse> {
    const fullRoute = route + '/userinfo';
    const response = await axiosApiInstance.get<GetUserInfoResponse>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new AuthService ();