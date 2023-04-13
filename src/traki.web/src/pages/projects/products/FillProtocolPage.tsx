import React, { useEffect, useState } from 'react';
import { Button, Grid, Typography } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { Section } from '../../../contracts/protocol/Section';
import protocolService from '../../../services/protocol-service';
import sectionService from '../../../services/section-service';
import { FillSection } from 'features/protocols/components';

const initialProtocol: Protocol = {
  id: 1,
  name: '',
  sections: [],
  isTemplate: false,
  modifiedDate: '',
  isSigned: false
};

export function FillProtocolPage() {
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