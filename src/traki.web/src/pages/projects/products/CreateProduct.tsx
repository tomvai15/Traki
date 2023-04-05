import React, { useEffect, useState } from 'react';
import { Breadcrumbs, Button, Card, CardContent, Grid, IconButton, TextField, Typography } from '@mui/material';
import projectService from '../../../services/project-service';
import { Link as BreadLink } from '@mui/material';
import { PhotoCamera } from '@mui/icons-material';
import pictureService from '../../../services/picture-service';
import { v4 as uuid } from 'uuid';
import { Project } from '../../../contracts/projects/Project';
import { CreateProjectRequest } from '../../../contracts/projects/CreateProjectRequest';

export function CreateProduct() {

  const [previewImage, setPreviewImage] = useState<string>();
  const [file, setFile] = useState<File>();

  const [name, setName] = useState<string>('');
  const [client, setClient] = useState<string>('');
  const [address, setAddress] = useState<string>('');

  function canSubmit () {
    return name && client && address && previewImage && file;
  }

  async function submitProject() {
    if (!canSubmit()) {
      return;
    }
    let pictureName = '';
    if (previewImage != '' && file) {
      pictureName = `${uuid()}${file.type.replace('image/','.')}`;
      const formData = new FormData();
      formData.append(pictureName, file, pictureName);
      await pictureService.uploadPicturesFormData('item', formData);
    }

    const project: Project = {
      id: 0,
      name: name,
      clientName: client,
      address: address,
      imageName: pictureName
    };

    const request: CreateProjectRequest = {
      project: project
    };

    await projectService.createProject(request);
  }

  function selectFile (event: React.ChangeEvent<HTMLInputElement>) {
    const { files } = event.target;
    const selectedFiles = files as FileList;
    setFile(selectedFiles?.[0]);
    setPreviewImage(URL.createObjectURL(selectedFiles?.[0]));
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
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <TextField size='medium'
              id="standard-read-only-input"
              label="Project name"
              variant="standard"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
            <TextField
              id="standard-read-only-input"
              label="Address"
              variant="standard"
              value={address}
              onChange={(e) => setAddress(e.target.value)}
            />
            <TextField
              id="standard-read-only-input"
              label="Client name"
              variant="standard"
              value={client}
              onChange={(e) => setClient(e.target.value)}
            />
          </CardContent>  
          <CardContent>    
            <Button disabled={!canSubmit()} onClick={submitProject}  variant='contained'>
              Create Project
            </Button>
          </CardContent>  
        </Card>
      </Grid>
      <Grid item xs={6} md={6}>
        <Card sx={{height: "100%", padding: 5, paddingBottom: 0}}>
          <CardContent>
            <img style={{maxHeight: '300px', width: 'auto', borderRadius: '2%',}} 
              src={previewImage ? previewImage : 'https://i0.wp.com/roadmap-tech.com/wp-content/uploads/2019/04/placeholder-image.jpg?resize=400%2C400&ssl=1'}></img>
          </CardContent>  
          <CardContent>
            <IconButton color="secondary" aria-label="upload picture" component="label">
              <input hidden accept="image/*" type="file" onChange={selectFile} />
              <PhotoCamera />
            </IconButton>
          </CardContent>  
        </Card>
      </Grid>
    </Grid>
  );
}