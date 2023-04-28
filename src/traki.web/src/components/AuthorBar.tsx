import React from 'react';
import { UserInfo } from '../contracts/auth/UserInfo';
import { Author } from '../contracts/drawing/defect/Author';
import { Avatar, Box, Stack, Typography } from '@mui/material';
import { CustomAvatar } from './CustomAvatar';

type Props = {
  user?: UserInfo | Author
  variant?: 'short' | 'long'
}

export function AuthorBar ({user, variant}: Props) { 

  return (
    <Stack direction={'row'} spacing={1} alignItems={'center'} sx={{marginRight: '10px'}}>
      <Box>
        <CustomAvatar size={35} user={user}/>
      </Box>
      <Box>
        {variant == 'short' ?       
          <Typography>
            {user?.name.toUpperCase()[0]}{user?.surname.toUpperCase()[0]}
          </Typography> :
          <Typography>
            {user?.name} {user?.surname}
          </Typography>}
      </Box>
    </Stack>
  );
}
