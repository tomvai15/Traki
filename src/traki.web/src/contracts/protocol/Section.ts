import { Checklist } from "./Checklist";
import { Table } from "./Table";

export type Section = {
  id: number,
  name: string,
  priority: string
  checklist: Checklist|undefined,
  table: Table|undefined,
}