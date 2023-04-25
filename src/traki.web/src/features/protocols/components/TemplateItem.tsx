import React from 'react';
import { Card, Typography, Box, FormControlLabel, Checkbox, TextField, Button, Menu, MenuItem, IconButton, useTheme } from '@mui/material';
import { Question, TextInput, MultipleChoice, Item, Option } from 'contracts/protocol/items';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import ClearIcon from '@mui/icons-material/Clear';
import { v4 as uuid } from 'uuid';
import DragIndicatorIcon from '@mui/icons-material/DragIndicator';

const defaultQuestion: Question = {
  id: uuid(), 
  comment: '',
  answer: undefined
};

const defaultTextInput: TextInput = {
  id: uuid(), 
  value: ''
};

const defaultMultipleChoice: MultipleChoice = {
  id: uuid(), 
  options: [{id: uuid(), name: 'Option A',   selected: false}, {id: uuid(), name: 'Option B',   selected: false}],
};

type TemplateItemProps = {
  item: Item
  deleteItem: (id: string) => void
  updateItem: (item: Item) => void
};

export function TemplateItem ({item, deleteItem, updateItem}: TemplateItemProps) {
  const theme = useTheme();
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);
  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  function changeToQuestion(item: Item) {
    item.textInput = undefined;
    item.multipleChoice = undefined;
    item.question = {...defaultQuestion};
    updateItem(item);
  }

  function changeToTextInput(item: Item) {
    item.multipleChoice = undefined;
    item.question = undefined;
    item.textInput = {...defaultTextInput};
    updateItem(item);
  }

  function changeToMultipleChoice(item: Item) {
    item.textInput = undefined;
    item.question = undefined;
    item.multipleChoice = {...defaultMultipleChoice};
    updateItem(item);
  }

  function addOption(item: Item) {
    if (!item.multipleChoice) return;

    const letter = (item.multipleChoice.options.length + 10).toString(36).toUpperCase();

    const newValue: Option = {id: uuid(), name: 'Option ' + letter,   selected: false};
    item.multipleChoice.options = [...item.multipleChoice.options, newValue];
    updateItem(item);
  }

  function removeOption(item: Item, valueId: string) {
    if (!item.multipleChoice) return;

    item.multipleChoice.options = item.multipleChoice.options.filter(v => v.id!= valueId);
    updateItem(item);
  }

  function updateOption(item: Item, valueId: string, updatedName: string) {
    if (!item.multipleChoice) return;

    const copiedOptions = [...item.multipleChoice.options];
    copiedOptions.forEach((element, index) => {
      copiedOptions[index] = element.id == valueId ? {...element, name: updatedName}  : element;
    });

    item.multipleChoice.options = copiedOptions;
    updateItem(item);
  }

  function updateName(item: Item, name: string) {
    item.name = name;
    updateItem(item);
  }

  function checkType() {
    if (item.question) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row', alignItems: 'flex-end'}}> 
          <Box sx={{flex: 3}}>
            <FormControlLabel control={<Checkbox disabled />} label="Yes" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled />} label="No" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled />} label="Other" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox disabled />} label="Not applicable" labelPlacement="start"/>
          </Box>
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              inputProps={{min: 0, style: { textAlign: 'center' }}}
              disabled
              id="standard-disabled"
              label="Comment"
              variant="standard"
            />
          </Box>
        </Box>);
    } else if (item.textInput) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row', alignItems: 'flex-end'}}> 
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
              inputProps={{min: 0, style: { textAlign: 'center' }}}
              disabled
              id="standard-disabled"
              label="Comment"
              variant="standard"
            />
          </Box>
        </Box>);
    } else if (item.multipleChoice) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row', alignItems: 'flex-end'}}> 
          <Box sx={{flex: 3}}>
            { item.multipleChoice.options.map((value, index) => 
              <FormControlLabel 
                key={index} 
                control={<Checkbox disabled />} 
                label={
                  <>
                    <IconButton onClick={() => removeOption(item, value.id)}>
                      <ClearIcon color={'error'}/>
                    </IconButton> 
                    <TextField
                      size='small'
                      id="standard-disabled"
                      label={null}
                      variant="standard"
                      value={value.name}
                      onChange={(e) => updateOption(item, value.id, e.target.value)}
                    />
                  </>
                } 
                labelPlacement="start"/>
            )}
          </Box>
          <Box sx={{alignSelf: 'flex-start'}}>
            <Button onClick={() => addOption(item)}>Add</Button>
          </Box>
        </Box>);
    }
    return <></>;
  }

  return (
    <Box sx={{padding: 1}}>
      <Card sx={{backgroundColor: theme.palette.grey[100], display: 'flex', padding: 1, flexDirection:'row', justifyItems: 'space'}}>
        <Box sx={{flex: 1}}>
          <TextField sx={{width: '90%'}}
            id="standard-disabled"
            multiline
            label="Question"
            variant="standard"
            value={item.name}
            onChange={(e) => updateName(item, e.target.value)}
          />
        </Box>
        {checkType()}
        <Box>
          <IconButton onClick={handleOpenUserMenu}>
            <MoreVertIcon/>
          </IconButton> 
          <Menu
            sx={{ mt: '45px' }}
            id="menu-appbar"
            anchorEl={anchorElUser}
            anchorOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
            keepMounted
            transformOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
            open={Boolean(anchorElUser)}
            onClose={handleCloseUserMenu}
          >
            <MenuItem onClick={() => {changeToQuestion(item); handleCloseUserMenu();}}>
              <Typography textAlign="center">Default Question</Typography>
            </MenuItem>
            <MenuItem onClick={() => {changeToMultipleChoice(item); handleCloseUserMenu();}}>
              <Typography textAlign="center">Multiple Choice</Typography>
            </MenuItem>
            <MenuItem onClick={() => {changeToTextInput(item); handleCloseUserMenu();}}>
              <Typography textAlign="center">Text Input</Typography>
            </MenuItem>
            <MenuItem onClick={() => {handleCloseUserMenu(); deleteItem(item.id);}}>
              <Typography color={'red'} textAlign="center">Delete</Typography>
            </MenuItem>
          </Menu>
        </Box>
      </Card>
    </Box>
  );
}