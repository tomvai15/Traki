import React, { useState } from 'react';
import { Box, Button, Card, CardContent, CardHeader, Divider, Stack, TextField } from '@mui/material';
import { Protocol } from 'contracts/protocol/Protocol';
import { CreateProtocolRequest } from 'contracts/protocol/CreateProtocolRequest';
import { protocolService } from 'services';
import { validate, validationRules } from 'utils/textValidation';

type Props = {
  fetchProtocols: () => void
  protocols: Protocol[]
}

export function NewProtocol({fetchProtocols, protocols}: Props) {

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

  function canSubmit() {
    return protocolName.length!=0 && validateInputs() && !protocols.map(x=> x.name).includes(protocolName);
  }

  function validateInputs() {
    return !validate(protocolName, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid;
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
            error={validate(protocolName, [validationRules.noSpecialSymbols]).invalid}
            helperText={validate(protocolName, [validationRules.noSpecialSymbols]).message}
            id="new-protocol-name"
            label="Protocol Name"
            variant="standard"
            value={protocolName}
            onChange={(e) => setProtocolName(e.target.value)}
          />
          <Box >
            <Button id="create-protocol" onClick={createProtocol} disabled={!canSubmit()} variant='contained' >Add New Protocol</Button>
          </Box>
        </Stack>
      </CardContent>  
    </Card>
  );
}