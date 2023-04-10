import { Defect } from "../../contracts/drawing/defect/Defect";

export type DefectWithImage = {
  defect: Defect,
  imageBase64: string
}