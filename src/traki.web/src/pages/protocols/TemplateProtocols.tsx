import React, { useEffect, useState } from 'react';
import { Breadcrumbs, Grid, Typography } from '@mui/material';
import { Protocol } from '../../contracts/protocol/Protocol';
import protocolService from '../../services/protocol-service';
import { NewProtocol, ProtocolCard, ProtocolsCard } from 'features/protocols/components';

export function TemplateProtocols() {
  const [protocols, setProtocols] = useState<Protocol[]>([]);
  const [selectedProtocol, setSelectedProtocol] = useState<Protocol>();

  useEffect(() => {
    fetchProtocols();
  }, []);

  async function fetchProtocols() {
    const response = await protocolService.getTemplateProtocols();
    setSelectedProtocol(response.protocols[0]);
    setProtocols(response.protocols);
  }

  function updateProtocol(protocol: Protocol) {
    setProtocols([...protocols.filter(x => x.id != protocol.id), protocol]);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <Typography color="text.primary">Protocol Templates</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid container item xs={5} md={5} spacing={2}>
        <Grid item xs={12} md={12}>
          <ProtocolsCard protocols={protocols} setSelectedProtocol={(protocol) => setSelectedProtocol(protocol)}/>
        </Grid>
        <Grid item xs={12} md={12}>
          <NewProtocol fetchProtocols={fetchProtocols}/>
        </Grid>
      </Grid>
      <Grid item xs={7} md={7}>
        <ProtocolCard selectedProtocol={selectedProtocol} updateProtocol={updateProtocol}/>
      </Grid>
    </Grid>
  );
}