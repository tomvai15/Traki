import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Breadcrumbs, Button, Card, CardContent, Grid, TextField, Typography } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { DragDropContext, Draggable, DropResult, Droppable } from 'react-beautiful-dnd';
import { Protocol } from '../../contracts/protocol/Protocol';
import { Section } from '../../contracts/protocol/Section';
import { Item } from '../../contracts/protocol/items/Item';
import { v4 as uuid } from 'uuid';
import { Checklist } from '../../contracts/protocol/Checklist';
import protocolService from '../../services/protocol-service';
import sectionService from '../../services/section-service';
import { UpdateProtocolRequest } from '../../contracts/protocol/UpdateProtocolRequest';
import { Link as BreadLink } from '@mui/material';
import { ProtocolSectionCard } from 'features/protocols/components/ProtocolSectionCard';

const items: Item[] = [{
  id: uuid(), 
  name: 'New Item', 
  priority: 1, 
  question: undefined, 
  multipleChoice: undefined, 
  textInput: undefined,
  itemImage: undefined
}];

const checklist: Checklist = {
  id: 1,
  items: items
};

const initialSection: Section = {
  id: 1,
  name: 'General check',
  priority: 1,
  checklist: checklist,
  table: undefined,
  protocolId: 0
};

const initialProtocol: Protocol = {
  id: 1,
  name: 'test',
  sections: [{...initialSection, name: 'General check 1' , priority: 1},{...initialSection, name: 'General check 2', priority: 2}],
  isTemplate: true,
  modifiedDate: 'sa',
  isSigned: false,
  isCompleted: false
};

export function ProtocolPage() {
  const navigate = useNavigate();
  const { protocolId } = useParams();

  const [protocol, setProtocol] = useState<Protocol>(initialProtocol);
  const [sections, setSections] = useState<Section[]>([]);

  const [initialSectionsJson,  setInitialSectionsJson] = useState<string>('');
  const [initialProtocolJson,  setInitialProtocolJson] = useState<string>('');

  useEffect(() => {
    fetchProtocol();
  }, []);

  function canUpdate() {
    return (JSON.stringify(sections) != initialSectionsJson) || (JSON.stringify(protocol) != initialProtocolJson);
  }

  async function updateProtocolAndSection() {
    const updateProtocolRequest: UpdateProtocolRequest ={
      protocol: protocol,
      sections: sections
    };
    await protocolService.updateProtocol(Number(protocolId), updateProtocolRequest);
    setInitialProtocolJson(JSON.stringify(protocol));
    setInitialSectionsJson(JSON.stringify(sections));
  }

  async function fetchProtocol() {
    const getProtocolsResponse = await protocolService.getProtocol(Number(protocolId));
    const getSectionsResponse = await sectionService.getSections(Number(protocolId));
    setProtocol(getProtocolsResponse.protocol);
    setInitialProtocolJson(JSON.stringify(getProtocolsResponse.protocol));
    orderAndSetSections(getSectionsResponse.sections);
  }

  function orderAndSetSections(sectionsToSort: Section[]) {
    const sortedItems = [...sectionsToSort];

    sortedItems.sort((a, b) => a.priority - b.priority);
    setSections(sortedItems);
    setInitialSectionsJson(JSON.stringify(sortedItems));
  }

  const onDragEnd = (result: DropResult) => {
    if (!result.destination) return;
    const { source, destination } = result;

    const copiedSections = [...sections];
    const [removed] = copiedSections.splice(source.index, 1);
    copiedSections.splice(destination.index, 0, removed);

    copiedSections.forEach((element, index) => {
      copiedSections[index] = {...element, priority: index+1};
    });

    setSections(copiedSections);
  };

  function updateName(newName: string) {
    setProtocol({...protocol, name: newName});
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <BreadLink color="inherit" href="/templates/protocols">
            Protocol Templates
          </BreadLink>
          <Typography color="text.primary">Protocol</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={12} md={12} >
        <Button onClick={() => navigate('/templates/protocols')} variant='contained' >Go back</Button>
      </Grid>
      <Grid item xs={12} md={12} >
        <Card title='Sample Project'>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Box sx={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}}>
              <TextField sx={{width: '50%'}}
                id="standard-disabled"
                label="Protocol Name"
                variant="standard"
                value={protocol.name}
                onChange={(e) => updateName(e.target.value)}
              />
              <Button onClick={() => updateProtocolAndSection()} disabled={!canUpdate()} variant='contained' >Save</Button>
            </Box>
          </CardContent>    
        </Card>
      </Grid>
      <Grid item xs={12} md={12} >
        <Typography variant="overline" >sections</Typography>
        <DragDropContext onDragEnd={result => onDragEnd(result)}>
          <Droppable droppableId={'asdsda'} >
            {(provided) => {
              return (
                <div
                  {...provided.droppableProps}
                  ref={provided.innerRef}
                >
                  {sections.map((item, index) => {
                    return (
                      <Draggable
                        key={index}
                        draggableId={item.priority.toString()}
                        index={index}
                      >
                        {(provided) => {
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
                              <ProtocolSectionCard section={item}></ProtocolSectionCard>
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
        <Button onClick={() => navigate('sections/create')} variant='contained' >Add Section</Button>
      </Grid>
    </Grid>
  );
}