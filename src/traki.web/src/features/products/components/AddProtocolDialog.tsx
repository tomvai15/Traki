import React, { useEffect, useState } from 'react';
import AssignmentIcon from '@mui/icons-material/Assignment';
import { Dialog, DialogContent, DialogTitle, ListItem, ListItemAvatar, ListItemButton, ListItemText } from '@mui/material';
import { Protocol } from '../../../contracts/protocol/Protocol';
import protocolService from '../../../services/protocol-service';
import { PaginatedList } from 'components/PaginatedList';

export interface Props {
  open: boolean;
  addProtocol: (id: number) => void;
  onclose: () => void;
}

export default function AddProtocolDialog(props: Props) {
  const { onclose, addProtocol, open } = props;

  const [protocols, setProtocols] = useState<Protocol[]>([]);

  useEffect(() => {
    fetchProtocols();
  }, []);

  async function fetchProtocols() {
    const getProtocolsResponse = await protocolService.getTemplateProtocols();
    setProtocols(getProtocolsResponse.protocols);
  }

  const handleListItemClick = (protocolId: number) => {
    addProtocol(protocolId);
    onclose();
  };

  return (
    <Dialog onClose={onclose} open={open}>
      <DialogTitle>Select protocol</DialogTitle>
      <DialogContent>
        <PaginatedList selector={x=> x.name} heightPerItem={80} items={protocols} renderItem={(item) => (
          <ListItem disableGutters>
            <ListItemButton id='protocol-item' onClick={() => handleListItemClick(item.id)}>
              <ListItemAvatar>
                <AssignmentIcon />
              </ListItemAvatar>
              <ListItemText primary={item.name} />
            </ListItemButton>
          </ListItem>
        )}/>
      </DialogContent>
    </Dialog>
  );
}