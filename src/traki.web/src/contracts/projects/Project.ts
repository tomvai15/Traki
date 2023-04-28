import { Author } from "contracts/drawing/defect/Author";

export type Project = {
  id: number,
  name: string,
  clientName: string,
  address: string,
  imageName: string,
  author?: Author
}