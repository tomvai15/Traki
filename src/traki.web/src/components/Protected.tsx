import React, { useEffect } from 'react';
import {Navigate, useLocation} from 'react-router-dom';
import { useUserInformation } from 'hooks/useUserInformation';
import { useRecoilState } from 'recoil';
import { userState } from 'state/user-state';
import { ro } from 'date-fns/locale';
import { Box, Typography } from '@mui/material';

type Props = {
  roles: string[],
  children?: React.ReactNode
};

export const Protected: React.FC<Props> = ({children, roles}) => {

  const [userInfo] = useRecoilState(userState);

  console.log(userInfo);

  if (userInfo.id < 0) {
    return <Navigate to={'/login'}/>;
  }

  if (!userInfo.role || !roles.includes(userInfo.role)) {
    return (
      <Box sx={{display: 'flex', flexDirection: 'row', justifyContent: 'center', height: '80vh', alignItems: 'center'}}>
        <Typography id="not-found" style={{fontSize: 30}}>Unauthorized</Typography>
      </Box>
    );
  }

  return  <>{children}</>;
};
