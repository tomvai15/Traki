import { GetProductsResponse } from '../contracts/product/GetProductsResponse';
import { GetProductResponse } from '../contracts/product/GetProductResponse';
import { CreateProductRequest } from '../contracts/product/CreateProductRequest';
import axiosApiInstance from './axios-instance';
import { AddProtocolRequest } from '../contracts/product/AddProtocolRequest';

const route = 'projects/{projectId}/products/{productId}';

class ProductService {
  async getProducts(projectId: number): Promise<GetProductsResponse> {
    const fullRoute = route.format({ projectId: projectId.toString(), productId: ''}); 
    const response = await axiosApiInstance.get<GetProductsResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async getProduct(projectId: number, id: number): Promise<GetProductResponse> {
    const fullRoute = route.format({projectId: projectId.toString(), productId: id.toString()}); 
    const response = await axiosApiInstance.get<GetProductResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async createProduct(createProductRequest: CreateProductRequest): Promise<boolean> {
    const projectId = 1;
    const fullRoute = route.format({ projectId: projectId.toString(), productId: ''}); 
    await axiosApiInstance.post(fullRoute, createProductRequest, { headers: {} });
    return true;
  }

  async addProtocol( projectId: number, productId: number, protocolId: number): Promise<void> {
    const fullRoute = `projects/${projectId}/products/${productId}/protocols`;
    const addProtocolRequest: AddProtocolRequest = {
      protocolId: protocolId
    };
    await axiosApiInstance.post(fullRoute, addProtocolRequest, { headers: {} });
  }
}
export default new ProductService ();