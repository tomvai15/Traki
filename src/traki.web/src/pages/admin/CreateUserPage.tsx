import React, { useState } from 'react';
import { Breadcrumbs, Button, Card, CardContent, CardHeader, Divider, FormControl, Grid, InputLabel, MenuItem, Select, TextField, Typography } from '@mui/material';
import { userService } from 'services';
import { User } from 'contracts/user/User';
import { Role } from 'contracts/user/Roles';
import { UserStatus } from 'contracts/user/UserStatus';
import { CreateUserRequest } from 'contracts/user/CreateUserRequest';
import { AuthorisationCodeRequest } from 'contracts/auth/AuthorisationCodeRequest';
import authService from 'services/auth-service';
import { useLocation } from 'react-router-dom';
import InputIcon from '@mui/icons-material/Input';
import { useRecoilState } from 'recoil';
import { userState } from 'state/user-state';
import { validate, validationRules } from 'utils/textValidation';
import { useAlert } from 'hooks/useAlert';

const initialUser: User = {
  id: 0,
  name: '',
  surname: '',
  email: '',
  role: Role[Role.ProjectManager],
  status: UserStatus.created,
};

export function CreateUserPage() {
  const [userInfo] = useRecoilState(userState);
  const location = useLocation();
  const [user, setUser] = useState<User>(initialUser);

  const { displaySuccess,  displayError } = useAlert();

  async function createUser() {
    const request: CreateUserRequest = {
      user: {...user, role: user.role}
    };

    try {
      await userService.createUser(request);
      displaySuccess('User was created');
    } catch {
      displayError('User with same email already exists');
    }
  }

  async function getCodeUrl() {
    const request: AuthorisationCodeRequest = {
      state: btoa(location.pathname),
      loginAsAdmin: true
    };
    const res = await authService.getAuthorisationCodeUrl(request);
    window.location.replace(res);
  }

  function canSubmit () {
    return user.email.length != 0 && user.name.length != 0 && user.surname.length != 0 && isValid();
  }

  function isValid () {
    return !validate(user.email, [validationRules.email]).invalid && 
          !validate(user.surname, [validationRules.onlyLetters]).invalid && 
          !validate(user.name, [validationRules.onlyLetters]).invalid;
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
                  error={validate(user.email, [validationRules.email]).invalid}
                  helperText={validate(user.email, [validationRules.email]).message}
                  onChange={(e) => setUser({...user, email: e.target.value})}/>
              </Grid>
              <Grid item xs={6} md={6}>
                <TextField 
                  label='Name'
                  sx={{width: '100%'}}
                  value={user.name}
                  error={validate(user.name, [validationRules.onlyLetters]).invalid}
                  helperText={validate(user.name, [validationRules.onlyLetters]).message}
                  onChange={(e) => setUser({...user, name: e.target.value})}/>
              </Grid>
              <Grid item xs={6} md={6}>
                <TextField 
                  label='Surname'
                  sx={{width: '100%'}}
                  value={user.surname}
                  error={validate(user.surname, [validationRules.onlyLetters]).invalid}
                  helperText={validate(user.surname, [validationRules.onlyLetters]).message}
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
                <Button disabled={!canSubmit()} variant='contained' onClick={createUser}>Create User</Button>
              </Grid>
            </Grid>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
}