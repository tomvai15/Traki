import { Author } from "contracts/drawing/defect/Author";
import { Section } from "./Section";

export type Protocol = {
  id: number,
  name: string,
  isSigned: boolean,
  isTemplate: boolean,
  sections: Section[],
  modifiedDate: string,
  signer?: Author
}