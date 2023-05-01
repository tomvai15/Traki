import React, { useEffect, useState } from 'react';
import { Button, Card, CardActions, CardContent, CardHeader, Divider, Typography } from '@mui/material';
import Box from '@mui/material/Box';
import { Checklist } from '../../../contracts/protocol/Checklist';
import { Section } from '../../../contracts/protocol/Section';
import { Item } from '../../../contracts/protocol/items/Item';
import { UpdateSectionAnswersRequest } from '../../../contracts/protocol/section/UpdateSectionAnswersRequest';
import { sectionService } from 'services';
import { initialSection } from '../data';
import { FillItem } from '.';
import { Table } from 'contracts/protocol/section/Table';
import { RowColumn } from 'contracts/protocol/section/RowColumn';
import { TableRow } from 'contracts/protocol/section/TableRow';
import { FillTable } from './FillTable';
import { validate, validationRules } from 'utils/textValidation';

type Props = {
  protocolId: number,
  sectionId: number
}

export function FillSection({protocolId, sectionId}: Props) {

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
    return  (initialSectionJson != JSON.stringify(section) ||
            initialTableJson != JSON.stringify(table)) &&
            isValid() ;
  }


  function isValid(): boolean {
    return !section.checklist?.items.map(x => { 
      return ( 
        (x.textInput == undefined ? true : !validate(x.textInput.value, [validationRules.noSpecialSymbols]).invalid) &&
        (x.question == undefined ? true : !validate(x.question.comment, [validationRules.noSpecialSymbols]).invalid)
      );
    }).some((value) => value == false);
  }

  async function updateSection() {
    const request: UpdateSectionAnswersRequest = {
      section: {...section, table: table}
    };
    await sectionService.updateSectionAnswers(protocolId, sectionId, request);
    setInitialTableJson(JSON.stringify(table));
    setInitialSectionJson(JSON.stringify(section));
  }

  return (
    <Box>
      <Card>
        <CardHeader title={section.name}
          action={<Button disabled={!canUpdate()} onClick={updateSection} variant='contained'>Save Answers</Button>}>
        </CardHeader>
        <Divider/>
        <CardContent>
          {sectionType == 'checklist' &&
          <Box>
            {section.checklist?.items.map((item, index) => 
              <FillItem key={index} item={item} updateItem={updateItem}></FillItem>
            )}
          </Box>}
        </CardContent>
      </Card>
      {sectionType == 'table' && table &&
        <Box sx={{marginLeft:3}}>
          <FillTable table={table} updateTable={setTable}/>
        </Box>}
    </Box>
  );
}