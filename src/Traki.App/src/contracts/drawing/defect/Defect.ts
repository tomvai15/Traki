import { DefectComment } from "./DefectComment";
import { DefectStatus } from "./DefectStatus";

export type Defect = {
  id: number,
  title: string
  description: string
  status: DefectStatus,
  x: number,
  y: number,
  width: number,
  height: number,
  drawingId: number,
  defectComments?: DefectComment[]
}