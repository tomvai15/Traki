import { GetCompanyResponse } from '../contracts/company/GetCompanyResponse';
import { UpdateCompanyRequest } from '../contracts/company/UpdateCompanyRequest';
import axiosApiInstance from './axios-instance';

const route = 'companies/1';

class CompanyService {
  async getCompany(): Promise<GetCompanyResponse> {
    const fullRoute = route;
    const response = await axiosApiInstance.get<GetCompanyResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async updateCompany(updateCompany: UpdateCompanyRequest): Promise<void> {
    const fullRoute = route;
    await axiosApiInstance.patch(fullRoute, updateCompany, { headers: {} });
  }
}
export default new CompanyService ();