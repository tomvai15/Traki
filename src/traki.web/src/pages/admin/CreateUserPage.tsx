import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Breadcrumbs, Button, Card, CardContent, CardHeader, Divider, FormControl, Grid, InputLabel, MenuItem, Select, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography } from '@mui/material';
import { userService } from 'services';
import { User } from 'contracts/user/User';
import { Role } from 'contracts/user/Roles';
import { UserStatus } from 'contracts/user/UserStatus';
import { CreateUserRequest } from 'contracts/user/CreateUserRequest';

const initialUser: User = {
  id: 0,
  name: '',
  surname: '',
  email: '',
  role: Role[Role.ProjectManager],
  status: UserStatus.created,
};

export function CreateUserPage() {
  const [user, setUser] = useState<User>(initialUser);
  const [message, setMessage] = useState<string>();

  async function createUser() {
    const request: CreateUserRequest = {
      user: {...user, role: user.role}
    };
    await userService.createUser(request);
    setMessage('User created');
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
          <CardHeader title="User Information"/>
          <Divider/>
          <CardContent>
            <Grid container spacing={2}>
              <Grid item xs={12} md={12}>
                <TextField 
                  sx={{width: '100%'}}
                  label='Email'
                  value={user.email}
                  onChange={(e) => setUser({...user, email: e.target.value})}/>
              </Grid>
              <Grid item xs={6} md={6}>
                <TextField 
                  label='Name'
                  sx={{width: '100%'}}
                  value={user.name}
                  onChange={(e) => setUser({...user, name: e.target.value})}/>
              </Grid>
              <Grid item xs={6} md={6}>
                <TextField 
                  label='Surname'
                  sx={{width: '100%'}}
                  value={user.surname}
                  onChange={(e) => setUser({...user, surname: e.target.value})}/>
              </Grid>
              <Grid item xs={12} md={12}>
                <FormControl fullWidth>
                  <InputLabel id="demo-simple-select-label">Role</InputLabel>
                  <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    value={user.role}
                    label="Age"
                    onChange={(e) => setUser({...user, role: e.target.value as string})}
                  >
                    <MenuItem value={Role[Role.Administrator]}>{Role[Role.Administrator]}</MenuItem>
                    <MenuItem value={Role[Role.ProjectManager]}>{Role[Role.ProjectManager]}</MenuItem>
                    <MenuItem value={Role[Role.ProductManager]}>{Role[Role.ProductManager]}</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
              <Grid item xs={12} md={12}>
                <Divider></Divider>
              </Grid>
              <Grid item xs={12} md={12}>
                <Button onClick={createUser}>Create User</Button>
              </Grid>
            </Grid>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
}