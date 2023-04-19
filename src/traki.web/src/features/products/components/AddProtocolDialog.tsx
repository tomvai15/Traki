import React, { useEffect, useState } from 'react';
import AssignmentIcon from '@mui/icons-material/Assignment';
import { Link as BreadLink, Dialog, DialogTitle, List, ListItem, ListItemAvatar, ListItemButton, ListItemText } from '@mui/material';
import { Protocol } from '../../../contracts/protocol/Protocol';
import protocolService from '../../../services/protocol-service';

export interface Props {
  open: boolean;
  selectedValue: string;
  addProtocol: (id: number) => void;
  onclose: () => void;
}

export default function AddProtocolDialog(props: Props) {
  const { onclose, addProtocol, selectedValue, open } = props;

  const [protocols, setProtocols] = useState<Protocol[]>([]);

  useEffect(() => {
    fetchProtocols();
  }, []);

  async function fetchProtocols() {
    const getProtocolsResponse = await protocolService.getTemplateProtocols();
    setProtocols(getProtocolsResponse.protocols);
  }

  const handleListItemClick = (protocolId: number) => {
    console.log(protocolId);
    addProtocol(protocolId);
    console.log('????');
    onclose();
  };

  return (
    <Dialog onClose={onclose} open={open}>
      <DialogTitle>Select protocol</DialogTitle>
      <List sx={{ pt: 0 }}>
        {protocols.map((protocol, index) => (
          <ListItem key={index} disableGutters>
            <ListItemButton onClick={() => handleListItemClick(protocol.id)}>
              <ListItemAvatar>
                <AssignmentIcon />
              </ListItemAvatar>
              <ListItemText primary={  protocol.id +' ' + protocol.name} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Dialog>
  );
}