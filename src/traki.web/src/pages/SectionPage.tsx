import { Grid, Card, CardContent, Typography, Box, FormControlLabel, Checkbox, TextField, Button, Menu, MenuItem, IconButton } from '@mui/material';
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


const defaultQuestion: Question = {
  id: 1, 
  comment: '',
  answer: AnswerType.No
};

const defaultTextInput: TextInput = {
  id: 1, 
  value: ''
};

const defaultMultipleChoice: MultipleChoice = {
  id: 1, 
  values: [{id: 1, name: 'Option A'}, {id: 2, name: 'Option B'}],
};

const textInput: TextInput = {
  id: 1, 
  value: '',
};

const value1: Value = {
  id: 1, 
  name: 'Option A',
};

const value2: Value = {
  id: 2, 
  name: 'Option B',
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
  deleteItem: (id: number) => void
  updateItem: (item: Item) => void
}

function TemplateItem ({item, deleteItem, updateItem}: TemplateItemProps) {
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);
  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  function changeToQuestion(item: Item) {
    item.textInput = undefined;
    item.multipleChoice = undefined;
    item.question = {...defaultQuestion};
    updateItem(item);
  }

  function changeToTextInput(item: Item) {
    item.multipleChoice = undefined;
    item.question = undefined;
    item.textInput = {...defaultTextInput};
    updateItem(item);
  }

  function changeToMultipleChoice(item: Item) {
    item.textInput = undefined;
    item.question = undefined;
    item.multipleChoice = {...defaultMultipleChoice};
    updateItem(item);
  }

  function addOption(item: Item) {
    if (!item.multipleChoice) return;

    // TODO: fix incoming bugs
    const newValue: Value = {id: item.multipleChoice.values.length, name: 'Option C'};
    item.multipleChoice.values = [...item.multipleChoice.values, newValue];
    updateItem(item);
  }

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
              inputProps={{min: 0, style: { textAlign: 'center' }}}
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
              inputProps={{min: 0, style: { textAlign: 'center' }}}
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
          <Button onClick={() => addOption(item)}>Add</Button>
        </Box>);
    }
    return <></>;
  }

  return (
    <Card sx={{display: 'flex', margin: 1, padding: 1, flexDirection:'row', justifyItems: 'space'}}>
      <Box sx={{flex: 1}}>
        <TextField sx={{width: '90%'}}
          id="standard-disabled"
          label="Question"
          variant="standard"
          defaultValue={ item.priority + ' ' + item.name}
        />
      </Box>
      {checkType()}
      <Box>
        <IconButton onClick={handleOpenUserMenu}>
          <MoreVertIcon/>
        </IconButton> 
        <Menu
          sx={{ mt: '45px' }}
          id="menu-appbar"
          anchorEl={anchorElUser}
          anchorOrigin={{
            vertical: 'top',
            horizontal: 'right',
          }}
          keepMounted
          transformOrigin={{
            vertical: 'top',
            horizontal: 'right',
          }}
          open={Boolean(anchorElUser)}
          onClose={handleCloseUserMenu}
        >
          <MenuItem onClick={() => {changeToQuestion(item); handleCloseUserMenu();}}>
            <Typography textAlign="center">Default Question</Typography>
          </MenuItem>
          <MenuItem onClick={() => {changeToMultipleChoice(item); handleCloseUserMenu();}}>
            <Typography textAlign="center">Multiple Choice</Typography>
          </MenuItem>
          <MenuItem onClick={() => {changeToTextInput(item); handleCloseUserMenu();}}>
            <Typography textAlign="center">Text Input</Typography>
          </MenuItem>
          <MenuItem onClick={() => {handleCloseUserMenu(); deleteItem(item.id);}}>
            <Typography color={'red'} textAlign="center">Delete</Typography>
          </MenuItem>
        </Menu>
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

  function updateItem(item: Item) {
    if (!section.checklist) return;

    const copiedItems = [...section.checklist.items];
    copiedItems.forEach((element, index) => {

      copiedItems[index] = element.id==item.id ? item : copiedItems[index];
    });

    const newChecklist = {...section.checklist, items: copiedItems};
    setSection({...section, checklist: newChecklist});
  }

  function deleteItem(id: number) {
    if (!section.checklist) return;

    const copiedItems = [...section.checklist.items.filter(x=> x.id != id)];

    copiedItems.forEach((element, index) => {
      copiedItems[index] = {...element, id:index, priority: (index+1).toString()};
    });
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
                              <TemplateItem item={item} deleteItem={deleteItem} updateItem={updateItem}></TemplateItem>
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