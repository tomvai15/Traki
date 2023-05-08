import  React, { useState } from 'react';
import { View, Image, TouchableHighlight, KeyboardAvoidingView, Platform} from 'react-native';
import { Card, Text, Title, TextInput, Divider, IconButton, DataTable, Button } from 'react-native-paper';
import { Item } from '../../../contracts/protocol/items/Item';
import * as ImagePicker from 'expo-image-picker';
import { Table } from '../../../contracts/protocol/section/Table';
import { ScrollView } from 'react-native-gesture-handler';
import { TableRow } from '../../../contracts/protocol/section/TableRow';
import { RowColumn } from '../../../contracts/protocol/section/RowColumn';

type Props = {
  table: Table,
  updateTable: (table: Table) => void,
  buttonVisible: boolean
}

const optionsPerPage = [2, 3, 4];
const cellsPerPage = 3;

export function ProtocolTable({ table, updateTable, buttonVisible }: Props) {
  
  const [page, setPage] = React.useState<number>(0);
  const [itemsPerPage, setItemsPerPage] = React.useState(optionsPerPage[0]);

  const [index, setIndex] = useState(0);

  function updateTableRow(tableRow: TableRow) {
    const updatedRows = table.tableRows.map( row => row.id == tableRow.id ? tableRow : row);
    updateTable({...table, tableRows: updatedRows});
  }

  function updateColumn(tableRow: TableRow, rowColumn: RowColumn, value: string) {
    const updatedColumn = {...rowColumn, value: value};
    const updatedColumns = tableRow.rowColumns.map( column => column.id == rowColumn.id ? updatedColumn : column);
    updateTableRow({...tableRow, rowColumns: updatedColumns});
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
    <Card
      mode='outlined'
      style={{ borderWidth:0, marginHorizontal:5, marginVertical:10, marginBottom: 50 }}>
      <View style={{display: 'flex', padding: 10, justifyContent: 'space-around', flexDirection: 'row'}}>
        <ScrollView horizontal={true}>
          <DataTable style={{width: table.tableRows[0].rowColumns.length*170}}>
            <DataTable.Header>
              {table.tableRows[0].rowColumns.map((item, index) => 
                <DataTable.Title textStyle={{fontWeight: '900', color: 'black', fontSize: 12}} key={index}>{item.value}</DataTable.Title>
              )}
            </DataTable.Header>
            <Divider style={{ height: 2 }} />

            { table.tableRows.slice(1).map((row, index) =>
              <DataTable.Row key={index}>
                {row.rowColumns.map((collumn, index) => 
                  <DataTable.Cell key={index}>
                    <KeyboardAvoidingView 
                      keyboardVerticalOffset={200}
                      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
                      style={{ flex: 1 }}
                      enabled>
                      <TextInput 
                        value={collumn.value} 
                        onChangeText={(value) => updateColumn(row, collumn, value)}
                        style={{borderBottomColor: 'black', borderBottomWidth: 0, backgroundColor: 'white', width: 150}}/>
                    </KeyboardAvoidingView>
                  </DataTable.Cell>
                )}
              </DataTable.Row>)}
          </DataTable>
        </ScrollView>
      </View>
      { buttonVisible == true && <Card.Content>
        <Button mode='contained' onPress={addTableRow}>Add row</Button>
      </Card.Content>}
    </Card>
  );
}