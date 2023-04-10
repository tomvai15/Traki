import { GetProductsResponse } from '../contracts/product/GetProductsResponse';
import { GetProductResponse } from '../contracts/product/GetProductResponse';
import { CreateProductRequest } from '../contracts/product/CreateProductRequest';
import axiosApiInstance from './axios-instance';

const route = 'projects/{projectId}/products/{productId}';

class ProductService {
  async getProducts(): Promise<GetProductsResponse> {
    const projectId = 1;
    const fullRoute = route.format({ projectId: projectId.toString(), productId: ''}); 
    const response = await axiosApiInstance.get<GetProductsResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async getProduct(id: number): Promise<GetProductResponse> {
    const projectId = 1;
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
}
export default new ProductService ();