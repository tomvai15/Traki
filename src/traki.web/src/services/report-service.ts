import { SignDocumentRequest } from '../contracts/report/SignDocumentRequest';
import axiosApiInstance from './axios-instance';

const route = 'protocols/{protocolId}/reports';

class ReportService {
  async getReport(protocolId: number): Promise<string> {
    const fullRoute = route.format({protocolId: protocolId.toString()}); 
    const response = await axiosApiInstance.get<string>(fullRoute, { headers: {} });
    return response.data;
  }

  async validateDocumentSign(protocolId: number): Promise<string> {
    const fullRoute = route.format({protocolId: protocolId.toString()}) + '/validate';
    const response = await axiosApiInstance.get<string>(fullRoute, { headers: {} });
    return response.data;
  }

  async signReport(signDocumentRequest: SignDocumentRequest): Promise<string> {
    const fullRoute = route.format({protocolId: signDocumentRequest.protocolId.toString()}) + '/sign';
    const response = await axiosApiInstance.post<string>(fullRoute, signDocumentRequest, { headers: {} });
    return response.data;
  }
}
export default new ReportService ();