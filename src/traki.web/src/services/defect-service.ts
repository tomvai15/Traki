import { CreateDefectCommentRequest } from '../contracts/drawing/defect/CreateDefectCommentRequest';
import { CreateDefectRequest } from '../contracts/drawing/defect/CreateDefectRequest';
import { GetDefectResponse } from '../contracts/drawing/defect/GetDefectResponse';
import axiosApiInstance from './axios-instance';

const route = 'drawings/{drawingId}/defects/{defectId}';

class DefectService {
  async createDefect(drawingId: number, createDefectRequest: CreateDefectRequest): Promise<GetDefectResponse> {
    const fullRoute = route.format({drawingId: drawingId.toString(), defectId: ''});
    const response = await axiosApiInstance.post<GetDefectResponse>(fullRoute, createDefectRequest, { headers: {} });
    return response.data;
  }

  async updateDefect(drawingId: number, defectId: number, createDefectRequest: CreateDefectRequest): Promise<GetDefectResponse> {
    const fullRoute = route.format({drawingId: drawingId.toString(), defectId: defectId.toString()});
    const response = await axiosApiInstance.put<GetDefectResponse>(fullRoute, createDefectRequest, { headers: {} });
    return response.data;
  }

  async createDefectComment(drawingId: number, defectId: number, createDefectCommentRequest: CreateDefectCommentRequest): Promise<void> {
    const fullRoute = route.format({drawingId: drawingId.toString(), defectId: defectId.toString()}) + '/comments';
    await axiosApiInstance.post(fullRoute, createDefectCommentRequest, { headers: {} });
  }

  async getDefect(drawingId: number, defectId: number): Promise<GetDefectResponse> {
    const fullRoute = route.format({drawingId: drawingId.toString(), defectId: defectId.toString()});
    const response = await axiosApiInstance.get<GetDefectResponse>(fullRoute, { headers: {} });
    return response.data;
  }
}
export default new DefectService ();