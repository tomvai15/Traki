import React from 'react';
import Box from '@mui/material/Box';
import { Accordion, AccordionDetails, AccordionSummary, Button, Card, Divider, Typography, useTheme } from '@mui/material';
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
            {section.checklist?.items.map((item, index) => 
              <Box key={index}>
                <Typography>
                  {item.name}
                </Typography>
                <Divider></Divider>
              </Box>)}
            <Button onClick={() => navigate(`/templates/protocols/${section.protocolId}/sections/${section.id}`)} variant='contained'>Edit</Button>
          </AccordionDetails>
        </Accordion>
      </Card>
    </Box>
  );
}