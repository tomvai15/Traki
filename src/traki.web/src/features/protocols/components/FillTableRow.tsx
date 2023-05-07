import React from 'react';
import { TextField, TableRow as MuiTableRow, TableCell } from '@mui/material';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { RowColumn } from 'contracts/protocol/section/RowColumn';
import { validate, validationRules } from 'utils/textValidation';

type Props = {
  tableRow: TableRow,
  updateTableRow: (tableRow: TableRow) => void
}

export function FillTableRow ({tableRow, updateTableRow}: Props) {

  function updateColumn(rowColumn: RowColumn, value: string) {
    const updatedColumn = {...rowColumn, value: value};
    const updatedColumns = tableRow.rowColumns.map( column => column.id == rowColumn.id ? updatedColumn : column);
    updateTableRow({...tableRow, rowColumns: updatedColumns});
  }

  return (
    <MuiTableRow>
      {tableRow.rowColumns.map((column, index) => 
        <TableCell key={index}>
          <TextField 
            sx={{marginLeft: '10px'}}
            size='small'
            id="standard-disabled"
            error={validate(column.value, [validationRules.noSpecialSymbols]).invalid}
            helperText={validate(column.value, [validationRules.noSpecialSymbols]).message}
            label={null}
            variant="standard"
            value={column.value}
            onChange={(e) => updateColumn(column, e.target.value)}
          />
        </TableCell >)}
    </MuiTableRow>
  );
}