import { Checklist, Section } from 'contracts/protocol';
import { Question, Item } from 'contracts/protocol/items';
import { RowColumn } from 'contracts/protocol/section/RowColumn';
import { Table } from 'contracts/protocol/section/Table';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { v4 as uuid } from 'uuid';

export const question: Question = {
  id: uuid(), 
  comment: '',
  answer: undefined
};

export const defaultItem: Item ={
  id: uuid(), 
  name: 'New question', 
  priority: 1, 
  question: question, 
  multipleChoice: undefined, 
  textInput: undefined,
  itemImage: undefined
};

export const defaultColumn: RowColumn = {
  id: 0, 
  value: 'New column',
  columnIndex: 0
};

export const defaulRow: TableRow = {
  id: 0, 
  rowColumns: [defaultColumn],
  rowIndex: 0
};

export const defaulTable: Table = {
  id: 0, 
  tableRows: [defaulRow]
};

export const defaultChecklist: Checklist ={
  id: 0, 
  items: [defaultItem]
};

export const initialSection: Section = {
  id: 0,
  name: 'New section',
  priority: 1,
  checklist: undefined,
  table: undefined,
  protocolId: 0
};