import React from 'react';
import Box from '@mui/material/Box';
import { RowColumn } from 'contracts/protocol/section/RowColumn';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { Button, Divider, IconButton, Stack, Table, TableBody, TableCell, TableHead, TextField } from '@mui/material';
import ClearIcon from '@mui/icons-material/Clear';
import { defaultColumn } from '../data';

type Props = {
  row: TableRow
  setRow: (tableRow: TableRow) => void,
}

export function CreateTableCard ({row, setRow}:Props) {
  function removeColumn(columnId: number) {
    updateRowColumns(row.rowColumns.filter(x => x.id != columnId));
  }

  function addColumn() {
    const newColumn = {...defaultColumn, id: row.rowColumns.length};
    const numberedColumns: RowColumn[] = [...row.rowColumns, newColumn].map((item, index) => {
      return {...item, columnIndex: index};
    });
    updateRowColumns(numberedColumns);
  }

  function updateRowColumn(rowColumnId: number, value: string) {
    const updatedColumns = row.rowColumns.map(x => x.id == rowColumnId ? {...x, value: value} : x);
    updateRowColumns(updatedColumns);
  }

  function updateRowColumns(rowColumns: RowColumn[]) {
    setRow({...row, rowColumns: rowColumns});
  }

  return (
    <Box>
      <Stack direction='row'>
        {row.rowColumns.map((column, index) => 
          <Stack key={index} direction='row' alignItems='center'>
            <Divider orientation="vertical" flexItem />
            <TextField 
              sx={{marginLeft: '10px'}}
              size='small'
              id="standard-disabled"
              label={null}
              variant="standard"
              value={column.value}
              onChange={(e) => updateRowColumn(column.id, e.target.value)}
            />
            <IconButton onClick={() => removeColumn(column.id)}>
              <ClearIcon color={'error'}/>
            </IconButton>
            <Divider orientation="vertical" flexItem /> 
          </Stack>)}
      </Stack>
      <Button onClick={addColumn}>Add column</Button>
    </Box>


  
  );
}