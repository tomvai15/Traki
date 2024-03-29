import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Breadcrumbs, Button, Card, CardActions, CardContent, CardMedia, Grid, TextField, Typography } from '@mui/material';
import companyService from '../../services/company-service';
import { Company } from '../../contracts/company/Company';
import { UpdateCompanyRequest } from '../../contracts/company/UpdateCompanyRequest';
import pictureService from '../../services/picture-service';
import FileUploadIcon from '@mui/icons-material/FileUpload';
import { useAlert } from 'hooks/useAlert';

// TODO: allow only specific resolution

export function CompanyPage() {
  const { displaySuccess  } = useAlert();
  const [previewImage, setPreviewImage] = useState<string>();
  const [company, setCompany] = useState<Company>();
  const [canEdit, setCanEdit] = useState<boolean>(false);

  const [file, setFile] = useState<File>();
  const [image, setImage] = useState<string>('');
  const [name, setName] = useState<string>('');
  const [address, setAddress] = useState<string>('');
  const [phone, setPhone] = useState<string>('sadsadsa');

  useEffect(() => {
    fetchCompany();
  }, []);
  
  const selectFile = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { files } = event.target;
    const selectedFiles = files as FileList;
    setFile(selectedFiles?.[0]);
    setPreviewImage(URL.createObjectURL(selectedFiles?.[0]));
  };

  async function fetchCompany() {
    const getCompanyResponse = await companyService.getCompany();
    setCompany(getCompanyResponse.company);

    setName(getCompanyResponse.company.name);
    setAddress(getCompanyResponse.company.address);
    setPhone(getCompanyResponse.company.phoneNumber);

    const image = await pictureService.getPicture('company', getCompanyResponse.company.imageName);
    setImage(image);
  }

  async function updateCompany() {
    setCanEdit(false);
    
    if (!company) {
      return;
    }

    const updateCompanyRequest: UpdateCompanyRequest = {
      company: company
    };

    updateCompanyRequest.company.name = name;
    updateCompanyRequest.company.address = address;
    updateCompanyRequest.company.phoneNumber = phone;
    
    await companyService.updateCompany(updateCompanyRequest);
    setCanEdit(false);
    displaySuccess("Company information was updated successfully");
  }

  async function updateLogo() {
    if (!company || !file) {
      return;
    }
    await pictureService.uploadPicture('company', company.imageName, file);
    setFile(undefined);
    setPreviewImage(undefined);
    await fetchCompany();
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <Typography color="text.primary">Company information</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={6} md={4} >
        <Card title='Sample Project'>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <TextField size='medium'
              id="standard-read-only-input"
              label="Company name"
              defaultValue='...'
              value={name}
              onChange={(e) => setName(e.target.value)}
              InputProps={{
                readOnly: !canEdit,
              }}
              variant="standard"
            />
            <TextField
              id="standard-read-only-input"
              label="Address"
              defaultValue='...'
              value={address}
              onChange={(e) => setAddress(e.target.value)}
              InputProps={{
                readOnly: !canEdit,
              }}
              variant="standard"
            />
          </CardContent>    
          {canEdit ?     
            <CardActions>
              <Button variant='contained' onClick={() => updateCompany()} size="small">Update</Button>
            </CardActions> : 
            <CardActions>
              <Button variant='outlined' onClick={() => setCanEdit(true)} size="small">Edit</Button>
            </CardActions>}
        </Card>
      </Grid>
      <Grid item xs={6} md={8}>
        <Card sx={{height: "100%", padding: 5, paddingBottom: 0}}>
          <CardContent sx={{ height: "100%"}}>
            { image.length == 0 ? 
              <Box sx={{backgroundColor: '#ededed', height: "100%"}} ></Box> :
              <CardMedia
                component="img"
                image={previewImage ? previewImage : image}
                alt="random"
              />}
            <Button component="label" variant='contained' startIcon={<FileUploadIcon/>} size="small" sx={{marginRight: 1}}>
              Change Logo
              <input onChange={selectFile}
                type="file"
                hidden
              />
            </Button>
            <Button disabled={!previewImage} onClick={updateLogo} component="label" variant='contained' startIcon={<FileUploadIcon/>} size="small">
              Upload
            </Button>
          </CardContent>    
        </Card>
      </Grid>
    </Grid>
  );
}

/*

<Grid item xs={12} md={6}>
        <Card sx={{padding: 5}}>
          <Grid container spacing={2}>
            <Grid item xs={6} md={4}>
              <Box sx={{display: 'flex',flexDirection: 'column', alignItems: 'center'}}>
                <PeopleIcon/>
                <Typography>12 Employees</Typography>
              </Box>
            </Grid>
            <Grid item xs={6} md={4}>
              <Box sx={{display: 'flex',flexDirection: 'column', alignItems: 'center'}}>
                <PeopleIcon/>
                <Typography>8 Projects</Typography>
              </Box>
            </Grid>
            <Grid item xs={6} md={4}>
              <Box sx={{display: 'flex',flexDirection: 'column', alignItems: 'center'}}>
                <AssignmentIcon />
                <Typography>12 Templates</Typography>
              </Box>
            </Grid>
          </Grid>
        </Card>
      </Grid>

*/