import React, { useEffect, useState } from 'react';
import { Button, Card, CardActions, Typography } from '@mui/material';
import Box from '@mui/material/Box';
import { Checklist } from '../../../contracts/protocol/Checklist';
import { Section } from '../../../contracts/protocol/Section';
import { Item } from '../../../contracts/protocol/items/Item';
import { UpdateSectionAnswersRequest } from '../../../contracts/protocol/section/UpdateSectionAnswersRequest';
import { sectionService } from 'services';
import { initialSection } from '../data';

type Props = {
  protocolId: number,
  sectionId: number
}

export function FillSection({protocolId, sectionId}: Props) {

  const [section, setSection] = useState<Section>(initialSection);
  const [initialSectionJson, setInitialSectionJson] = useState<string>('');

  useEffect(() => {
    fetchSection();
  }, []);

  async function fetchSection() {
    const getSectionResponse = await sectionService.getSection(Number(protocolId), Number(sectionId));
    console.log(getSectionResponse);
    orderAndSetSection(getSectionResponse.section);
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
    const sortedItems = [...sectionToSort.checklist.items];
    sortedItems.sort((a, b) => a.priority - b.priority);

    const copiedChecklist: Checklist = {...sectionToSort.checklist, items: sortedItems};
    const copiedSection: Section = {...sectionToSort, checklist: copiedChecklist};
    setSection(copiedSection);
    setInitialSectionJson(JSON.stringify(copiedSection));
  }

  function canUpdate(): boolean {
    return initialSectionJson != JSON.stringify(section);
  }

  async function updateSection() {
    const request: UpdateSectionAnswersRequest = {
      section: section
    };
    await sectionService.updateSectionAnswers(protocolId, sectionId, request);
    setInitialSectionJson(JSON.stringify(section));
  }

  return (
    <Box>
      <Card sx={{margin:2}}>
        <CardActions>
          <Box sx={{display: 'flex', width: '100%', flexDirection: 'row', justifyContent: 'space-between'}}>
            <Box>
              <Typography variant='h6'>{section.name}</Typography>
            </Box>
            <Box>
              <Button disabled={!canUpdate()} onClick={updateSection} variant='contained'>Save Answers</Button>
            </Box>
          </Box>
        </CardActions>
      </Card>
      <Box sx={{marginLeft:3}}>
        {section.checklist?.items.map((item, index) => 
          <FillItem key={index} item={item} updateItem={updateItem}></FillItem>
        )}
      </Box>
    </Box>
  );
}