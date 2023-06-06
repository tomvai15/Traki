import React, { useState } from 'react';
import { Card, CardContent, Checkbox, FormControlLabel, IconButton, Stack, TextField, Typography, useTheme } from '@mui/material';
import Box from '@mui/material/Box';
import { AnswerType } from '../../../contracts/protocol/items/AnswerType';
import { Item } from '../../../contracts/protocol/items/Item';
import { Question } from '../../../contracts/protocol/items/Question';
import { TextInput } from '../../../contracts/protocol/items/TextInput';
import { validate, validationRules } from 'utils/textValidation';
import { PhotoCamera } from '@mui/icons-material';
import { ItemImage } from '../types/ItemImage';
import ImageWithViewer from 'components/ImageWithViewer';

type Props = {
  item: Item,
  updateItem: (item: Item) => void
  completed: boolean,
  itemImage: ItemImage| undefined
  updateItemImage: (itemImage: ItemImage) => void,
}

export function FillItem ({item, updateItem, completed, itemImage, updateItemImage}: Props) {

  const theme = useTheme();

  const [previewImage, setPreviewImage] = useState<string>('');
  const [file, setFile] = useState<File>();

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

  const selectFile = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { files } = event.target;
    const selectedFiles = files as FileList;
    setFile(selectedFiles?.[0]);
    setPreviewImage(URL.createObjectURL(selectedFiles?.[0]));

    const itemImageName = itemImage && itemImage.imageName != '' ? itemImage.imageName : `${item.id}.jpeg`;

    const updatedItemImage: ItemImage = {
      id: item.id,
      isLocal: true,
      localImageUri: selectedFiles?.[0],
      imageName: itemImageName,
      imageBase64: itemImage ? itemImage.imageBase64 : ''
    };

    updateItemImage(updatedItemImage);
    updateItem({...item, itemImage: itemImageName});
  };

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

  function isChecked(i: Item, answer: AnswerType): boolean {
    if (!i.question) return false;
    return i.question.answer != undefined ? i.question.answer==answer : false;
  }

  function checkType() {
    if (item.question) {
      return (
        <Box sx={{display: 'flex', flexDirection:'row', alignItems: 'flex-end'}}> 
          <Box sx={{flex: 3}}>
            <FormControlLabel control={<Checkbox disabled={completed} onChange={()=>updateQuestionAnswer(AnswerType.Yes)}  checked={isChecked(item, AnswerType.Yes)}/>} label="Yes" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled={completed} onChange={()=>updateQuestionAnswer(AnswerType.No)} checked={isChecked(item, AnswerType.No)}/>} label="No" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled={completed} onChange={()=>updateQuestionAnswer(AnswerType.Other)} checked={isChecked(item, AnswerType.Other)}/>} label="Other" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled={completed} onChange={()=>updateQuestionAnswer(AnswerType.NotApplicable)} checked={isChecked(item, AnswerType.NotApplicable)}/>} label="Not applicable" labelPlacement="start"/>
          </Box>
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              multiline={true}
              inputProps={{ maxLength: 250 }}
              id="question-comment"
              error={validate(item.question.comment, [validationRules.noSpecialSymbols]).invalid}
              helperText={validate(item.question.comment, [validationRules.noSpecialSymbols]).message}
              label="Comment"
              variant="standard"
              value={item.question.comment}
              onChange={(e) => updateQuestionComment(e.target.value)}
              disabled={completed}
            />
          </Box>
        </Box>);
    } else if (item.textInput) {
      return (
        <Box sx={{display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              multiline={true}
              inputProps={{ maxLength: 250 }}
              id="textinput"
              error={validate(item.textInput.value, [validationRules.noSpecialSymbols]).invalid}
              helperText={validate(item.textInput.value, [validationRules.noSpecialSymbols]).message}
              label="Comment"
              variant="standard"
              value={item.textInput.value}
              onChange={(e) => updateTextInput(e.target.value)}
              disabled={completed}
            />
          </Box>
        </Box>);
    } else if (item.multipleChoice) {
      return (
        <Box sx={{display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            { item.multipleChoice.options.map((value, index) => 
              <FormControlLabel 
                key={index} 
                control={<Checkbox disabled={completed} checked={value.selected ? value.selected : false} onClick={() => updateMultipleChoice(value.name)}/>} 
                label={value.name} 
                labelPlacement="start"/>
            )}
          </Box>
        </Box>);
    }
    return <></>;
  }

  function displayPhoto() {
    if (itemImage !== undefined) {
      if (itemImage.isLocal && itemImage.localImageUri) {
        return (
          <Stack direction={'row'} justifyContent={'flex-end'}>
            <ImageWithViewer source={URL.createObjectURL(itemImage.localImageUri)} height={100}/>
          </Stack>
        );
      } else if (itemImage.imageBase64 != '') {
        return (
          <Stack direction={'row'} justifyContent={'flex-end'}>
            <ImageWithViewer source={itemImage.imageBase64} height={100}/>
          </Stack>
        );
      }
      return;
    }
    else {
      return;
    }
  }

  return (
    <Card sx={{backgroundColor: theme.palette.grey[100], marginBottom: '10px' }}>
      <CardContent sx={{padding: '10px'}}>
        <Stack direction={'column'}>
          <Stack direction={'row'}>
            <Box sx={{flex: 1}}>
              <Typography variant='h6'>{item.name}</Typography>
            </Box>
            <Box sx={{flex: 3}}>
              {checkType()}
            </Box>
            <IconButton color="secondary" aria-label="upload picture" component="label">
              <input hidden accept="image/*" type="file" onChange={selectFile} />
              <PhotoCamera />
            </IconButton>
          </Stack>
          {displayPhoto()}
        </Stack>
      </CardContent>
    </Card>
  );
}