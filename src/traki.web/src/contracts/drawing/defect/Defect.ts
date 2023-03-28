import { DefectStatus } from "./DefectStatus"

export type Defect = {
  id: number,
  title: string
  decription: string
  status: DefectStatus
}