import React, { useEffect, useState } from 'react';
import { Breadcrumbs, Grid, Stack, Typography } from '@mui/material';
import { Protocol } from '../../contracts/protocol/Protocol';
import protocolService from '../../services/protocol-service';
import { NewProtocol, ProtocolCard, ProtocolsCard } from 'features/protocols/components';
import { ProtectedComponent } from 'components/ProtectedComponent';
import { DeleteItemDialog } from 'components/DeleteItemDialog';

export function TemplateProtocols() {
  const [protocols, setProtocols] = useState<Protocol[]>([]);
  const [selectedProtocol, setSelectedProtocol] = useState<Protocol>();

  const [openProductDialog, setOpenProductDialog] = React.useState(false);

  const openDialog = () => {
    setOpenProductDialog(true);
  };

  const handleProductDialogClose = () => {
    setOpenProductDialog(false);
  };

  useEffect(() => {
    fetchProtocols();
  }, []);

  async function fetchProtocols() {
    const response = await protocolService.getTemplateProtocols();
    setSelectedProtocol(response.protocols[0]);
    setProtocols(response.protocols);
  }

  function updateProtocol(protocol: Protocol) {
    setProtocols([...protocols.filter(x => x.id != protocol.id), protocol]);
  }

  async function deleteProtocol() {
    if (!selectedProtocol){
      return;
    }
    await protocolService.deleteProtocol(selectedProtocol.id);
    handleProductDialogClose();
    await fetchProtocols();
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <Typography color="text.primary">Protocol Templates</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={5} md={5} spacing={2}>
        <Stack spacing={2}>
          <Grid item xs={12} md={12}>
            <ProtocolsCard protocols={protocols} setSelectedProtocol={(protocol) => setSelectedProtocol(protocol)}/>
          </Grid>
          <Grid item xs={12} md={12}>
            <ProtectedComponent role='ProjectManager'>
              <NewProtocol protocols={protocols} fetchProtocols={fetchProtocols}/>
            </ProtectedComponent>
          </Grid>
        </Stack>
      </Grid>
      <Grid item xs={7} md={7}>
        <ProtocolCard deleteProtocol={openDialog} selectedProtocol={selectedProtocol} updateProtocol={updateProtocol}/>
      </Grid>
      <DeleteItemDialog
        open={openProductDialog}
        handleClose={handleProductDialogClose}
        title={'Delete protocol'}
        body={`Are you sure you want to delete protocol ${selectedProtocol?.name}?`}
        action={deleteProtocol}  
      />
    </Grid>
  );
}