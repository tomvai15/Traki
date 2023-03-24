import { AnswerType } from "./AnswerType";

export type Question = {
  id: number,
  comment: string,
  answer: AnswerType
}