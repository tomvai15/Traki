import { GetUserResponse } from 'contracts/user/GetUserResponse';
import axiosApiInstance from './axios-instance';
import { GetUsersResponse } from 'contracts/user/GetUsersResponse';
import { UpdateUserStatusRequest } from 'contracts/user/UpdateUserStatusRequest';
import { CreateUserRequest } from 'contracts/user/CreateUserRequest';

const route = 'users/{userId}';

class UserService {
  async getUsers(): Promise<GetUsersResponse> {
    const fullRoute = route.format({userId: ''}); 
    const response = await axiosApiInstance.get<GetUsersResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async getUser(userId: number): Promise<GetUserResponse> {
    const fullRoute = route.format({userId: userId.toString()}); 
    const response = await axiosApiInstance.get<GetUserResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async createUser(request: CreateUserRequest): Promise<void> {
    const fullRoute = route.format({userId: ''}); 
    await axiosApiInstance.post(fullRoute, request, { headers: {} });
  }

  async updateUserStatus(userId: number, request: UpdateUserStatusRequest): Promise<void> {
    const fullRoute = route.format({userId: userId.toString()}) + '/status'; 
    await axiosApiInstance.post(fullRoute, request, { headers: {} });
  }
}
export default new UserService ();