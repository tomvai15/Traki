import React, { useState } from 'react';
import { Box, Button, Card, CardContent, CardHeader, Divider, Stack, TextField } from '@mui/material';
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
      isSigned: false,
      isCompleted: false
    };
    const createProtocolRequest: CreateProtocolRequest = {
      protocol: newProtocol
    };
    await protocolService.createProtocol(createProtocolRequest);
    await fetchProtocols();
  }

  return (
    <Card>
      <CardHeader title={'New protocol'}>
      </CardHeader>
      <Divider/>
      <CardContent> 
        <Stack spacing={1}>
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