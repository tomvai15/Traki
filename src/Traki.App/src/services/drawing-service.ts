import { GetDrawingsResponse } from '../contracts/drawing/GetDrawingsResponse';
import axiosApiInstance from './axios-instance';

const route = 'products/{productId}/drawings/{drawingId}';

class DrawingService {
  async getDrawings(productId: number): Promise<GetDrawingsResponse> {
    const fullRoute = route.format({productId: productId.toString(), drawingId: ''});
    const response = await axiosApiInstance.get<GetDrawingsResponse>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new DrawingService ();