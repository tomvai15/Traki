import React from 'react';
import Box from '@mui/material/Box';
import { Typography } from '@mui/material';
import { Section } from 'contracts/protocol';
import { DropResult, DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import { ProtocolSectionCard } from './ProtocolSectionCard';

type Props = {
  sections: Section[]
  setSections: (sections: Section[]) => void
}

export function ProtocolSections({sections, setSections}: Props) {

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

  return (
    <Box>
      <Typography variant="overline" >sections</Typography>
      <Box sx={{marginLeft: '-5px'}}>
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
      </Box>
    </Box>
  );
}