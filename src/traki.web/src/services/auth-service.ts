import { AxiosResponse } from 'axios';
import { LoginRequest } from '../contracts/auth/LoginRequest';
import axiosApiInstance from './axios-instance';
import { GetUserResponse } from '../contracts/auth/GetUserResponse';
import { LoginOAuthRequest } from '../contracts/auth/LoginOAuthRequest';
import { AuthorisationCodeRequest } from '../contracts/auth/AuthorisationCodeRequest';

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

  async getAuthorisationCodeUrl(authorisationCodeRequest: AuthorisationCodeRequest): Promise<string> {
    const fullRoute = route + '/code';
    const response = await axiosApiInstance.post<string>(fullRoute, authorisationCodeRequest, { headers: {} });
    return response.data;
  }
}
export default new AuthService ();