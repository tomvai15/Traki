import { Protocol } from "./Protocol";
import { Section } from "./Section";

export type UpdateProtocolRequest = {
  protocol: Protocol
  sections: Section[]
}