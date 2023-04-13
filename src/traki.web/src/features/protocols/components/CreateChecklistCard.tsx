import React from 'react';
import Box from '@mui/material/Box';
import { Checklist, Section } from 'contracts/protocol';
import { DragDropContext, Droppable, Draggable, DropResult } from 'react-beautiful-dnd';
import { TemplateItem } from './TemplateItem';
import { Item, Question } from 'contracts/protocol/items';
import { Button } from '@mui/material';
import { v4 as uuid } from 'uuid';
import { defaultItem } from '../data';

type Props = {
  checklist: Checklist
  updateItems: (item: Item[]) => void,
}

export function CreateChecklistCard ({checklist, updateItems}: Props) {

  const onDragEnd = (result: DropResult) => {
    if (!result.destination) return;
    const { source, destination } = result;
  
    const copiedItems = [...checklist.items];
    const [removed] = copiedItems.splice(source.index, 1);
    copiedItems.splice(destination.index, 0, removed);

    copiedItems.forEach((element, index) => {
      copiedItems[index] = {...element, priority: index+1};
    });
    updateItems(copiedItems);
  };

  function addItem() {
    console.log('a');
    if (!checklist) {
      return;
    }
    console.log('b');
    const newId = uuid();
    const newItem: Item = {...defaultItem, id: newId, priority: checklist.items.length + 1};
    const copiedItems = [...checklist.items, newItem];
    updateItems(copiedItems);
  }

  function updateItem(item: Item) {
    if (!checklist) return;

    const copiedItems = [...checklist.items];
    copiedItems.forEach((element, index) => {
      copiedItems[index] = element.id==item.id ? item : copiedItems[index];
    });

    updateItems(copiedItems);
  }

  function deleteItem(id: string) {
    if (!checklist) return;

    const copiedItems = [...checklist.items.filter(x=> x.id != id)];

    copiedItems.forEach((element, index) => {
      copiedItems[index] = {...element, priority: index+1};
    });

    updateItems(copiedItems);
  }

  return (
    <Box>
      <DragDropContext onDragEnd={result => onDragEnd(result)}>
        <Droppable droppableId={'asdsda'} >
          {(provided, snapshot) => {
            return (
              <div
                {...provided.droppableProps}
                ref={provided.innerRef}
              >
                {checklist?.items.map((item, index) => {
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
      <Button onClick={() => addItem()} variant='contained'>Add new question</Button>
    </Box>
  );
}