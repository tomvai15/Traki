import axios from 'axios';
import { GetProductsResponse } from '../contracts/product/GetProductsResponse';
import { GetProductResponse } from '../contracts/product/GetProductResponse';
import { CreateProductRequest } from '../contracts/product/CreateProductRequest';
import { url } from './endpoints';

const route = url + 'products'

class ProductService {
  async getProducts(): Promise<GetProductsResponse> {
    const response = await axios.get<GetProductsResponse>(route, { headers: {} });
    return response.data;
  }

  async getProduct(id: number): Promise<GetProductResponse> {
    const response = await axios.get<GetProductResponse>(`${route}/${id}`, { headers: {} });
    return response.data;
  }

  async createProduct(createProductRequest: CreateProductRequest): Promise<boolean> {
    await axios.post(route, createProductRequest, { headers: {} });
    return true;
  }
}
export default new ProductService ();