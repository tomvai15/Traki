import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Grid, Card, CardContent, Box, TextField, Button, } from '@mui/material';
import { DragDropContext, Draggable, DropResult, Droppable } from 'react-beautiful-dnd';
import { Checklist, Section, UpdateSectionRequest } from 'contracts/protocol';
import { Question, Item } from 'contracts/protocol/items';
import { sectionService } from 'services';
import { v4 as uuid } from 'uuid';
import { TemplateItem } from 'features/protocols/components/TemplateItem';

const question: Question = {
  id: uuid(), 
  comment: '',
  answer: undefined
};

const defaultItem: Item ={
  id: uuid(), 
  name: 'Item Name', 
  priority: 1, 
  question: question, 
  multipleChoice: undefined, 
  textInput: undefined
};

const items: Item[] = [];

const checklist: Checklist = {
  id: 0,
  items: items
};

const initialChecklist: Checklist = {
  id: 0,
  items: []
};

const initialSection: Section = {
  id: 0,
  name: 'Section Name',
  priority: 1,
  checklist: checklist,
  table: undefined,
  protocolId: 0
};

export function CreateSectionPage() {

  const navigate = useNavigate();
  const { protocolId, sectionId } = useParams();
  const [section, setSection] = useState<Section>(initialSection);

  const [canCreate, setCanCreate] = useState<boolean>(true);

  const onDragEnd = (result: DropResult) => {
    if (!result.destination) return;
    const { source, destination } = result;

    if (!section.checklist) return;
  
    const copiedItems = [...section.checklist.items];
    const [removed] = copiedItems.splice(source.index, 1);
    copiedItems.splice(destination.index, 0, removed);

    copiedItems.forEach((element, index) => {
      copiedItems[index] = {...element, priority: index+1};
    });

    const newChecklist = {...section.checklist, items: copiedItems};
    setSection({...section, checklist: newChecklist});
  };

  function addItem() {
    if (!section.checklist) {
      section.checklist = initialChecklist;
    }
    const newId = uuid();
    const newItem: Item = {...defaultItem, id: newId, priority: section.checklist.items.length + 1};
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
      copiedItems[index] = {...element, priority: index+1};
    });
    const newChecklist = {...section.checklist, items: copiedItems};
    setSection({...section, checklist: newChecklist});
  }

  async function createSection() {
    const updateSectionRequest: UpdateSectionRequest = {
      section: section
    };

    await sectionService.createSection(Number(protocolId), updateSectionRequest);
    setCanCreate(false);
  }

  function updateSectionName(newName: string) {
    setSection({...section, name: newName});
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Button onClick={() => navigate('/templates/protocols/' + protocolId)} variant='contained' >Go back</Button>
      </Grid>
      <Grid item xs={12} md={12} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Box sx={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}}>
              <TextField sx={{width: '50%'}}
                id="standard-disabled"
                label="Section Name"
                variant="standard"
                value={section?.name}
                onChange={(e) => updateSectionName(e.target.value)}
              />
              <Button disabled={!canCreate} onClick={() => createSection()} variant='contained'>Create</Button>
            </Box>
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
      </Grid>
    </Grid>
  );
}