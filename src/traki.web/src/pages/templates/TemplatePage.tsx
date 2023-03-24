import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Button, Card, CardActions, CardContent, CardMedia, Divider, Grid, Paper, TextField, Typography, styled } from '@mui/material';
import companyService from '../../services/company-service';
import { Company } from '../../contracts/company/Company';
import { UpdateCompanyRequest } from '../../contracts/company/UpdateCompanyRequest';
import pictureService from '../../services/picture-service';
import FileUploadIcon from '@mui/icons-material/FileUpload';
import PeopleIcon from '@mui/icons-material/People';
import AssignmentIcon from '@mui/icons-material/Assignment';

// TODO: allow only specific resolution

export function TemplatePage() {
  const [previewImage, setPreviewImage] = useState<string>();
  const [company, setCompany] = useState<Company>();
  const [canEdit, setCanEdit] = useState<boolean>(false);

  const [file, setFile] = useState<File>();
  const [image, setImage] = useState<string>('');
  const [name, setName] = useState<string>('');
  const [address, setAddress] = useState<string>('');
  const [phone, setPhone] = useState<string>('');

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
      <Grid item xs={12} md={12} >
        <Card title='Sample Project'>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant="h5">Template Name</Typography>
            <Typography variant="h6" fontSize={15} >Checklist</Typography>
          </CardContent>  
          <Divider/>  
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant="h6" fontSize={15} >Checkpoints</Typography>
            <Card title='Sample Project' sx={{marginBottom: 1}}>
              <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
                <Typography variant="h5">1 Checkpoint Name</Typography>
                <Divider/>  
                <Card title='Sample Project'>
                  <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
                    <Typography variant="h5">1 Question Name</Typography>
                  </CardContent> 
                  <Divider/> 
                  <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
                    <Typography variant="h5">2 Question Name</Typography>
                  </CardContent>  
                </Card>
              </CardContent>  
            </Card>
            <Card title='Sample Project'>
              <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
                <Typography variant="h5">2 Checkpoint Name</Typography>
              </CardContent>  
            </Card>
          </CardContent>  
        </Card>
      </Grid>
    </Grid>
  );
}