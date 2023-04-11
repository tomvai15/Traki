import React from 'react';
import Box from '@mui/material/Box';
import { Accordion, AccordionDetails, AccordionSummary, Button, Divider, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Section } from 'contracts/protocol';

type SectionItemProps = {
  section: Section
}

export function ProtocolSectionCard ({section}: SectionItemProps) {
  const navigate = useNavigate();
  return (
    <Box sx={{padding: 1}}>
      <Accordion>
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
          <Button onClick={() => navigate(`sections/${section.id}`)} variant='contained'>Edit</Button>
        </AccordionDetails>
      </Accordion>
    </Box>
  );
}