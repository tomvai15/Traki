import React, { useEffect, useState } from 'react';
import { Button, Card, CardActions, CardContent, Grid, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { Protocol } from '../../contracts/protocol/Protocol';
import protocolService from '../../services/protocol-service';

export function TemplateProtocols() {

  const navigate = useNavigate();

  const [protocols, setProtocols] = useState<Protocol[]>([]);

  useEffect(() => {
    fetchProtocols();
  }, []);

  async function fetchProtocols() {
    const getProtocolsResponse = await protocolService.getTemplateProtocols();
    setProtocols(getProtocolsResponse.protocols);
  }

  return (
    <Grid container spacing={2}>
      {protocols.map((protocol, index) =>
        <Grid key={index} item xs={12} md={12} >
          <Card title='Sample Project'>
            <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
              <Typography variant="h5">{protocol.name}</Typography>
              <Typography variant="h6" fontSize={15} > Modified in {protocol.modifiedDate}</Typography>
            </CardContent>    
            <CardActions>  
              <Button onClick={() => navigate('/templates/protocols/1')} variant='contained' >Details</Button>
            </CardActions>  
          </Card>
        </Grid>)}
      <Grid item xs={12} md={12} >
        <Button variant='contained' >Add New Protocol</Button>
      </Grid>
    </Grid>
  );
}