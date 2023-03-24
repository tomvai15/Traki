import { Grid, Card, CardContent, Typography, Box, FormControlLabel, Checkbox, TextField } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { Section } from '../contracts/protocol/Section';
import { Checklist } from '../contracts/protocol/Checklist';
import { Item } from '../contracts/protocol/items/Item';
import { Question } from '../contracts/protocol/items/Question';
import { AnswerType } from '../contracts/protocol/items/AnswerType';
import { TextInput } from '../contracts/protocol/items/TextInput';
import { MultipleChoice } from '../contracts/protocol/items/SingleChoice';
import { Value } from '../contracts/protocol/items/Value';
// TODO: allow only specific resolution

const section: Section = {
  id: 1,
  name: 'General check',
  priority: '1',
  checklist: undefined,
  table: undefined
};

const checklist: Checklist = {
  id: 1,
  items: []
};

const question: Question = {
  id: 1, 
  comment: '',
  answer: AnswerType.No
};

const textInput: TextInput = {
  id: 1, 
  value: '',
};

const value1: Value = {
  id: 1, 
  name: 'Mechanic',
};

const value2: Value = {
  id: 1, 
  name: 'Electronic',
};


const multipleChoice: MultipleChoice = {
  id: 1, 
  values: [value1, value2],
};

const items: Item[] = [{
  id: 1, 
  name: 'Is this Question Item', 
  priority: '1', 
  question: question, 
  multipleChoice: undefined, 
  textInput: undefined
}, {
  id: 2, 
  name: 'Serial number:', 
  priority: '1', 
  question: undefined, 
  multipleChoice: undefined, 
  textInput: textInput
}, {
  id: 3, 
  name: 'Multiple choice question:', 
  priority: '1', 
  question: undefined, 
  multipleChoice: multipleChoice, 
  textInput: undefined
}
];



type TemplateItemProps = {
  item: Item
}


function TemplateItem ({item}: TemplateItemProps) {

  function checkType() {
    if (item.question) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            <FormControlLabel control={<Checkbox disabled />} label="Yes" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled />} label="No" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled />} label="Other" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled />} label="Not applicable" labelPlacement="start"/>
          </Box>
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              disabled
              id="standard-disabled"
              label="Comment"
              variant="standard"
            />
          </Box>
        </Box>);
    } else if (item.textInput) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              disabled
              id="standard-disabled"
              label="Comment"
              variant="standard"
            />
          </Box>
        </Box>);
    } else if (item.multipleChoice) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            { item.multipleChoice.values.map((value) =>
              <FormControlLabel key={value.id} control={<Checkbox disabled />} label={value.name} labelPlacement="start"/>
            )}
          </Box>
        </Box>);
    }
    return <></>;
  }

  return (
    <Box sx={{display: 'flex', flexDirection:'row', justifyItems: 'space'}}>
      <Box sx={{flex: 1}}>
        <Typography variant='h5'>{item.priority} {item.name}</Typography>
      </Box>
      {checkType()}
    </Box>
  );
}

export function SectionPage() {
  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <TemplateItem item={items[0]}></TemplateItem>
            <TemplateItem item={items[1]}></TemplateItem>
            <TemplateItem item={items[2]}></TemplateItem>
          </CardContent>    
        </Card>
      </Grid>
    </Grid>
  );
}