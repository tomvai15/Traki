import { GetProductsResponse } from '../contracts/product/GetProductsResponse';
import { GetProductResponse } from '../contracts/product/GetProductResponse';
import { CreateProductRequest } from '../contracts/product/CreateProductRequest';
import axiosApiInstance from './axios-instance';
import { AddProtocolRequest } from '../contracts/product/AddProtocolRequest';
import { GetProductProtocolsResponse } from '../contracts/product/GetProductProtocolsResponse';
import { UpdateProductRequest } from 'contracts/product/UpdateProductRequest';

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

  async updateProduct(projectId: number, productId: number, request: UpdateProductRequest): Promise<void> {
    const fullRoute = route.format({projectId: projectId.toString(), productId: productId.toString()}); 
    await axiosApiInstance.put(fullRoute, request, { headers: {} });
  }

  async createProduct(projectId: number, createProductRequest: CreateProductRequest): Promise<boolean> {
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

  async getProtocols( projectId: number, productId: number): Promise<GetProductProtocolsResponse> {
    const fullRoute = `projects/${projectId}/products/${productId}/protocols`;
    const response = await axiosApiInstance.get<GetProductProtocolsResponse>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new ProductService ();