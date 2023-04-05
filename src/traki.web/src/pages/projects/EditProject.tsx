import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Breadcrumbs, Button, Card, CardContent, Divider, Grid, List, ListItem, ListItemButton, ListItemText, Table, TableBody, TableCell, TableRow, Typography } from '@mui/material';
import BuildCircleOutlinedIcon from '@mui/icons-material/BuildCircleOutlined';
import projectService from '../../services/project-service';
import productService from '../../services/product-service';
import { Link as BreadLink } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';
import AddIcon from '@mui/icons-material/Add';

export function EditProject() {

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <BreadLink color="inherit" href="/projects">
            Projects
          </BreadLink>
          <Typography color="text.primary">Edit project</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={12} md={12}>
        <Button color='secondary' variant='contained' startIcon={<AddIcon/>}>Add Project</Button>
      </Grid>
      <Grid item xs={12} md={12}>
        <Typography>Create project</Typography>
      </Grid>
    </Grid>
  );
}