import { AddProtocolRequest } from '../contracts/product/AddProtocolRequest';
import { GetProductProtocolsResponse } from '../contracts/product/GetProductProtocolsResponse';
import { CreateProtocolRequest } from '../contracts/protocol/CreateProtocolRequest';
import { GetProtocolResponse } from '../contracts/protocol/GetProtocolResponse';
import { GetProtocolsResponse } from '../contracts/protocol/GetProtocolsResponse';
import { UpdateProtocolRequest } from '../contracts/protocol/UpdateProtocolRequest';
import axiosApiInstance from './axios-instance';

const route = 'protocols';

class ProtocolService {
  async getProtocol(protocolId: number): Promise<GetProtocolResponse> {
    const fullRoute = `${route}/${protocolId}`;
    const response = await axiosApiInstance.get<GetProtocolResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async updateProtocol(protocolId: number, updateProtocolRequest: UpdateProtocolRequest): Promise<void> {
    const fullRoute = `${route}/${protocolId}`;
    console.log(updateProtocolRequest);
    await axiosApiInstance.put(fullRoute, updateProtocolRequest, { headers: {} });
  }

  async createProtocol(createProtocolRequest: CreateProtocolRequest): Promise<void> {
    const fullRoute = route;
    await axiosApiInstance.post(fullRoute, createProtocolRequest, { headers: {} });
  }

  async getTemplateProtocols(): Promise<GetProtocolsResponse> {
    const fullRoute = route + '/templates';
    const response = await axiosApiInstance.get<GetProtocolsResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async addProtocol( projectId: number, productId: number, protocolId: number): Promise<void> {
    const fullRoute = `projects/${projectId}/products/${productId}/protocols`;
    const addProtocolRequest: AddProtocolRequest = {
      protocolId: protocolId
    };
    await axiosApiInstance.post(fullRoute, addProtocolRequest, { headers: {} });
  }

  async getProtocols( projectId: number, productId: number): Promise<GetProductProtocolsResponse> {
    const fullRoute = `projects/${projectId}/products/${productId}/protocols`;
    const response = await axiosApiInstance.get<GetProductProtocolsResponse>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new ProtocolService ();