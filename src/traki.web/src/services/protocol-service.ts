import { CreateProtocolRequest } from '../contracts/protocol/CreateProtocolRequest';
import { GetProtocolResponse } from '../contracts/protocol/GetProtocolResponse';
import { GetProtocolsResponse } from '../contracts/protocol/GetProtocolsResponse';
import { UpdateProtocolRequest } from '../contracts/protocol/UpdateProtocolRequest';
import axiosApiInstance from './axios-instance';

const route = 'protocols';

class ProtocolService {
  async getProtocol(protocolId: number): Promise<GetProtocolResponse> {
    const fullRoute = route + '/' + protocolId;
    const response = await axiosApiInstance.get<GetProtocolResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async deleteProtocol(protocolId: number): Promise<void> {
    const fullRoute = route + '/' + protocolId;
    await axiosApiInstance.delete(fullRoute, { headers: {} });
  }

  async updateProtocol(protocolId: number, updateProtocolRequest: UpdateProtocolRequest): Promise<void> {
    const fullRoute = route + '/' + protocolId;
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
}
export default new ProtocolService ();