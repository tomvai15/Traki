import { CreateDrawingRequest } from 'contracts/drawing/CreateDrawingRequest';
import { GetDrawingsResponse } from '../contracts/drawing/GetDrawingsResponse';
import axiosApiInstance from './axios-instance';

const route = 'products/{productId}/drawings/{drawingId}';

class DrawingService {
  async getDrawings(productId: number): Promise<GetDrawingsResponse> {
    const fullRoute = route.format({productId: productId.toString(), drawingId: ''});
    const response = await axiosApiInstance.get<GetDrawingsResponse>(fullRoute, { headers: {} });
    return response.data;
  }

  async createDrawing(productId: number, request: CreateDrawingRequest): Promise<void> {
    const fullRoute = route.format({productId: productId.toString(), drawingId: ''});
    await axiosApiInstance.post(fullRoute, request, { headers: {} });
  }

  async deleteDrawing(productId: number, drawingId: number): Promise<void> {
    const fullRoute = route.format({productId: productId.toString(), drawingId: drawingId.toString()});
    await axiosApiInstance.delete(fullRoute, { headers: {} });
  }
}
export default new DrawingService ();