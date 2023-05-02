import React from 'react';
import Box from '@mui/material/Box';
import { Accordion, AccordionDetails, AccordionSummary, Button, Card, Checkbox, Divider, FormControlLabel, IconButton, Stack, TextField, Typography, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Section } from 'contracts/protocol';
import { Item } from 'contracts/protocol/items';

type SectionItemProps = {
  section: Section
}

export function ProtocolSectionCard ({section}: SectionItemProps) {
  const theme = useTheme();
  const navigate = useNavigate();



  function checkType(item: Item) {
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
                    <TextField
                      size='small'
                      id="standard-disabled"
                      label={null}
                      variant="standard"
                      value={value.name}
                    />
                  </>
                } 
                labelPlacement="start"/>
            )}
          </Box>
        </Box>);
    }
    return <></>;
  }
  
  return (
    <Box sx={{padding: '5px'}}>
      <Card>
        <Accordion sx={{backgroundColor: theme.palette.grey[100]}}>
          <AccordionSummary expandIcon={<ExpandMoreIcon />}>
            <Typography>{section.name}</Typography>
          </AccordionSummary>
          <AccordionDetails>
            {section.checklist && section.checklist.items.map((item, index) => 
              <Box key={index}>
                <Stack direction={'column'} spacing={1}>
                  <Typography>
                    {item.name}
                  </Typography>
                </Stack>
                <Divider></Divider>
              </Box>)}  

            {section.table &&             
              <Stack height={20} direction={'row'} spacing={1}>
                <Divider style={{borderWidth: 1, borderColor: 'black'}} orientation={'vertical'}></Divider>
                {section.table.tableRows[0].rowColumns.map((item, index) => 
                  <Stack direction={'row'} key={index} spacing={1}>
                    <Typography>
                      {item.value}
                    </Typography>
                    <Divider style={{borderWidth: 1, borderColor: 'black'}} orientation={'vertical'}></Divider>
                  </Stack>)}
              </Stack>
            }             
            <Button sx={{marginTop: '10px'}} onClick={() => navigate(`/templates/protocols/${section.protocolId}/sections/${section.id}`)} variant='contained'>Details</Button>
          </AccordionDetails>
        </Accordion>
      </Card>
    </Box>
  );
}