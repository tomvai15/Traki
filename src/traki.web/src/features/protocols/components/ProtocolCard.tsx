import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, Grid, Stack, TextField, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { Section } from 'contracts/protocol';
import { Protocol } from 'contracts/protocol/Protocol';
import { UpdateProtocolRequest } from 'contracts/protocol/UpdateProtocolRequest';
import { protocolService, sectionService } from 'services';
import { ProtocolSections } from './ProtocolSections';

type Props = {
  selectedProtocol?: Protocol,
  updateProtocol: (protocol: Protocol) => void
}

export function ProtocolCard({selectedProtocol, updateProtocol}: Props) {
  const navigate = useNavigate();

  const [protocol, setProtocol] = useState<Protocol>();
  const [sections, setSections] = useState<Section[]>([]);

  const [initialSectionsJson,  setInitialSectionsJson] = useState<string>('');
  const [initialProtocolJson,  setInitialProtocolJson] = useState<string>('');

  useEffect(() => {
    fetchProtocol();
  }, [selectedProtocol]);

  function canUpdate() {
    return (JSON.stringify(sections) != initialSectionsJson) || (JSON.stringify(protocol) != initialProtocolJson);
  }

  async function updateProtocolAndSection() {
    if (!protocol) {
      return;
    }

    const updateProtocolRequest: UpdateProtocolRequest ={
      protocol: protocol,
      sections: sections
    };
    await protocolService.updateProtocol(protocol.id, updateProtocolRequest);
    setInitialProtocolJson(JSON.stringify(protocol));
    setInitialSectionsJson(JSON.stringify(sections));
  }

  async function fetchProtocol() {
    if (!selectedProtocol) {
      return;
    }
    const getProtocolsResponse = await protocolService.getProtocol(selectedProtocol.id);
    const getSectionsResponse = await sectionService.getSections(selectedProtocol.id);
    setProtocol(getProtocolsResponse.protocol);
    setInitialProtocolJson(JSON.stringify(getProtocolsResponse.protocol));
    orderAndSetSections(getSectionsResponse.sections);
  }

  function orderAndSetSections(sectionsToSort: Section[]) {
    const sortedItems = [...sectionsToSort];

    sortedItems.sort((a, b) => a.priority - b.priority);
    setSections(sortedItems);
    setInitialSectionsJson(JSON.stringify(sortedItems));
  }

  function updateName(newName: string) {
    if (!protocol) {
      return;
    }
    setProtocol({...protocol, name: newName});
  }

  if (!protocol) {
    return (<Card sx={{height: 200}}></Card>);
  }

  return (
    <Card>
      <CardContent>
        <Stack direction='row' justifyContent={'space-between'}>
          <TextField sx={{width: '50%'}}
            id="standard-disabled"
            label="Protocol Name"
            variant="standard"
            value={protocol.name}
            onChange={(e) => updateName(e.target.value)}
          />
          <Button onClick={() => updateProtocolAndSection()} disabled={!canUpdate()} variant='contained' >Save</Button>
        </Stack>
        <ProtocolSections sections={sections} setSections={setSections}/>
        <Button onClick={() => navigate(`/templates/protocols/${protocol.id}/sections/create`)} variant='contained' >Add Section</Button>
      </CardContent>
    </Card>
  );
}