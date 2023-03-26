import { GetProtocolResponse } from '../contracts/protocol/GetProtocolResponse';
import { GetProtocolsResponse } from '../contracts/protocol/GetProtocolsResponse';
import { UpdateProtocolRequest } from '../contracts/protocol/UpdateProtocolRequest';
import { UpdateSectionRequest } from '../contracts/protocol/UpdateSectionRequest';
import axiosApiInstance from './axios-instance';

const route = 'protocols';

class ProtocolService {
  async getProtocol(protocolId: number): Promise<GetProtocolResponse> {
    const fullRoute = route + '/' + protocolId;
    const response = await axiosApiInstance.get<GetProtocolResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async updateProtocol(protocolId: number, updateProtocolRequest: UpdateProtocolRequest): Promise<void> {
    const fullRoute = route + '/' + protocolId;
    console.log(updateProtocolRequest);
    await axiosApiInstance.put(fullRoute, updateProtocolRequest, { headers: {} });
  }

  async getTemplateProtocols(): Promise<GetProtocolsResponse> {
    const fullRoute = route + '/templates';
    const response = await axiosApiInstance.get<GetProtocolsResponse>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new ProtocolService ();