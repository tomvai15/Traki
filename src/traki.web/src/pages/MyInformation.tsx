import React, { useEffect, useState } from 'react';
import { Box, Button, Card, CardContent, Divider, Grid, Stack, TextField, Typography } from '@mui/material';
import { userState } from 'state/user-state';
import { useRecoilState } from 'recoil';
import { UserInfo } from 'contracts/auth/UserInfo';
import Avatar from 'react-avatar-edit';
import authService from 'services/auth-service';
import { UpdateUserInfoRequest } from 'contracts/auth/UpdateUserInfoRequest';
import { useUserInformation } from 'hooks/useUserInformation';
import { avatarImage } from 'features/auth/data/avatarImage';

const profileWidth = '200px';

export function MyInformation() {
  const { fetchFullUserInformation } = useUserInformation();

  const [editingAvatar, setEditingAvatar] = useState<boolean>(false);
  const [preview, setPreview] = useState<string>('');
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

  async function updateProfileImage(){
    if (!user) {
      return;
    }
    const request: UpdateUserInfoRequest = {
      user: {...user, userIconBase64: preview}
    };

    await authService.updateUserInfo(request);
    setUser(request.user);
    setEditingAvatar(false);
    await fetchFullUserInformation();
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
                { !editingAvatar ?
                  <Box component={Button} onClick={() => setEditingAvatar(true)} sx={{borderRadius: '50%', position: 'relative', height: profileWidth, width: profileWidth}}>
                    <img  width={profileWidth} style={{display: 'block', borderRadius: '50%'}} src={user.userIconBase64 != undefined ? user.userIconBase64 : avatarImage}></img>
                    <Box sx={styles.overlay}>
                      <Stack sx={{height: '100%'}} direction={'column'} justifyContent={'center'} alignItems={'center'}>
                        <Typography variant='h4'>Edit Profile</Typography>
                      </Stack>
                    </Box>
                  </Box> :
                  <Box>
                    <Avatar
                      exportAsSquare
                      width={300}
                      onCrop={(e) => {setPreview(e);}}
                      height={300}
                      src={user.userIconBase64}
                    />
                    <Stack direction={'row'} spacing={1} sx={{marginTop: '10px', marginBottom: '10px'}}>
                      <Button onClick={() => setEditingAvatar(false)}>Cancel</Button>
                      <Button variant='contained' onClick={() => updateProfileImage()}>Update</Button>
                    </Stack>
                  </Box>}
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

const styles = {
  overlay: {
    borderRadius: '50%',
    top: 0,
    width: '100%',
    color: "#4E4E4E00",
    background: "#F1F1F100",
    position: 'absolute',
    bottom: 0,
    '&:hover': {
      background: "#4E4E4E72",
      color: 'white',
    }
  }
};