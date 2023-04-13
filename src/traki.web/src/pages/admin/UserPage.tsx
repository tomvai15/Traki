import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Breadcrumbs, Button, Card, CardContent, Divider, Grid, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography } from '@mui/material';
import { userService } from 'services';
import { User } from 'contracts/user/User';
import { useParams } from 'react-router-dom';
import { UpdateUserStatusRequest } from 'contracts/user/UpdateUserStatusRequest';
import { UserStatus } from 'contracts/user/UserStatus';

export function UserPage() {
  const { userId } = useParams();
  const [user, setUser] = useState<User>();

  useEffect(()=> {
    fetchUsers();
  }, []);
 
  async function fetchUsers() {
    const response = await userService.getUser(Number(userId));
    setUser(response.user);
  }

  async function updateStatus(status: string) {
    const request: UpdateUserStatusRequest = {
      status: status
    };

    await userService.updateUserStatus(Number(userId), request);
    fetchUsers();
  }

  if (!user) {
    return (
      <Grid container spacing={2}>
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb">
            <Typography color="text.primary">Users</Typography>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={12} md={12}>
          <Card>
            <CardContent>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    );
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <Typography color="text.primary">Users</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={6} md={6}>
        <Card>
          <CardContent>
            <Grid container spacing={2}>
              <Grid item xs={12} md={12}>
                <Typography>User Information</Typography>
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
              <Grid item xs={12} md={12}>
                <Divider></Divider>
              </Grid>
              <Grid item xs={12} md={12}>
                <TextField 
                  focused
                  label='Status'
                  color={user.status == UserStatus.active ? 'success' : 'error' }
                  value={user.status}
                  InputProps={{
                    readOnly: true
                  }}/>
              </Grid>
              <Grid item xs={6} md={6}>
                { user.status == UserStatus.active && 
                  <Button onClick={() => updateStatus(UserStatus.blocked)} color='error'>Block User</Button>}
                { user.status == UserStatus.blocked && 
                  <Button onClick={() => updateStatus(UserStatus.active)} color='success'>Unblock User</Button>}
              </Grid>
            </Grid>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
}