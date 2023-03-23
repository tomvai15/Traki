import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Card, CardContent, Grid, Paper, TextField, Typography, styled } from '@mui/material';
import companyService from '../services/company-service';
import { Company } from '../contracts/company/Company';

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  textAlign: 'center',
}));

export function CompanyPage() {

  const [company, setCompany] = useState<Company>();

  useEffect(() => {
    fetchCompany();
  }, []);

  async function fetchCompany() {
    const getCompanyResponse = await companyService.getCompany();
    setCompany(getCompanyResponse.company);
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
          <Grid item xs={6} md={4}>
            <Card title='Sample Project'>
              <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
                <TextField size='medium'
                  id="standard-read-only-input"
                  label="Company name"
                  defaultValue='...'
                  value={company?.name}
                  InputProps={{
                    readOnly: true,
                  }}
                  variant="standard"
                />
                <TextField
                  id="standard-read-only-input"
                  label="Address"
                  defaultValue='...'
                  value={company?.address}
                  InputProps={{
                    readOnly: true,
                  }}
                  variant="standard"
                />
                <TextField
                  id="standard-read-only-input"
                  label="Phone number"
                  defaultValue='...'
                  value='+3701245655'
                  InputProps={{
                    readOnly: true,
                  }}
                  variant="standard"
                />
              </CardContent>      
            </Card>
          </Grid>
          <Grid item xs={6} md={8}>
            <Card title='Sample Project'>
              <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
                <TextField size='medium'
                  id="standard-read-only-input"
                  label="Company name"
                  defaultValue='...'
                  value={company?.name}
                  InputProps={{
                    readOnly: true,
                  }}
                  variant="standard"
                />
                <TextField
                  id="standard-read-only-input"
                  label="Address"
                  defaultValue='...'
                  value={company?.address}
                  InputProps={{
                    readOnly: true,
                  }}
                  variant="standard"
                />
                <TextField
                  id="standard-read-only-input"
                  label="Phone number"
                  defaultValue='...'
                  value='+3701245655'
                  InputProps={{
                    readOnly: true,
                  }}
                  variant="standard"
                />
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