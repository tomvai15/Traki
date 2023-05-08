import  React from 'react';
import { View} from 'react-native';
import { Paragraph, TextInput, SegmentedButtons, HelperText } from 'react-native-paper';
import { Item } from '../../../contracts/protocol/items/Item';
import { Question } from '../../../contracts/protocol/items/Question';
import { AnswerType } from '../../../contracts/protocol/items/AnswerType';
import { validate, validationRules } from '../../../utils/textValidation';

type Props = {
  item: Item,
  updateItem: (item: Item) => void
};

export function ProtocolSectionItemQuestion({ item, updateItem }: Props) {

  function updateQuestionComment(newValue: string) {
    if (!item.question) {
      return;
    }
    const updatedQuestion: Question = {...item.question, comment: newValue};
    const updatedItem: Item = {...item, question: updatedQuestion};
    updateItem(updatedItem);
  }

  function updateQuestionAnswer(newAnswer: AnswerType) {
    if (!item.question) {
      return;
    }
    const updatedQuestion: Question = {...item.question, answer: item.question.answer == newAnswer ? undefined : newAnswer};
    const updatedItem: Item = {...item, question: updatedQuestion};
    updateItem(updatedItem);
  }

  return (
    <View>
      <SegmentedButtons
        value={item.question?.answer == undefined ? '' : item.question?.answer.toString()}
        onValueChange={(value: string) => updateQuestionAnswer(Number(value) as AnswerType)}
        buttons={[
          { value: '0', label: 'Yes' },
          { value: '1', label: 'No'},
          { value: '2', label: 'Other' },
          { value: '3', label: 'N.A.' },
        ]}
      />
      <Paragraph>Comment</Paragraph>
      {item.question &&
        <View>
          <TextInput 
            value={item.question.comment} 
            onChangeText={(value)=>  updateQuestionComment(value)}
            multiline={true}/>
          <HelperText type="error" visible={validate(item.question.comment, [validationRules.noSpecialSymbols]).invalid}>
            {validate(item.question.comment, [validationRules.noSpecialSymbols]).message}
          </HelperText>
        </View>}
    </View>
  );
}