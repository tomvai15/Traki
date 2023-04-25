import React, { useState } from 'react';
import { Box, Button, Card, CardContent, Stack, TextField, Typography } from '@mui/material';
import { Protocol } from 'contracts/protocol/Protocol';
import { CreateProtocolRequest } from 'contracts/protocol/CreateProtocolRequest';
import { protocolService } from 'services';

type Props = {
  fetchProtocols: () => void
}

export function NewProtocol({fetchProtocols}: Props) {

  const [protocolName, setProtocolName] = useState<string>('');

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
    <Card>
      <CardContent> 
        <Stack spacing={1}>
          <Typography>New Protocol</Typography> 
          <TextField
            sx={{marginTop: '-10px'}}
            id="standard-disabled"
            label="Protocol Name"
            variant="standard"
            value={protocolName}
            onChange={(e) => setProtocolName(e.target.value)}
          />
          <Box >
            <Button onClick={createProtocol} disabled={!protocolName.length} variant='contained' >Add New Protocol</Button>
          </Box>
        </Stack>
      </CardContent>  
    </Card>
  );
}