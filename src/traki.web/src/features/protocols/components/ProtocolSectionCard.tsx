import React from 'react';
import Box from '@mui/material/Box';
import { Accordion, AccordionDetails, AccordionSummary, Button, Card, Divider, Stack, Typography, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Section } from 'contracts/protocol';

type SectionItemProps = {
  section: Section
}

export function ProtocolSectionCard ({section}: SectionItemProps) {
  const theme = useTheme();
  const navigate = useNavigate();

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