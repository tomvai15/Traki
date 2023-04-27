import { DefectComment } from "../../../contracts/drawing/defect/DefectComment"

export type CommentWithImage = {
  defectComment: DefectComment,
  imageBase64: string
}