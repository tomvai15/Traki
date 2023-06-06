import { Author } from './Author';
import { DefectComment } from './DefectComment';
import { DefectStatus } from './DefectStatus';
import { StatusChange } from './StatusChange';

export type Defect = {
  id: number,
  creationDate: string,
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
  statusChanges?: StatusChange[],
  author?: Author
}