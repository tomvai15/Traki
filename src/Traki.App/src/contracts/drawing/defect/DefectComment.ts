import { Author } from "./Author";

export type DefectComment = {
  id: number,
  text: string,
  imageName: string,
  date: string
  author?: Author
}