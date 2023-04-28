import { Author } from "contracts/drawing/defect/Author";

export type Product = {
  id: number,
  name: string,
  status: string,
  projectId: number,
  author?: Author
}