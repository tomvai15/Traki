import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Button, Card, CardActions, CardContent, CardMedia, Grid, Paper, TextField, Typography, styled } from '@mui/material';
import companyService from '../../services/company-service';
import { Company } from '../../contracts/company/Company';
import { UpdateCompanyRequest } from '../../contracts/company/UpdateCompanyRequest';
import pictureService from '../../services/picture-service';
import FileUploadIcon from '@mui/icons-material/FileUpload';
import PeopleIcon from '@mui/icons-material/People';
import AssignmentIcon from '@mui/icons-material/Assignment';
import { useNavigate } from 'react-router-dom';

// TODO: allow only specific resolution

export function Templates() {

  const navigate = useNavigate();

  useEffect(() => {
    fetchCompany();
  }, []);

  function fetchCompany() {
    console.log('asd');
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Card title='Sample Project' onClick={() => navigate('/templates/1')}>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant="h5">Template Name</Typography>
            <Typography variant="h6" fontSize={15} >Checklist</Typography>
          </CardContent>    
        </Card>
      </Grid>
      <Grid item xs={12} md={12} >
        <Card title='Sample Project'>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant="h5">Template Name</Typography>
            <Typography variant="h6" fontSize={15} >Test Report</Typography>
          </CardContent>    
        </Card>
      </Grid>
    </Grid>
  );
}