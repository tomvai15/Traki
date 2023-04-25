import React from 'react';
import { Card, CardActions, Checkbox, FormControlLabel, Grid, TextField, Typography } from '@mui/material';
import Box from '@mui/material/Box';
import { AnswerType } from '../../../contracts/protocol/items/AnswerType';
import { Item } from '../../../contracts/protocol/items/Item';
import { Question } from '../../../contracts/protocol/items/Question';
import { TextInput } from '../../../contracts/protocol/items/TextInput';

type Props = {
  item: Item,
  updateItem: (item: Item) => void
}

export function FillItem ({item, updateItem}: Props) {

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

  function updateTextInput(newValue: string) {
    if (!item.textInput) {
      return;
    }
    const updatedTextInput: TextInput = {...item.textInput, value: newValue};
    const updatedItem: Item = {...item, textInput: updatedTextInput};
    updateItem(updatedItem);
  } 

  function updateMultipleChoice(option: string) {
    if (!item.multipleChoice) {
      return;
    }
    console.log(option + ' ');

    const updatedOptions = [...item.multipleChoice.options];

    updatedOptions.forEach((item, index) => {
      updatedOptions[index] = item.name == option ? {...item, selected: !item.selected} : {...item, selected: false};
    });

    const multipleChoice = {...item.multipleChoice, options: updatedOptions};
    const updatedItem: Item = {...item, multipleChoice: multipleChoice};
    console.log(updatedItem);
    updateItem(updatedItem);
  }

  function isChecked(i: Item, answer: AnswerType): boolean {
    if (!i.question) return false;
    return i.question.answer != undefined ? i.question.answer==answer : false;
  }

  function checkType() {
    if (item.question) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row', alignItems: 'flex-end'}}> 
          <Box sx={{flex: 3}}>
            <FormControlLabel control={<Checkbox onChange={()=>updateQuestionAnswer(AnswerType.Yes)}  checked={isChecked(item, AnswerType.Yes)}/>} label="Yes" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox onChange={()=>updateQuestionAnswer(AnswerType.No)} checked={isChecked(item, AnswerType.No)}/>} label="No" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox onChange={()=>updateQuestionAnswer(AnswerType.Other)} checked={isChecked(item, AnswerType.Other)}/>} label="Other" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox onChange={()=>updateQuestionAnswer(AnswerType.NotApplicable)} checked={isChecked(item, AnswerType.NotApplicable)}/>} label="Not applicable" labelPlacement="start"/>
          </Box>
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              id="standard-disabled"
              label="Comment"
              variant="standard"
              value={item.question.comment}
              onChange={(e) => updateQuestionComment(e.target.value)}
            />
          </Box>
        </Box>);
    } else if (item.textInput) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              id="standard-disabled"
              label="Comment"
              variant="standard"
              value={item.textInput.value}
              onChange={(e) => updateTextInput(e.target.value)}
            />
          </Box>
        </Box>);
    } else if (item.multipleChoice) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            { item.multipleChoice.options.map((value, index) => 
              <FormControlLabel 
                key={index} 
                control={<Checkbox checked={value.selected ? value.selected : false} onClick={(e) => updateMultipleChoice(value.name)}/>} 
                label={value.name} 
                labelPlacement="start"/>
            )}
          </Box>
        </Box>);
    }
    return <></>;
  }

  return (
    <Card sx={{margin:2}}>
      <CardActions>
        <Box sx={{flex: 1}}>
          <Typography variant='h6'>{item.name}</Typography>
        </Box>
        {checkType()}
      </CardActions>
    </Card>
  );
}