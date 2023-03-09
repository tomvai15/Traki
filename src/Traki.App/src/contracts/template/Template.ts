import { Question } from "./Question"

export type Template = {
  id: number,
  name: string,
  standard: string,
  questions: Question[]
}