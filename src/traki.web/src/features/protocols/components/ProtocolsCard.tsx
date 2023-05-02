import React from 'react';
import { Card, CardContent, CardHeader, Divider, List, ListItemButton, ListItemText, useTheme } from '@mui/material';
import { Protocol } from 'contracts/protocol/Protocol';
import { PaginatedList } from 'components/PaginatedList';

type Props = {
  protocols: Protocol[],
  setSelectedProtocol: (protocol: Protocol) => void
}

export function ProtocolsCard({protocols, setSelectedProtocol}: Props) {
  const theme = useTheme();

  return (
    <Card>
      <CardHeader title={'Template protocols'}>
      </CardHeader>
      <Divider/>
      <CardContent>
        <PaginatedList selector={x=> x.name} items={protocols} renderItem={(item) => 
          <Card sx={{margin: '5px'}} >
            <ListItemButton onClick={() => setSelectedProtocol(item)} alignItems="flex-start" sx={{backgroundColor: theme.palette.grey[100]}}>
              <ListItemText
                primary={item.name}
                secondary='Modified in 2023-03-30'
              />
            </ListItemButton>
          </Card>
        }/>
      </CardContent>
    </Card>
  );
}