import React from 'react';
import { Button, Divider, Stack, TextField } from '@mui/material';
import Box from '@mui/material/Box';
import { Table } from 'contracts/protocol/section/Table';
import { FillTableRow } from './FillTableRow';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { RowColumn } from 'contracts/protocol/section/RowColumn';

type Props = {
  table: Table,
  updateTable: (table: Table) => void
}

export function FillTable ({table, updateTable}: Props) {

  function updateTableRow(tableRow: TableRow) {
    const updatedRows = table.tableRows.map( row => row.id == tableRow.id ? tableRow : row);
    updateTable({...table, tableRows: updatedRows});
  }

  function addTableRow() {
    const newTableRowColumns = [...table.tableRows[0].rowColumns].map((item): RowColumn => {
      return {...item, value: ''};
    });

    const newTableRow: TableRow = {
      id: table.tableRows.length+10000,
      rowColumns: newTableRowColumns
    };

    updateTable({...table, tableRows: [...table.tableRows, newTableRow]});
  }

  return (
    <Box>
      <Stack direction='row'>
        {table.tableRows[0].rowColumns.map((column, index) => 
          <Stack key={index} direction='row' alignItems='center'>
            <Divider orientation="vertical" flexItem />
            <TextField 
              sx={{marginLeft: '10px'}}
              size='small'
              id="standard-disabled"
              label={null}
              variant="standard"
              value={column.value}
            />
            <Divider orientation="vertical" flexItem /> 
          </Stack>)}
      </Stack>
      {table.tableRows.slice(1).map((item, index) => <FillTableRow key={index} tableRow={item} updateTableRow={updateTableRow}/> )}
      <Button onClick={addTableRow}>Add Row</Button>
    </Box>
  );
}