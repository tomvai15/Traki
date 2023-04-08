import { Defect } from "../drawing/defect/Defect";

export type DefectRecomendation = {
  defect: Defect,
  productId: number,
  projectId: number
}