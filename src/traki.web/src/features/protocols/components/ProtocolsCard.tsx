import React from 'react';
import { Card, CardContent, List, ListItemButton, ListItemText, useTheme } from '@mui/material';
import { Protocol } from 'contracts/protocol/Protocol';

type Props = {
  protocols: Protocol[],
  setSelectedProtocol: (protocol: Protocol) => void
}

export function ProtocolsCard({protocols, setSelectedProtocol}: Props) {
  const theme = useTheme();

  return (
    <Card>
      <CardContent>
        <List component="nav">
          {protocols.map((protocol, index) =>
            <Card key={index} sx={{margin: '5px'}} >
              <ListItemButton onClick={() => setSelectedProtocol(protocol)} key={index} alignItems="flex-start" sx={{backgroundColor: theme.palette.grey[100]}}>
                <ListItemText
                  primary={protocol.name}
                  secondary='Modified in 2023-03-30'
                />
              </ListItemButton>
            </Card>)}
        </List>
      </CardContent>
    </Card>
  );
}