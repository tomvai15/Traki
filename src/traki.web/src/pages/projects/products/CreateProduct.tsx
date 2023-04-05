import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Breadcrumbs, Button, Grid, Typography } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';

export function CreateProduct() {

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <Typography color="text.primary">Projects</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={12} md={12}>
        <Button color='secondary' variant='contained' startIcon={<AddIcon/>}>Add Project</Button>
      </Grid>
      <Grid item xs={12} md={12}>
        <Typography>Create product</Typography>
      </Grid>
    </Grid>
  );
}