import { DefectStatus } from "./DefectStatus";

export type Defect = {
  id: number,
  title: string
  description: string
  status: DefectStatus,
  xPosition: number,
  yPosition: number,
  width: number,
  height: number,
}