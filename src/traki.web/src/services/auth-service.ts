import { AxiosResponse } from 'axios';
import { LoginRequest } from '../contracts/auth/LoginRequest';
import axiosApiInstance from './axios-instance';
import { GetUserResponse } from '../contracts/auth/GetUserResponse';
import { LoginOAuthRequest } from '../contracts/auth/LoginOAuthRequest';
import { AuthorisationCodeRequest } from '../contracts/auth/AuthorisationCodeRequest';
import { ActivateAccountRequest } from 'contracts/auth/ActivateAccountRequest';
import { GetUserInfoResponse } from 'contracts/auth/GetUserInfoResponse';
import { UpdateUserInfoRequest } from 'contracts/auth/UpdateUserInfoRequest';

const route = 'auth';

class AuthService {

  async activate(request: ActivateAccountRequest): Promise<AxiosResponse> {
    const fullRoute = route + '/activate';
    const response = await axiosApiInstance.post(fullRoute, request, { headers: {} ,  validateStatus: (status) => status < 500 });
    return response;
  }

  async login(loginRequest: LoginRequest): Promise<AxiosResponse> {
    const fullRoute = route + '/login';
    const response = await axiosApiInstance.post(fullRoute, loginRequest, { headers: {} ,  validateStatus: (status) => status < 500 });
    return response;
  }

  async logout(): Promise<AxiosResponse> {
    const fullRoute = route + '/logout';
    const response = await axiosApiInstance.post(fullRoute,{ headers: {} });
    return response;
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

  async updateUserInfo(request: UpdateUserInfoRequest): Promise<void> {
    const fullRoute = route + '/userinfo';
    await axiosApiInstance.post(fullRoute, request, { headers: {} });
  }

  async loginDocusign(loginOAuthRequest: LoginOAuthRequest): Promise<void> {
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