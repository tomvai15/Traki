import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Accordion, AccordionDetails, AccordionSummary, Button, Card, CardContent, Divider, Grid, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { DragDropContext, Draggable, DropResult, Droppable } from 'react-beautiful-dnd';
import { Protocol } from '../../contracts/protocol/Protocol';
import { Section } from '../../contracts/protocol/Section';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Item } from '../../contracts/protocol/items/Item';
import { v4 as uuid } from 'uuid';
import { Checklist } from '../../contracts/protocol/Checklist';
// TODO: allow only specific resolution

const items: Item[] = [{
  id: uuid(), 
  name: 'Is this Question Item', 
  priority: '1', 
  question: undefined, 
  multipleChoice: undefined, 
  textInput: undefined
}, {
  id: uuid(), 
  name: 'Serial number:', 
  priority: '2', 
  question: undefined, 
  multipleChoice: undefined, 
  textInput: undefined
}, {
  id: uuid(), 
  name: 'Multiple choice question:', 
  priority: '3', 
  question: undefined, 
  multipleChoice: undefined, 
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

const initialProtocol: Protocol = {
  id: 1,
  name: 'test',
  sections: [{...initialSection, name: 'General check 1' , priority: '1'},{...initialSection, name: 'General check 2', priority: '2'}]
};

type SectionItemProps = {
  section: Section
}

function SectionItem ({section}: SectionItemProps) {

  const navigate = useNavigate();
  return (
    <Box sx={{padding: 1}}>
      <Accordion>
        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
          <Typography>{section.name}</Typography>
        </AccordionSummary>
        <AccordionDetails>
          {section.checklist?.items.map((item, index) => 
            <Box key={index}>
              <Typography>
                {item.name}
              </Typography>
              <Divider></Divider>
            </Box>)}
          <Button onClick={() => navigate('/report')} variant='contained'>Edit</Button>
          <Button color='error' variant='contained'>Delete</Button>
        </AccordionDetails>
      </Accordion>
    </Box>
  );
}


export function ProtocolTemplates() {

  const navigate = useNavigate();

  const [protocol, setProtocol] = useState<Protocol>(initialProtocol);

  useEffect(() => {
    fetchCompany();
  }, []);

  function fetchCompany() {
    console.log('asd');
  }

  const onDragEnd = (result: DropResult) => {
    if (!result.destination) return;
    const { source, destination } = result;

    if (!protocol.sections) return;
  
    const copiedSections = [...protocol.sections];
    const [removed] = copiedSections.splice(source.index, 1);
    copiedSections.splice(destination.index, 0, removed);

    copiedSections.forEach((element, index) => {
      copiedSections[index] = {...element, priority: (index+1).toString()};
    });

    const newProtocol: Protocol = {...protocol, sections: copiedSections};
    setProtocol(newProtocol);
  };

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Card title='Sample Project' onClick={() => navigate('/templates/1')}>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant="h5">Template Name</Typography>
            <Typography variant="h6" fontSize={15} >Checklist</Typography>
          </CardContent>    
        </Card>
      </Grid>
      <Grid item xs={12} md={12} >
        <DragDropContext onDragEnd={result => onDragEnd(result)}>
          <Droppable droppableId={'asdsda'} >
            {(provided, snapshot) => {
              return (
                <div
                  {...provided.droppableProps}
                  ref={provided.innerRef}
                >
                  {protocol?.sections?.map((item, index) => {
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
                              <SectionItem section={item}></SectionItem>
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
      </Grid>
      <Grid item xs={12} md={12} >
        <Button variant='contained' >Update</Button>
      </Grid>
    </Grid>
  );
}