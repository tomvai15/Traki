import { AxiosResponse } from 'axios';
import { LoginRequest } from '../contracts/auth/LoginRequest';
import axiosApiInstance from './axios-instance';
import { GetUserResponse } from '../contracts/auth/GetUserResponse';
import { LoginOAuthRequest } from '../contracts/auth/LoginOAuthRequest';

const route = 'auth';

class AuthService {
  async login(loginRequest: LoginRequest): Promise<AxiosResponse> {
    console.log(loginRequest);
    const fullRoute = route + '/login';
    const response = await axiosApiInstance.post(fullRoute, loginRequest, { headers: {} ,  validateStatus: (status) => status < 500 });
    return response;
  }

  async getUserInfo(): Promise<GetUserResponse> {
    const fullRoute = route + '/userinfo';
    const response = await axiosApiInstance.get<GetUserResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async loginDocusign(loginOAuthRequest: LoginOAuthRequest): Promise<void> {
    console.log(loginOAuthRequest);
    const fullRoute = route + '/docusign';
    await axiosApiInstance.post(fullRoute, loginOAuthRequest, { headers: {} });
  }
}
export default new AuthService ();