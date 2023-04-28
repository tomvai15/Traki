import React, { useEffect, useState } from 'react';
import AssignmentIcon from '@mui/icons-material/Assignment';
import { Link as BreadLink, Dialog, DialogTitle, List, ListItem, ListItemAvatar, ListItemButton, ListItemText } from '@mui/material';
import { Protocol } from '../../../contracts/protocol/Protocol';
import protocolService from '../../../services/protocol-service';
import { PaginatedList } from 'components/PaginatedList';

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
      <PaginatedList height='380px' items={protocols} renderItem={(item) => (
        <ListItem disableGutters>
          <ListItemButton onClick={() => handleListItemClick(item.id)}>
            <ListItemAvatar>
              <AssignmentIcon />
            </ListItemAvatar>
            <ListItemText primary={  item.id +' ' + item.name} />
          </ListItemButton>
        </ListItem>
      )}/>
    </Dialog>
  );
}