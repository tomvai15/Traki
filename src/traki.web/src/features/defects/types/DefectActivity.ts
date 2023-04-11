import { StatusChange } from "contracts/drawing/defect/StatusChange";
import { CommentWithImage } from "./CommentWithImage";
import { Author } from "contracts/drawing/defect/Author";

export type DefectActivity = {
  author: Author,
  date: string,
  defectComment?: CommentWithImage,
  statusChange?: StatusChange
}