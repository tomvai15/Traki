import React from 'react';
import Box from '@mui/material/Box';
import { RowColumn } from 'contracts/protocol/section/RowColumn';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { Button, Card, Divider, IconButton, Stack, Table, TableBody, TableCell, TableHead, TextField, Typography, useTheme } from '@mui/material';
import ClearIcon from '@mui/icons-material/Clear';
import { defaultColumn } from '../data';
import { validate, validationRules } from 'utils/textValidation';
import { ProtectedComponent } from 'components/ProtectedComponent';

type Props = {
  row: TableRow
  setRow: (tableRow: TableRow) => void,
}

export function CreateTableCard ({row, setRow}:Props) {
  const theme = useTheme();
  
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
      <Box sx={{padding: 1}}>
        <Card sx={{backgroundColor: theme.palette.grey[100]}}>
          <Stack direction={'column'} spacing={1} alignItems={'flex-start'}>
            <Typography variant='caption' sx={{margin: '5px'}}>
              Table headers
            </Typography>
            <Stack direction='row'>
              {row.rowColumns.map((column, index) => 
                <Stack key={index} direction='row' alignItems='center'>
                  <Divider orientation="vertical" flexItem />
                  <TextField 
                    id={'table-header-'+index}
                    inputProps={{ maxLength: 100 }}
                    error={validate(column.value, [validationRules.noSpecialSymbols]).invalid}
                    helperText={validate(column.value, [validationRules.noSpecialSymbols]).message}
                    sx={{marginLeft: '10px'}}
                    size='small'
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
          </Stack>
        </Card>
        <ProtectedComponent role={'ProjectManager'}>
          <Button sx={{marginTop: '10px'}} variant='contained' onClick={addColumn}>Add column</Button>
        </ProtectedComponent>
      </Box>
    </Box>


  
  );
}