import { Button, Card, CardActions, Checkbox, FormControlLabel, Grid, TextField, Typography } from '@mui/material';
import Box from '@mui/material/Box';
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Checklist } from '../../../contracts/protocol/Checklist';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { Section } from '../../../contracts/protocol/Section';
import { AnswerType } from '../../../contracts/protocol/items/AnswerType';
import { Item } from '../../../contracts/protocol/items/Item';
import { Question } from '../../../contracts/protocol/items/Question';
import { TextInput } from '../../../contracts/protocol/items/TextInput';
import { UpdateSectionAnswersRequest } from '../../../contracts/protocol/section/UpdateSectionAnswersRequest';
import protocolService from '../../../services/protocol-service';
import sectionService from '../../../services/section-service';

const initialProtocol: Protocol = {
  id: 1,
  name: '',
  sections: [],
  isTemplate: false,
  modifiedDate: '',
  isSigned: false
};

const initialSection: Section = {
  id: 0,
  name: '',
  priority: 1,
  checklist: undefined,
  table: undefined,
  protocolId: 0
};

type FillItemProps = {
  item: Item,
  updateItem: (item: Item) => void
}

function FillItem ({item, updateItem}: FillItemProps) {

  function updateQuestionComment(newValue: string) {
    if (!item.question) {
      return;
    }
    const updatedQuestion: Question = {...item.question, comment: newValue};
    const updatedItem: Item = {...item, question: updatedQuestion};
    updateItem(updatedItem);
  }

  function updateQuestionAnswer(newAnswer: AnswerType) {
    if (!item.question) {
      return;
    }
    const updatedQuestion: Question = {...item.question, answer: item.question.answer == newAnswer ? undefined : newAnswer};
    const updatedItem: Item = {...item, question: updatedQuestion};
    updateItem(updatedItem);
  }

  function updateTextInput(newValue: string) {
    if (!item.textInput) {
      return;
    }
    const updatedTextInput: TextInput = {...item.textInput, value: newValue};
    const updatedItem: Item = {...item, textInput: updatedTextInput};
    updateItem(updatedItem);
  } 

  function updateMultipleChoice(option: string) {
    if (!item.multipleChoice) {
      return;
    }
    console.log(option + ' ');

    const updatedOptions = [...item.multipleChoice.options];

    updatedOptions.forEach((item, index) => {
      updatedOptions[index] = item.name == option ? {...item, selected: !item.selected} : {...item, selected: false};
    });

    const multipleChoice = {...item.multipleChoice, options: updatedOptions};
    const updatedItem: Item = {...item, multipleChoice: multipleChoice};
    console.log(updatedItem);
    updateItem(updatedItem);
  }

  function isChecked(i: Item, answer: AnswerType): boolean {
    if (!i.question) return false;
    return i.question.answer != undefined ? i.question.answer==answer : false;
  }

  function checkType() {
    if (item.question) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            <FormControlLabel control={<Checkbox onChange={()=>updateQuestionAnswer(AnswerType.Yes)}  checked={isChecked(item, AnswerType.Yes)}/>} label="Yes" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox onChange={()=>updateQuestionAnswer(AnswerType.No)} checked={isChecked(item, AnswerType.No)}/>} label="No" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox onChange={()=>updateQuestionAnswer(AnswerType.Other)} checked={isChecked(item, AnswerType.Other)}/>} label="Other" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox onChange={()=>updateQuestionAnswer(AnswerType.NotApplicable)} checked={isChecked(item, AnswerType.NotApplicable)}/>} label="Not applicable" labelPlacement="start"/>
          </Box>
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              id="standard-disabled"
              label="Comment"
              variant="standard"
              value={item.question.comment}
              onChange={(e) => updateQuestionComment(e.target.value)}
            />
          </Box>
        </Box>);
    } else if (item.textInput) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              id="standard-disabled"
              label="Comment"
              variant="standard"
              value={item.textInput.value}
              onChange={(e) => updateTextInput(e.target.value)}
            />
          </Box>
        </Box>);
    } else if (item.multipleChoice) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            { item.multipleChoice.options.map((value, index) => 
              <FormControlLabel 
                key={index} 
                control={<Checkbox checked={value.selected ? value.selected : false} onClick={(e) => updateMultipleChoice(value.name)}/>} 
                label={value.name} 
                labelPlacement="start"/>
            )}
          </Box>
        </Box>);
    }
    return <></>;
  }

  return (
    <Card sx={{margin:2}}>
      <CardActions>
        <Box sx={{flex: 1}}>
          <Typography variant='h6'>{item.name}</Typography>
        </Box>
        {checkType()}
      </CardActions>
    </Card>
  );
}

type FillSectionProps = {
  protocolId: number,
  sectionId: number
}

function FillSection({protocolId, sectionId}: FillSectionProps) {

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


export function FillProtocol() {
  const navigate = useNavigate();
  const { projectId, productId, protocolId } = useParams();
  const [protocol, setProtocol] = useState<Protocol>(initialProtocol);
  const [sections, setSections] = useState<Section[]>([]);

  useEffect(() => {
    fetchProtocol();
  }, []);

  async function fetchProtocol() {
    const getProtocolsResponse = await protocolService.getProtocol(Number(protocolId));
    const getSectionsResponse = await sectionService.getSections(Number(protocolId));
    setProtocol(getProtocolsResponse.protocol);
    orderAndSetSections(getSectionsResponse.sections);
  }

  function orderAndSetSections(sectionsToSort: Section[]) {
    const sortedItems = [...sectionsToSort];

    console.log(sectionsToSort);
    sortedItems.sort((a, b) => a.priority - b.priority);
    setSections(sortedItems);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Button onClick={() => navigate(`/projects/${projectId}/products/${productId}`)} variant='contained' >Go back</Button>
      </Grid>
      <Grid item xs={12} md={12} >
        <Typography variant='h5'>{protocol.name}</Typography>
      </Grid>
      {sections.map((section, index) => 
        <Grid key={index} item xs={12} md={12} >
          <FillSection protocolId={Number(protocolId)} sectionId={section.id}></FillSection>
        </Grid>)}
    </Grid>
  );
}