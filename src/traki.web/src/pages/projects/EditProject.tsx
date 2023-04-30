import React, { useEffect, useState } from 'react';
import { Breadcrumbs, Button, Card, CardContent, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid, IconButton, Stack, TextField, Typography } from '@mui/material';
import projectService from '../../services/project-service';
import { Link as BreadLink } from '@mui/material';
import { PhotoCamera } from '@mui/icons-material';
import pictureService from '../../services/picture-service';
import { v4 as uuid } from 'uuid';
import { Project } from '../../contracts/projects/Project';
import { CreateProjectRequest } from '../../contracts/projects/CreateProjectRequest';
import { useNavigate, useParams } from 'react-router-dom';
import { validate, validationRules } from 'utils/textValidation';
import { DeleteItemDialog } from 'components/DeleteItemDialog';


const initialProject: Project = {
  id: 0,
  name: '',
  clientName: '',
  address: '',
  imageName: '',
  creationDate: ''
};

export function EditProject() {
  const navigate = useNavigate();
  const { projectId } = useParams();

  const [previewImage, setPreviewImage] = useState<string>();
  const [file, setFile] = useState<File>();

  const [initialImage, setInitialImage] = useState<string>('');  
  const [initialProjectJson, setInitialProjectJson] = useState<string>('');
  const [project, setProject] = useState<Project>(initialProject);

  const [openProductDialog, setOpenProductDialog] = React.useState(false);

  const handleProductDialogClose = () => {
    setOpenProductDialog(false);
  };

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
    return JSON.stringify(project) != initialProjectJson && validateProjectInputs(); 
  }

  async function submitProject() {
    if (!canSubmit()) {
      return;
    }
    let pictureName = initialImage;
    if (initialImage != project.imageName) {
      if (previewImage != '' && file) {
        pictureName = `${uuid()}${file.type.replace('image/','.')}`;
        const formData = new FormData();
        formData.append(pictureName, file, pictureName);
        await pictureService.uploadPicturesFormData('item', formData);
      } 
    }

    const updateProject: Project = {
      id: project.id,
      name: project.name,
      clientName: project.clientName,
      address: project.address,
      imageName: pictureName,
      creationDate: project.creationDate
    };

    const request: CreateProjectRequest = {
      project: updateProject
    };

    await projectService.updateProject(Number(projectId), request);
    await fetchProject();
  }

  function selectFile (event: React.ChangeEvent<HTMLInputElement>) {
    const { files } = event.target;
    const selectedFiles = files as FileList;
    setFile(selectedFiles?.[0]);
    setPreviewImage(URL.createObjectURL(selectedFiles?.[0]));
    setProject({...project, imageName: '????'});
  }

  function validateProjectInputs() {
    return !validate(project.name, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid &&
            !validate(project.address, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid &&
            !validate(project.clientName, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid;
  }

  async function deleteProject() {
    await projectService.deleteProject(project.id);
    navigate(`/projects`);
    handleProductDialogClose();
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
            <Stack direction={'row'} justifyContent={'space-between'} alignItems={'center'}>
              <Typography>Project Information</Typography>
              <Button onClick={() => setOpenProductDialog(true)} variant='contained' color='error'>Delete</Button>
            </Stack>       
            <TextField size='medium'
              inputProps={{ maxLength: 50 }}
              id="project-name"
              error={validate(project.name, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid}
              helperText={validate(project.name, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).message}
              label="Project name"
              variant="standard"
              value={project.name}
              onChange={(e) => setProject({...project, name: e.target.value})}
            />
            <TextField
              inputProps={{ maxLength: 50 }}
              id="project-address"
              error={validate(project.address, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid}
              helperText={validate(project.address, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).message}
              label="Address"
              variant="standard"
              value={project.address}
              onChange={(e) => setProject({...project, address: e.target.value})}
            />
            <TextField
              inputProps={{ maxLength: 50 }}
              id="project-client"
              error={validate(project.clientName, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).invalid}
              helperText={validate(project.clientName, [validationRules.nonEmpty, validationRules.noSpecialSymbols]).message}
              label="Client name"
              variant="standard"
              value={project.clientName}
              onChange={(e) => setProject({...project, clientName: e.target.value})}
            />
          </CardContent>  
          <CardContent>    
            <Button disabled={!canSubmit()} onClick={submitProject}  variant='contained'>
              Edit Project
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
      <DeleteItemDialog
        open={openProductDialog}
        handleClose={handleProductDialogClose}
        title={'Delete project'}
        body={`Are you sure you want to delete project ${project.name}?`}
        action={deleteProject}  
      />
    </Grid>
  );
}