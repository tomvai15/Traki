import { Section } from './Section';

export type Protocol = {
  id: number,
  name: string,
  isTemplate: boolean,
  sections: Section[],
  modifiedDate: string
}