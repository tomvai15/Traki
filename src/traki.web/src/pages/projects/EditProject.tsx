import React, { useEffect, useState } from 'react';
import { Breadcrumbs, Button, Card, CardContent, Grid, IconButton, TextField, Typography } from '@mui/material';
import projectService from '../../services/project-service';
import { Link as BreadLink } from '@mui/material';
import { PhotoCamera } from '@mui/icons-material';
import pictureService from '../../services/picture-service';
import { v4 as uuid } from 'uuid';
import { Project } from '../../contracts/projects/Project';
import { CreateProjectRequest } from '../../contracts/projects/CreateProjectRequest';
import { useParams } from 'react-router-dom';


const project: Project = {
  id: 0,
  name: '',
  clientName: '',
  address: '',
  imageName: ''
};

export function EditProject() {
  const { projectId } = useParams();

  const [previewImage, setPreviewImage] = useState<string>();
  const [file, setFile] = useState<File>();

  const [initialImage, setInitialImage] = useState<string>('');  
  const [initialProjectJson, setInitialProjectJson] = useState<string>('');
  const [initialProject, setProject] = useState<Project>(project);

  useEffect(() => {
    fetchProject();
  }, []);

  async function fetchProject () {
    const response = await projectService.getProject(Number(projectId));
    setProject(response.project);
    setInitialProjectJson(JSON.stringify(response.project));
    setInitialImage(response.project.imageName);

    const imageBase64 = await pictureService.getPicture('item', response.project.imageName);
    setPreviewImage(imageBase64);
  }

  function canSubmit () {
    return JSON.stringify(initialProject) != initialProjectJson; 
  }

  async function submitProject() {
    if (!canSubmit()) {
      return;
    }
    let pictureName = initialImage;
    if (initialImage != initialProject.imageName) {
      if (previewImage != '' && file) {
        pictureName = `${uuid()}${file.type.replace('image/','.')}`;
        const formData = new FormData();
        formData.append(pictureName, file, pictureName);
        await pictureService.uploadPicturesFormData('item', formData);
      } 
    }

    const project: Project = {
      id: initialProject.id,
      name: initialProject.name,
      clientName: initialProject.clientName,
      address: initialProject.address,
      imageName: pictureName
    };

    const request: CreateProjectRequest = {
      project: project
    };

    await projectService.updateProject(Number(projectId), request);
  }

  function selectFile (event: React.ChangeEvent<HTMLInputElement>) {
    const { files } = event.target;
    const selectedFiles = files as FileList;
    setFile(selectedFiles?.[0]);
    setPreviewImage(URL.createObjectURL(selectedFiles?.[0]));
    setProject({...initialProject, imageName: '????'});
  }

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
      <Grid item xs={6} md={6}>
        <Card title='Sample Project'>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <TextField size='medium'
              id="standard-read-only-input"
              label="Project name"
              variant="standard"
              value={initialProject.name}
              onChange={(e) => setProject({...initialProject, name: e.target.value})}
            />
            <TextField
              id="standard-read-only-input"
              label="Address"
              variant="standard"
              value={initialProject.address}
              onChange={(e) => setProject({...initialProject, address: e.target.value})}
            />
            <TextField
              id="standard-read-only-input"
              label="Client name"
              variant="standard"
              value={initialProject.clientName}
              onChange={(e) => setProject({...initialProject, clientName: e.target.value})}
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