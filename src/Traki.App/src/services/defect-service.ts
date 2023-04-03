import { CreateDefectCommentRequest } from '../contracts/drawing/defect/CreateDefectCommentRequest';
import { CreateDefectRequest } from '../contracts/drawing/defect/CreateDefectRequest';
import { GetDefectResponse } from '../contracts/drawing/defect/GetDefectResponse';
import axiosApiInstance from './axios-instance';

const route = 'drawings/{drawingId}/defects/{defectId}';

class DefectService {
  async createDefect(drawingId: number, createDefectRequest: CreateDefectRequest): Promise<void> {
    const fullRoute = route.format({drawingId: drawingId.toString(), defectId: ''});
    await axiosApiInstance.post(fullRoute, createDefectRequest, { headers: {} });
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