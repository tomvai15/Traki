import React from 'react';
import { UserInfo } from '../contracts/auth/UserInfo';
import { Author } from '../contracts/drawing/defect/Author';
import { Avatar, Box } from '@mui/material';

type Props = {
  user?: UserInfo | Author
  size: number
}

export function CustomAvatar ({user, size}: Props) { 

  if (!user) {
    return (
      <Avatar sx={{ width: size, height: size }} />
    );
  }

  return (
    <Box>
      { user.userIconBase64 != undefined  && user.userIconBase64 != '' && user.userIconBase64 != null ? 
        <Avatar sx={{ width: size, height: size }}  src={user.userIconBase64}/> :
        <Avatar sx={{ width: size, height: size }} src={user.userIconBase64}>
          {user.name.toUpperCase()[0] + '' + user.surname.toUpperCase()[0]}
        </Avatar>}
    </Box>
  );
}
