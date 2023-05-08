import { Author } from './Author';
import { DefectStatus } from './DefectStatus';

export type StatusChange = {
  id: number,
  from: DefectStatus,
  to: DefectStatus,
  date: string,
  author?: Author
}