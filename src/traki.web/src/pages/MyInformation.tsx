import React, { useEffect, useState } from 'react';
import { Card, CardContent, Divider, Grid, TextField, Typography } from '@mui/material';
import { userState } from 'state/user-state';
import { useRecoilState } from 'recoil';
import { UserInfo } from 'contracts/auth/UserInfo';

export function MyInformation() {

  const [userInfo] = useRecoilState(userState);

  const [user, setUser] = useState<UserInfo>();

  useEffect(() => {
    setUser(userInfo.user);
  }, [userInfo]);

  if (!user) {
    return (
      <Grid container spacing={2}>
        <Grid item xs={6} md={6}>
          <Card>
            <CardContent>
              <Grid container spacing={2}>
                <Grid item xs={12} md={12}>
                  <Typography>My Information</Typography>
                </Grid>
              </Grid> 
            </CardContent>
          </Card>
        </Grid>
      </Grid>);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={6} md={6}>
        <Card>
          <CardContent>
            <Grid container spacing={2}>
              <Grid item xs={12} md={12}>
                <Typography>My Information</Typography>
              </Grid>
              <Grid item xs={12} md={12}>
                <Divider></Divider>
              </Grid>
              <Grid item xs={12} md={12}>
                <TextField 
                  sx={{width: '100%'}}
                  label='Email'
                  value={user.email}
                  InputProps={{
                    readOnly: true
                  }}/>
              </Grid>
              <Grid item xs={6} md={6}>
                <TextField 
                  label='Name'
                  sx={{width: '100%'}}
                  value={user.name} 
                  InputProps={{
                    readOnly: true
                  }}/>
              </Grid>
              <Grid item xs={6} md={6}>
                <TextField 
                  label='Surname'
                  sx={{width: '100%'}}
                  value={user.surname}
                  InputProps={{
                    readOnly: true
                  }}/>
              </Grid>
              <Grid item xs={12} md={12}>
                <TextField 
                  label='Role'
                  value={user.role} 
                  InputProps={{
                    readOnly: true
                  }}/>
              </Grid>
            </Grid>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
}