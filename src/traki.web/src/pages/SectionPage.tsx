import { Grid, Card, CardContent, Typography, Box, FormControlLabel, Checkbox, TextField, Button, Menu, MenuItem, IconButton } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { Section } from '../contracts/protocol/Section';
import { Checklist } from '../contracts/protocol/Checklist';
import { Item } from '../contracts/protocol/items/Item';
import { Question } from '../contracts/protocol/items/Question';
import { AnswerType } from '../contracts/protocol/items/AnswerType';
import { TextInput } from '../contracts/protocol/items/TextInput';
import { MultipleChoice } from '../contracts/protocol/items/MultipleChoice';
import { Option } from '../contracts/protocol/items/Option';
import { DragDropContext, Draggable, DropResult, Droppable } from 'react-beautiful-dnd';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import DeleteIcon from '@mui/icons-material/Delete';
import { v4 as uuid } from 'uuid';
import ClearIcon from '@mui/icons-material/Clear';
import { UpdateSectionRequest } from '../contracts/protocol/UpdateSectionRequest';
import sectionService from '../services/section-service';

// TODO: allow only specific resolution

const question: Question = {
  id: uuid(), 
  comment: '',
  answer: AnswerType.No
};


const defaultQuestion: Question = {
  id: uuid(), 
  comment: '',
  answer: AnswerType.No
};

const defaultTextInput: TextInput = {
  id: uuid(), 
  value: ''
};

const defaultMultipleChoice: MultipleChoice = {
  id: uuid(), 
  options: [{id: uuid(), name: 'Option A'}, {id: uuid(), name: 'Option B'}],
};

const textInput: TextInput = {
  id: uuid(), 
  value: '',
};

const value1: Option = {
  id: uuid(), 
  name: 'Option A',
};

const value2: Option = {
  id: uuid(), 
  name: 'Option B',
};

const defaultItem: Item ={
  id: uuid(), 
  name: 'Is this Question Item', 
  priority: '1', 
  question: question, 
  multipleChoice: undefined, 
  textInput: undefined
};

const multipleChoice: MultipleChoice = {
  id: uuid(), 
  options: [value1, value2],
};

const items: Item[] = [{
  id: uuid(), 
  name: 'Is this Question Item', 
  priority: '1', 
  question: question, 
  multipleChoice: undefined, 
  textInput: undefined
}, {
  id: uuid(), 
  name: 'Serial number:', 
  priority: '2', 
  question: undefined, 
  multipleChoice: undefined, 
  textInput: textInput
}, {
  id: uuid(), 
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
  deleteItem: (id: string) => void
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

    const letter = (item.multipleChoice.options.length + 10).toString(36).toUpperCase();

    const newValue: Option = {id: uuid(), name: 'Option ' + letter};
    item.multipleChoice.options = [...item.multipleChoice.options, newValue];
    updateItem(item);
  }

  function removeOption(item: Item, valueId: string) {
    if (!item.multipleChoice) return;

    item.multipleChoice.options = item.multipleChoice.options.filter(v => v.id!= valueId);
    updateItem(item);
  }

  function updateOption(item: Item, valueId: string, updatedName: string) {
    if (!item.multipleChoice) return;

    const copiedOptions = [...item.multipleChoice.options];
    copiedOptions.forEach((element, index) => {
      copiedOptions[index] = element.id == valueId ? {...element, name: updatedName}  : element;
    });

    item.multipleChoice.options = copiedOptions;
    updateItem(item);
  }

  function updateName(item: Item, name: string) {
    item.name = name;
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
            { item.multipleChoice.options.map((value, index) => 
              <FormControlLabel 
                key={index} 
                control={<Checkbox disabled />} 
                label={
                  <>
                    <IconButton onClick={() => removeOption(item, value.id)}>
                      <ClearIcon color={'error'}/>
                    </IconButton> 
                    <TextField
                      size='small'
                      id="standard-disabled"
                      label={null}
                      variant="standard"
                      value={value.name}
                      onChange={(e) => updateOption(item, value.id, e.target.value)}
                    />
                  </>
                } 
                labelPlacement="start"/>
            )}
          </Box>
          <Button onClick={() => addOption(item)}>Add</Button>
        </Box>);
    }
    return <></>;
  }

  return (
    <Box sx={{padding: 1}}>
      <Card sx={{display: 'flex', padding: 1, flexDirection:'row', justifyItems: 'space'}}>
        <Box sx={{flex: 1}}>
          <TextField sx={{width: '90%'}}
            id="standard-disabled"
            label="Question"
            variant="standard"
            value={item.name}
            onChange={(e) => updateName(item, e.target.value)}
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
    </Box>
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
    const newId = uuid();
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

  function deleteItem(id: string) {
    if (!section.checklist) return;

    const copiedItems = [...section.checklist.items.filter(x=> x.id != id)];

    copiedItems.forEach((element, index) => {
      copiedItems[index] = {...element, priority: (index+1).toString()};
    });
    const newChecklist = {...section.checklist, items: copiedItems};
    setSection({...section, checklist: newChecklist});
  }

  async function updateSection() {
    const updateSectionRequest: UpdateSectionRequest = {
      section: section
    };
    await sectionService.updateSection(updateSectionRequest);
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
                        key={index}
                        draggableId={item.priority.toString()}
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
        <Button  onClick={() => updateSection()} variant='contained'>Submit</Button>
      </Grid>
    </Grid>
  );
}