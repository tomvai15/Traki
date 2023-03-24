import { Grid, Card, CardContent, Typography, Box, FormControlLabel, Checkbox, TextField, Button } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { Section } from '../contracts/protocol/Section';
import { Checklist } from '../contracts/protocol/Checklist';
import { Item } from '../contracts/protocol/items/Item';
import { Question } from '../contracts/protocol/items/Question';
import { AnswerType } from '../contracts/protocol/items/AnswerType';
import { TextInput } from '../contracts/protocol/items/TextInput';
import { MultipleChoice } from '../contracts/protocol/items/SingleChoice';
import { Value } from '../contracts/protocol/items/Value';
import { DragDropContext, Draggable, DropResult, Droppable } from 'react-beautiful-dnd';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import DeleteIcon from '@mui/icons-material/Delete';

// TODO: allow only specific resolution

const question: Question = {
  id: 1, 
  comment: '',
  answer: AnswerType.No
};

const textInput: TextInput = {
  id: 1, 
  value: '',
};

const value1: Value = {
  id: 1, 
  name: 'Mechanic',
};

const value2: Value = {
  id: 2, 
  name: 'Electronic',
};

const defaultItem: Item ={
  id: 1, 
  name: 'Is this Question Item', 
  priority: '1', 
  question: question, 
  multipleChoice: undefined, 
  textInput: undefined
};

const multipleChoice: MultipleChoice = {
  id: 1, 
  values: [value1, value2],
};

const items: Item[] = [{
  id: 1, 
  name: 'Is this Question Item', 
  priority: '1', 
  question: question, 
  multipleChoice: undefined, 
  textInput: undefined
}, {
  id: 2, 
  name: 'Serial number:', 
  priority: '2', 
  question: undefined, 
  multipleChoice: undefined, 
  textInput: textInput
}, {
  id: 3, 
  name: 'Multiple choice question:', 
  priority: '3', 
  question: undefined, 
  multipleChoice: multipleChoice, 
  textInput: undefined
}
];

const checklist: Checklist = {
  id: 1,
  items: items
};

const initialSection: Section = {
  id: 1,
  name: 'General check',
  priority: '1',
  checklist: checklist,
  table: undefined
};


type TemplateItemProps = {
  item: Item
}


function TemplateItem ({item}: TemplateItemProps) {
  function checkType() {
    if (item.question) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            <FormControlLabel control={<Checkbox disabled />} label="Yes" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled />} label="No" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled />} label="Other" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled />} label="Not applicable" labelPlacement="start"/>
          </Box>
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              disabled
              id="standard-disabled"
              label="Comment"
              variant="standard"
            />
          </Box>
        </Box>);
    } else if (item.textInput) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              disabled
              id="standard-disabled"
              label="Comment"
              variant="standard"
            />
          </Box>
        </Box>);
    } else if (item.multipleChoice) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            { item.multipleChoice.values.map((value) =>
              <FormControlLabel key={value.id} control={<Checkbox disabled />} label={value.name} labelPlacement="start"/>
            )}
          </Box>
        </Box>);
    }
    return <></>;
  }

  return (
    <Card sx={{display: 'flex', margin: 1, padding: 1, flexDirection:'row', justifyItems: 'space'}}>
      <Box sx={{flex: 1}}>
        <TextField sx={{width: '100%'}}
          id="standard-disabled"
          label="Question"
          variant="standard"
          defaultValue={ item.priority + ' ' + item.name}
        />
      </Box>
      {checkType()}
      <Box>
        <MoreVertIcon/>
        <DeleteIcon/>
      </Box>
    </Card>
  );
}

export function SectionPage() {

  const [section, setSection] = useState<Section>(initialSection);

  const onDragEnd = (result: DropResult) => {
    if (!result.destination) return;
    const { source, destination } = result;

    if (!section.checklist) return;
  
    const copiedItems = [...section.checklist.items];
    const [removed] = copiedItems.splice(source.index, 1);
    copiedItems.splice(destination.index, 0, removed);

    copiedItems.forEach((element, index) => {
      copiedItems[index] = {...element, priority: (index+1).toString()};
    });

    const newChecklist = {...section.checklist, items: copiedItems};
    setSection({...section, checklist: newChecklist});
  };

  function addItem() {
    if (!section.checklist) return;
    const newId = section.checklist.items.length+1;
    const newItem: Item = {...defaultItem, id: newId, priority: newId.toString()};
    const copiedItems = [...section.checklist.items, newItem];
    const newChecklist = {...section.checklist, items: copiedItems};
    setSection({...section, checklist: newChecklist});
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant='h6'>{section?.name}</Typography>
          </CardContent>    
        </Card>
        <DragDropContext onDragEnd={result => onDragEnd(result)}>
          <Droppable droppableId={'asdsda'} >
            {(provided, snapshot) => {
              return (
                <div
                  {...provided.droppableProps}
                  ref={provided.innerRef}
                >
                  {section.checklist?.items.map((item, index) => {
                    return (
                      <Draggable
                        key={item.id}
                        draggableId={item.id.toString()}
                        index={index}
                      >
                        {(provided, snapshot) => {
                          return (
                            <Box
                              ref={provided.innerRef}
                              {...provided.draggableProps}
                              {...provided.dragHandleProps}
                              style={{
                                userSelect: "none",
                                ...provided.draggableProps.style
                              }}
                            >
                              <TemplateItem item={item}></TemplateItem>
                            </Box>
                          );
                        }}
                      </Draggable>
                    );
                  })}
                  {provided.placeholder}
                </div>
              );
            }}
          </Droppable>
        </DragDropContext>
        <Button  onClick={() => addItem()} variant='contained'>Add new question</Button>
      </Grid>
    </Grid>
  );
}