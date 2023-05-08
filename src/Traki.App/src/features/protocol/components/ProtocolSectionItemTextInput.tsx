import  React from 'react';
import { HelperText, TextInput } from 'react-native-paper';
import { Item } from '../../../contracts/protocol/items/Item';
import { TextInput as ItemTextInput } from '../../../contracts/protocol/items/TextInput';
import { validate, validationRules } from '../../../utils/textValidation';
import { View } from 'react-native';

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
    <View>
      {item.textInput &&
        <View>
          <TextInput 
            maxLength={250}
            error={validate(item.textInput.value, [validationRules.noSpecialSymbols]).invalid}
            value={item.textInput?.value} 
            onChangeText={(value) =>  updateTextInput(value)}
            multiline={true}/>
          <HelperText type="error" visible={validate(item.textInput.value, [validationRules.noSpecialSymbols]).invalid}>
            {validate(item.textInput.value, [validationRules.noSpecialSymbols]).message}
          </HelperText>
        </View>}
    </View>
  );
}