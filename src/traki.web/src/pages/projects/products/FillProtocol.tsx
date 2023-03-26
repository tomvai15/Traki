import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Avatar, Button, Card, CardActions, CardContent, Checkbox, Dialog, DialogTitle, Divider, FormControlLabel, Grid, List, ListItem, ListItemAvatar, ListItemButton, ListItemText, Table, TableBody, TableCell, TableHead, TableRow, TextField, Typography } from '@mui/material';
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
import { Item } from '../../../contracts/protocol/items/Item';

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

type FillItemProps = {
  item: Item,
  updateItem: (item: Item) => void
}

function FillItem ({item, updateItem}: FillItemProps) {
  function checkType() {
    if (item.question) {
      return (
        <Box sx={{flex: 3, display: 'flex', flexDirection:'row'}}> 
          <Box sx={{flex: 3}}>
            <FormControlLabel control={<Checkbox/>} label="Yes" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox/>} label="No" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox/>} label="Other" labelPlacement="start"/>
            <FormControlLabel control={<Checkbox/>} label="Not applicable" labelPlacement="start"/>
          </Box>
          <Box sx={{flex: 3}}>
            <TextField sx={{width: '100%'}}
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
            { item.multipleChoice.options.map((value, index) => 
              <FormControlLabel 
                key={index} 
                control={<Checkbox/>} 
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
    console.log(getSectionResponse);
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
    <Box>
      <Card sx={{margin:2}}>
        <CardActions>
          <Box sx={{display: 'flex', width: '100%', flexDirection: 'row', justifyContent: 'space-between'}}>
            <Box>
              <Typography variant='h6'>{section.name}</Typography>
            </Box>
            <Box>
              <Button variant='contained'>Save Answers</Button>
            </Box>
          </Box>
        </CardActions>
      </Card>
      <Box sx={{marginLeft:3}}>
        {section.checklist?.items.map((item, index) => 
          <FillItem key={index} item={item} updateItem={()=> console.log('')}></FillItem>
        )}
      </Box>
    </Box>
  );
}


export function FillProtocol() {
  const navigate = useNavigate();
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
        <Button onClick={() => navigate(`/projects/${projectId}/products/${productId}`)} variant='contained' >Go back</Button>
      </Grid>
      <Grid item xs={12} md={12} >
        <Typography variant='h5'>{protocol.name}</Typography>
      </Grid>
      {sections.map((section, index) => 
        <Grid key={index} item xs={12} md={12} >
          <FillSection protocolId={Number(protocolId)} sectionId={section.id}></FillSection>
        </Grid>)}
    </Grid>
  );
}