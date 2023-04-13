import React from 'react';
import { Divider, Stack, TextField } from '@mui/material';
import Box from '@mui/material/Box';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { RowColumn } from 'contracts/protocol/section/RowColumn';

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
    <Stack direction='row' alignItems='center'>
      {tableRow.rowColumns.map((column, index) => 
        <Stack key={index} direction='row' alignItems='center'>
          <Divider orientation="vertical" flexItem />
          <TextField 
            sx={{marginLeft: '10px'}}
            size='small'
            id="standard-disabled"
            label={null}
            variant="standard"
            value={column.value}
            onChange={(e) => updateColumn(column, e.target.value)}
          />
          <Divider orientation="vertical" flexItem /> 
        </Stack>)}
    </Stack>
  );
}