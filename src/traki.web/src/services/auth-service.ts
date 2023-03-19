import { AxiosResponse } from 'axios';
import { LoginRequest } from '../contracts/auth/LoginRequest';
import axiosApiInstance from './axios-instance';

const route = 'auth';

class AuthService {
  async login(loginRequest: LoginRequest): Promise<AxiosResponse> {
    console.log(loginRequest);
    const fullRoute = route + '/login';
    const response = await axiosApiInstance.post(fullRoute, loginRequest, { headers: {} ,  validateStatus: (status) => status < 500 });
    return response;
  }

  async getUserInfo(): Promise<string> {
    const fullRoute = route + '/userinfo';
    const response = await axiosApiInstance.get<string>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new AuthService ();