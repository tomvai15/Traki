import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Avatar, Button, Card, CardActions, CardContent, Dialog, DialogTitle, Divider, Grid, List, ListItem, ListItemAvatar, ListItemButton, ListItemText, Table, TableBody, TableCell, TableHead, TableRow, Typography } from '@mui/material';
import BuildCircleOutlinedIcon from '@mui/icons-material/BuildCircleOutlined';
import productService from '../../../services/product-service';
import { Product } from '../../../contracts/product/Product';
import { useNavigate, useParams } from 'react-router-dom';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { blue } from '@mui/material/colors';
import PersonIcon from '@mui/icons-material/Person';
import AddIcon from '@mui/icons-material/Add';
import protocolService from '../../../services/protocol-service';
import AssignmentIcon from '@mui/icons-material/Assignment';
import { Section } from '../../../contracts/protocol/Section';
import sectionService from '../../../services/section-service';
import { Checklist } from '../../../contracts/protocol/Checklist';

const initialProtocol: Protocol = {
  id: 1,
  name: '',
  sections: [],
  isTemplate: false,
  modifiedDate: ''
};

const initialSection: Section = {
  id: 0,
  name: '',
  priority: 1,
  checklist: undefined,
  table: undefined
};

type FillSectionProps = {
  protocolId: number,
  sectionId: number
}

function FillSection({protocolId, sectionId}: FillSectionProps) {

  const [section, setSection] = useState<Section>(initialSection);

  useEffect(() => {
    fetchSection();
  }, []);

  async function fetchSection() {
    const getSectionResponse = await sectionService.getSection(Number(protocolId), Number(sectionId));
    orderAndSetSection(getSectionResponse.section);
  }

  function orderAndSetSection(sectionToSort: Section) {
    if (!sectionToSort.checklist)
    {
      setSection(sectionToSort);
      return;
    }
    const sortedItems = [...sectionToSort.checklist.items];
    sortedItems.sort((a, b) => a.priority - b.priority);

    const copiedChecklist: Checklist = {...sectionToSort.checklist, items: sortedItems};
    const copiedSection: Section = {...sectionToSort, checklist: copiedChecklist};
    setSection(copiedSection);
  }

  return (
    <Box sx={{backgroundColor: 'red'}}>
      adsasd
      {section.name}
    </Box>
  );
}


export function FillProtocol() {
  const { projectId, productId, protocolId } = useParams();
  const [protocol, setProtocol] = useState<Protocol>(initialProtocol);
  const [sections, setSections] = useState<Section[]>([]);

  useEffect(() => {
    fetchProtocol();
  }, []);

  async function fetchProtocol() {
    const getProtocolsResponse = await protocolService.getProtocol(Number(protocolId));
    const getSectionsResponse = await sectionService.getSections(Number(protocolId));
    setProtocol(getProtocolsResponse.protocol);
    orderAndSetSections(getSectionsResponse.sections);
  }

  function orderAndSetSections(sectionsToSort: Section[]) {
    const sortedItems = [...sectionsToSort];

    console.log(sectionsToSort);
    sortedItems.sort((a, b) => a.priority - b.priority);
    setSections(sortedItems);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Typography variant='h5'>{protocol.name}</Typography>
        {sections.map((section, index) => 
          <FillSection key={index} protocolId={Number(protocolId)} sectionId={section.id}></FillSection>)}
      </Grid>
    </Grid>
  );
}