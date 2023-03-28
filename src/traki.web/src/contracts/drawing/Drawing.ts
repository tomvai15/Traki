import { Defect } from "./defect/Defect";

export type Drawing = {
  id: number,
  title: string,
  imageName: string
  defects: Defect[]
}