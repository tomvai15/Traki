import React, { useEffect, useState } from 'react';
import { Breadcrumbs, Button, Card, CardContent, CardHeader, Divider, Grid, IconButton, TextField, Typography } from '@mui/material';
import projectService from '../../../services/project-service';
import { Link as BreadLink } from '@mui/material';
import { PhotoCamera } from '@mui/icons-material';
import pictureService from '../../../services/picture-service';
import { v4 as uuid } from 'uuid';
import { Project } from '../../../contracts/projects/Project';
import { CreateProjectRequest } from '../../../contracts/projects/CreateProjectRequest';
import { CreateProductRequest } from 'contracts/product/CreateProductRequest';
import { useNavigate, useParams } from 'react-router-dom';
import { productService } from 'services';

export function CreateProduct() {

  const navigate = useNavigate();
  const { projectId } = useParams();
  const [previewImage, setPreviewImage] = useState<string>();
  const [file, setFile] = useState<File>();

  const [name, setName] = useState<string>('');

  function canSubmit () {
    return name;
  }

  async function submitProject() {
    if (!canSubmit()) {
      return;
    }
    
    const request: CreateProductRequest = {
      product: {
        id: 0,
        name: name,
        status: 'Active',
        projectId: Number(projectId),
        creationDate: ''
      }
    };

    const response = await productService.createProduct(Number(projectId), request);
    console.log('??????');

    navigate(`/projects/${projectId}/products/${response.product.id}`);
  }
  
  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <BreadLink color="inherit" href="/projects">
            Projects
          </BreadLink>
          <Typography color="text.primary">Create product</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={6} md={6}>
        <Card title='Sample Project'>
          <CardHeader title={'Product information'}>
          </CardHeader>
          <Divider/>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <TextField size='medium'
              id="propduct-name"
              label="Propduct name"
              variant="standard"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </CardContent>  
          <CardContent>    
            <Button disabled={!canSubmit()} onClick={submitProject}  variant='contained'>
              Create product
            </Button>
          </CardContent>  
        </Card>
      </Grid>
    </Grid>
  );
}