import  React from 'react';
import { View } from 'react-native';
import { Checkbox, Text } from 'react-native-paper';
import { Item } from '../../../contracts/protocol/items/Item';

type Props = {
  item: Item,
  updateItem: (item: Item) => void
};

export function ProtocolSectionItemMultipleChoice({ item, updateItem }: Props) {

  function updateMultipleChoice(option: string) {
    if (!item.multipleChoice) {
      return;
    }
    const updatedOptions = [...item.multipleChoice.options];
    updatedOptions.forEach((item, index) => {
      updatedOptions[index] = item.name == option ? {...item, selected: !item.selected} : {...item, selected: false};
    });

    const multipleChoice = {...item.multipleChoice, options: updatedOptions};
    const updatedItem: Item = {...item, multipleChoice: multipleChoice};
    updateItem(updatedItem);
  }

  return (
    <View style={{display: 'flex', flexDirection: 'row', justifyContent: 'space-evenly'}}>
      {item.multipleChoice?.options.map((item, index) => 
        <View key={index} >
          <Text>{item.name}</Text>
          <Checkbox 
            status={item.selected ? 'checked' : 'unchecked'} 
            onPress={() => updateMultipleChoice(item.name)}/>
        </View>
      )}
    </View>
  );
}