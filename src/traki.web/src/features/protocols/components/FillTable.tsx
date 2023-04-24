import React from 'react';
import { Button, Divider, Stack, Table as MuiTable, TableRow as MuiTableRow, TableCell, TableHead, TextField, TableContainer, Paper, TableBody } from '@mui/material';
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
      rowColumns: newTableRowColumns,
      rowIndex: 0
    };

    const updatedRows =  [...table.tableRows, newTableRow].map((item, index) => {
      return {...item, rowIndex: index};
    });

    updateTable({...table, tableRows: updatedRows});
  }

  return (
    <Box sx={{marginRight: 2, marginLeft: 2}}>
      <TableContainer component={Paper}>
        <MuiTable aria-label="simple table">
          <TableHead>
            <MuiTableRow>
              {table.tableRows[0].rowColumns.map((column, index) => 
                <TableCell key={index}>{column.value}</TableCell>
              )}
            </MuiTableRow>
          </TableHead>
          <TableBody>
            {table.tableRows.slice(1).map((item, index)  => (
              <FillTableRow key={index} tableRow={item} updateTableRow={updateTableRow}/>
            ))}
          </TableBody>
        </MuiTable>
      </TableContainer>
      <Button onClick={addTableRow}>Add Row</Button>
    </Box>
  );
}