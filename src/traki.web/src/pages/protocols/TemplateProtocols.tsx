import React, { useEffect, useState } from 'react';
import { Box, Button, Card, CardActions, CardContent, Grid, TextField, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { Protocol } from '../../contracts/protocol/Protocol';
import protocolService from '../../services/protocol-service';
import { CreateProtocolRequest } from '../../contracts/protocol/CreateProtocolRequest';

export function TemplateProtocols() {

  const navigate = useNavigate();

  const [protocolName, setProtocolName] = useState<string>('');
  const [protocols, setProtocols] = useState<Protocol[]>([]);

  useEffect(() => {
    fetchProtocols();
  }, []);

  async function fetchProtocols() {
    const getProtocolsResponse = await protocolService.getTemplateProtocols();
    setProtocols(getProtocolsResponse.protocols);
  }

  async function createProtocol() {

    const newProtocol: Protocol = {
      id:0,
      name: protocolName,
      isTemplate: true,
      modifiedDate: '',
      sections: [],
      isSigned: false
    };
    const createProtocolRequest: CreateProtocolRequest = {
      protocol: newProtocol
    };
    await protocolService.createProtocol(createProtocolRequest);
    await fetchProtocols();
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
              <Button onClick={() => navigate('/templates/protocols/' + protocol.id)} variant='contained' >Details</Button>
            </CardActions>  
          </Card>
        </Grid>)}
      <Grid item xs={12} md={12}  sx={{ display: 'flex', flexDirection: 'column'}}>
        <Card>
          <CardContent> 
            <Typography>New Protocol</Typography> 
            <Box>
              <TextField sx={{width: '50%'}}
                id="standard-disabled"
                label="Protocol Name"
                variant="standard"
                value={protocolName}
                onChange={(e) => setProtocolName(e.target.value)}
              />
            </Box>
            <Box>
              <Button onClick={createProtocol} disabled={!protocolName.length} variant='contained' >Add New Protocol</Button>
            </Box>
          </CardContent>  
        </Card>
      </Grid>
    </Grid>
  );
}