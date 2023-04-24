import { GenerateReportRequest } from '../contracts/report/GenerateReportRequest';
import { GetReportResponse } from '../contracts/report/GetReportResponse';
import { SignDocumentRequest } from '../contracts/report/SignDocumentRequest';
import axiosApiInstance from './axios-instance';

const route = 'protocols/{protocolId}/reports';

class ReportService {

  async getReportPdf(protocolId: number) {
    const fullRoute = route.format({protocolId: protocolId.toString()}) + "/raw";     
    return axiosApiInstance.get(fullRoute, {
      headers: {
        'Content-Type': 'multipart/form-data'
      },
      responseType: 'arraybuffer'
    });
  }

  async getReport(protocolId: number): Promise<GetReportResponse> {
    const fullRoute = route.format({protocolId: protocolId.toString()}); 
    const response = await axiosApiInstance.get<GetReportResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async generateReport(protocolId: number, generateReportRequest: GenerateReportRequest): Promise<void> {
    const fullRoute = route.format({protocolId: protocolId.toString()}); 
    await axiosApiInstance.post(fullRoute, generateReportRequest, { headers: {} });
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