import { DefectComment } from "./DefectComment";
import { DefectStatus } from "./DefectStatus";

export type Defect = {
  id: number,
  drawingId: number,
  title: string
  description: string
  imageName: string,
  status: DefectStatus,
  x: number,
  y: number,
  width: number,
  height: number,
  defectComments?: DefectComment[]
}