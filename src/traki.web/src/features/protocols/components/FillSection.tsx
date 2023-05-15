import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, CardHeader, Divider } from '@mui/material';
import Box from '@mui/material/Box';
import { Checklist } from '../../../contracts/protocol/Checklist';
import { Section } from '../../../contracts/protocol/Section';
import { Item } from '../../../contracts/protocol/items/Item';
import { UpdateSectionAnswersRequest } from '../../../contracts/protocol/section/UpdateSectionAnswersRequest';
import { sectionService } from 'services';
import { initialSection } from '../data';
import { FillItem } from '.';
import { Table } from 'contracts/protocol/section/Table';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { FillTable } from './FillTable';
import { validate, validationRules } from 'utils/textValidation';
import { useAlert } from 'hooks/useAlert';

type Props = {
  protocolId: number,
  sectionId: number,
  completed: boolean
}

export function FillSection({protocolId, sectionId, completed}: Props) {

  const { displaySuccess  } = useAlert();
  const [section, setSection] = useState<Section>(initialSection);
  const [table, setTable] = useState<Table>();
  const [sectionType, setSectionType] = useState<string>('checklist');

  const [initialTableJson, setInitialTableJson] = useState<string>('');
  const [initialSectionJson, setInitialSectionJson] = useState<string>('');

  useEffect(() => {
    fetchSection();
  }, []);

  async function fetchSection() {
    const getSectionResponse = await sectionService.getSection(Number(protocolId), Number(sectionId));

    orderAndSetSection(getSectionResponse.section);
    orderAndSetTable(getSectionResponse.section.table);
  }

  function orderAndSetTable(table?: Table) {
    if (!table) {
      setInitialTableJson(JSON.stringify(undefined));
      return;
    }

    console.log(table);

    setSectionType('table');
    setTable(table);
    setInitialTableJson(JSON.stringify(table));
  }

  function updateItem(updatedItem: Item) {
    if (!section.checklist) {
      return;
    }
    const updatedItems = [...section.checklist.items];

    updatedItems.forEach((item, index) => {
      updatedItems[index] = item.id == updatedItem.id ? updatedItem : item;
    });

    const updatedChecklist: Checklist = {...section.checklist, items: updatedItems};
    setSection({...section, checklist: updatedChecklist});
  }

  function orderAndSetSection(sectionToSort: Section) {
    if (!sectionToSort.checklist)
    {
      setSection(sectionToSort);
      setInitialSectionJson(JSON.stringify(sectionToSort));
      return;
    }
    setSectionType('checklist');
    const sortedItems = [...sectionToSort.checklist.items];
    sortedItems.sort((a, b) => a.priority - b.priority);

    const copiedChecklist: Checklist = {...sectionToSort.checklist, items: sortedItems};
    const copiedSection: Section = {...sectionToSort, checklist: copiedChecklist};
    setSection(copiedSection);
    setInitialSectionJson(JSON.stringify(copiedSection));
  }

  function canUpdate(): boolean {
    return  ((section.checklist == undefined ? true : isValidChecklist()) && isValidTable()) &&  
      (initialSectionJson != JSON.stringify(section) ||
      initialTableJson != JSON.stringify(table));
  }


  function isValidChecklist(): boolean {
    
    const a = !section.checklist?.items.map(x => { 
      return ( 
        (x.textInput == undefined ? true : !validate(x.textInput.value, [validationRules.noSpecialSymbols]).invalid) &&
        (x.question == undefined ? true : !validate(x.question.comment, [validationRules.noSpecialSymbols]).invalid)
      );
    }).some((value) => value == false);
    console.log(a);
    return a;
  }

  function isValidTable(): boolean {
    if (!table) {
      return true;
    }
    return !table?.tableRows.map(x => { 
      console.log(isValidTableRow(x));
      return isValidTableRow(x);
    }).some((value) => value == false);
  }


  function isValidTableRow(tableRow: TableRow): boolean {
    return !tableRow.rowColumns.map(x => { 
      return (x.value == undefined ? true : !validate(x.value, [validationRules.noSpecialSymbols]).invalid);
    }).some((value) => value == false);
  }

  async function updateSection() {
    const request: UpdateSectionAnswersRequest = {
      section: {...section, table: table}
    };
    await sectionService.updateSectionAnswers(protocolId, sectionId, request);
    setInitialTableJson(JSON.stringify(table));
    setInitialSectionJson(JSON.stringify(section));
    displaySuccess(`Section ${section.name} was updated`);
  }

  return (
    <Box>
      <Card>
        <CardHeader title={section.name}
          action={ !completed && <Button id="save-section" disabled={!canUpdate()} onClick={updateSection} variant='contained'>Save Answers</Button>}>
        </CardHeader>
        <Divider/>
        <CardContent>
          {sectionType == 'checklist' &&
          <Box>
            {section.checklist?.items.map((item, index) => 
              <FillItem key={index} item={item} completed={completed} updateItem={updateItem}></FillItem>
            )}
          </Box>}
          {sectionType == 'table' && table &&
            <Box>
              <FillTable table={table} updateTable={setTable}/>
            </Box>}
        </CardContent>
      </Card>
    </Box>
  );
}