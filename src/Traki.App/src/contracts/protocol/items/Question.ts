import { AnswerType } from './AnswerType';

export type Question = {
  id: string,
  comment: string,
  answer: AnswerType | undefined
}