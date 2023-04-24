import  React, { useEffect, useState } from 'react';
import { TextInput } from 'react-native-paper';
import { Item } from '../../../contracts/protocol/items/Item';
import { TextInput as ItemTextInput } from '../../../contracts/protocol/items/TextInput';

type Props = {
  item: Item,
  updateItem: (item: Item) => void
};

export function ProtocolSectionItemTextInput({ item, updateItem }: Props) {
  function updateTextInput(newValue: string) {
    if (!item.textInput) {
      return;
    }
    const updatedTextInput: ItemTextInput = {...item.textInput, value: newValue};
    const updatedItem: Item = {...item, textInput: updatedTextInput};
    updateItem(updatedItem);
  } 

  return (
    <TextInput value={item.textInput?.value} 
            onChangeText={(value) =>  updateTextInput(value)}
            multiline={true}></TextInput>
  );
}