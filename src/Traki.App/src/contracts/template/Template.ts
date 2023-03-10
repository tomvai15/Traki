import { Question } from "../question/Question"

export type Template = {
  id: number,
  name: string,
  standard: string,
  questions: Question[]
}