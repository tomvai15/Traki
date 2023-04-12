import { RowColumn } from "./RowColumn";

export type TableRow = {
  id: number,
  rowIndex: number,
  rowColumns: RowColumn[]
}