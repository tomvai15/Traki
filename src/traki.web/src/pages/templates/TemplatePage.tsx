import React, { useState } from 'react';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Accordion, AccordionDetails, AccordionSummary, Button, Card, CardContent, Divider, Grid, Typography } from '@mui/material';
import Box from '@mui/material/Box';
import { DragDropContext, Draggable, DropResult, Droppable } from "react-beautiful-dnd";
import { v4 as uuid } from 'uuid';

type Question = {
  id: string,
  content: string
}

const itemsFromBackend: Question[] = [
  { id: '1', content: "First task" },
  { id: '2', content: "Second task" },
  { id: '3', content: "Third task" },
  { id: '4', content: "Fourth task" },
  { id: '5', content: "Fifth task" }
];

type Props = {
  items: Question[]
  setItems: (items: Question[]) => void
}

export function Checkpoint ({items, setItems} : Props) {

  const droppableId: string= uuid();

  const onDragEnd = (result: DropResult) => {
    if (!result.destination) return;
    const { source, destination } = result;
  
    const copiedItems = [...items];
    const [removed] = copiedItems.splice(source.index, 1);
    copiedItems.splice(destination.index, 0, removed);
    setItems(copiedItems);
  };

  return (
    <DragDropContext onDragEnd={result => onDragEnd(result)}>
      <Accordion>
        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
          <Typography variant="h5">1 Checkpoint Name</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <Droppable droppableId={droppableId} >
            {(provided) => {
              return (
                <div
                  {...provided.droppableProps}
                  ref={provided.innerRef}
                >
                  {items.map((item, index) => {
                    return (
                      <Draggable
                        key={item.id}
                        draggableId={item.id}
                        index={index}
                      >
                        {(provided, snapshot) => {
                          return (
                            <Card
                              ref={provided.innerRef}
                              {...provided.draggableProps}
                              {...provided.dragHandleProps}
                              style={{
                                userSelect: "none",
                                padding: 16,
                                margin: "0 0 8px 0",
                                minHeight: "50px",
                                backgroundColor: snapshot.isDragging
                                  ? "#263B4A"
                                  : "#456C86",
                                color: "white",
                                ...provided.draggableProps.style
                              }}
                            >
                              {index+1} {item.content}
                            </Card>
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
          <Button variant='contained'>Edit</Button>
          <Button variant='contained'>Delete</Button>
        </AccordionDetails>
      </Accordion>
    </DragDropContext>
  );
}


export function TemplatePage() {

  const [items, setItems] = useState(itemsFromBackend);

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Card title='Sample Project'>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant="h5">Template Name</Typography>
            <Typography variant="h6" fontSize={15} >Checklist</Typography>
            <Typography variant="h6" fontSize={15} >Last time modified in 2023-03-03 by Tomas Vainoris</Typography>
          </CardContent>  
          <Divider/>  
          <Box>
            <DragDropContext onDragEnd={() => {return;}}>
              <Droppable droppableId={'asdasd'} >
                {(provided, snapshot) => {
                  return (
                    <div
                      {...provided.droppableProps}
                      ref={provided.innerRef}
                      style={{
                        background: snapshot.isDraggingOver
                          ? "lightblue"
                          : "lightgrey"
                      }}
                    >
                      {items.map((item, index) => {
                        return (
                          <Draggable
                            key={item.id}
                            draggableId={item.id}
                            index={index}
                          >
                            {(provided, snapshot) => {
                              return (
                                <Card
                                  ref={provided.innerRef}
                                  {...provided.draggableProps}
                                  {...provided.dragHandleProps}
                                  style={{
                                    userSelect: "none",
                                    padding: 16,
                                    margin: "0 0 8px 0",
                                    minHeight: "50px",
                                    backgroundColor: snapshot.isDragging
                                      ? "#263B4A"
                                      : "#456C86",
                                    color: "white",
                                    ...provided.draggableProps.style
                                  }}
                                >
                                  {index+1} {item.content}
                                </Card>
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
              <Button variant='contained'>Edit</Button>
              <Button variant='contained'>Delete</Button>
            </DragDropContext>
          </Box>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant="h6" fontSize={15} >Checkpoints</Typography>
            <Checkpoint items={items} setItems={setItems}></Checkpoint>
            <Checkpoint items={items} setItems={setItems}></Checkpoint>
          </CardContent>  
        </Card>
      </Grid>
    </Grid>
  );
}

