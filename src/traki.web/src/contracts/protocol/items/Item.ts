import { Question } from "./Question";
import { MultipleChoice } from "./SingleChoice";
import { TextInput } from "./TextInput";

export type Item = {
  id: string,
  name: string,
  priority: string,
  question: Question|undefined,
  multipleChoice: MultipleChoice|undefined,
  textInput: TextInput|undefined
}