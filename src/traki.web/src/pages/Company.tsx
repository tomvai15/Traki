import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Button, Card, CardActions, CardContent, CardMedia, Grid, Paper, TextField, Typography, styled } from '@mui/material';
import companyService from '../services/company-service';
import { Company } from '../contracts/company/Company';
import { UpdateCompanyRequest } from '../contracts/company/UpdateCompanyRequest';
import pictureService from '../services/picture-service';

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  textAlign: 'center',
}));

export function CompanyPage() {

  const [company, setCompany] = useState<Company>();
  const [canEdit, setCanEdit] = useState<boolean>(false);

  const [image, setImage] = useState<string>('');
  const [name, setName] = useState<string>('');
  const [address, setAddress] = useState<string>('');
  const [phone, setPhone] = useState<string>('');

  useEffect(() => {
    fetchCompany();
  }, []);

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

  return (
    <Box component="main" sx={{
      flexGrow: 1,
      height: '100vh',
      display: 'flex', 
      flexDirection: 'column'
    }}>
      <Box sx={{height: 60,  backgroundColor: 'red'}}/>
      <Box sx={{flex: 1, padding: 5,  display: 'flex', backgroundColor: (theme) => theme.palette.grey[100], flexDirection: 'column'}}>
        
        <Grid container spacing={2}>
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
                <TextField
                  id="standard-read-only-input"
                  label="Phone number"
                  defaultValue='...'
                  value={phone}
                  onChange={(e) => setPhone(e.target.value)}
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
            <Card title='Sample Project' sx={{height: "100%"}}>
              <CardContent sx={{ height: "100%"}}>
                { image.length == 0 ? 
                  <Box sx={{backgroundColor: '#ededed', height: "100%"}} ></Box> :
                  <CardMedia
                    height='200'
                    component="img"
                    sx={{
                      16:9
                    }}
                    image={image}
                    alt="random"
                  />}
              </CardContent>      
            </Card>
          </Grid>
          <Grid item xs={12} md={12}>
            <Card sx={{padding: 5}}>
              <Grid container spacing={2}>
                <Grid item xs={6} md={4}>
                  <Item>xs=6 md=4</Item>
                </Grid>
                <Grid item xs={6} md={4}>
                  <Item>xs=6 md=4</Item>
                </Grid>
              </Grid>
            </Card>
          </Grid>
        </Grid>
      </Box>
    </Box>
  );
}