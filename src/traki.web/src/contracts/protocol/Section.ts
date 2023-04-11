import { Checklist } from "./Checklist";
import { Table } from "./section/Table";

export type Section = {
  id: number,
  name: string,
  priority: number
  checklist: Checklist|undefined,
  table: Table|undefined,
}