/* eslint-disable */
import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import { CircularProgress, Container } from '@mui/material';
import { useLocation, useNavigate } from 'react-router-dom';
import authService from '../../services/auth-service';
import { LoginOAuthRequest } from '../../contracts/auth/LoginOAuthRequest';
import { useRecoilState } from 'recoil';
import { userState } from '../../state/user-state';
import reportService from '../../services/report-service';
import path from 'path';

function useQuery() {
  const { search } = useLocation();
  return React.useMemo(() => new URLSearchParams(search), [search]);
}

export default function SignValidation() {

  const navigate = useNavigate();
  const [loadinig, setLoading] = useState(true);
  const [userInfo, setUserInfo] = useRecoilState(userState);
  let query = useQuery();

  useEffect(() => {
    validateSign();
  }, []);

  async function validateSign() {

    const event = query.get('event');
    const state = query.get('state');
    const decodedState = atob(state ?? '');

    var values = decodedState.split(':');
    
    const path = values[0];
    const protocolId = values[1];

    console.log(path);

    if (event == 'signing_complete') {
      setLoading(true);
      await reportService.validateDocumentSign(Number(protocolId));
      setLoading(false);
      navigate(path);
    } else {
      navigate(path);
    }
  }

  return (
    <Box component="main" sx={{
      flexGrow: 1,
      height: '100vh',
      marginTop: 8
    }}>
      <Container sx={{display: 'flex', alignItems: 'center', marginTop: '30vh', flexDirection: 'column'}} >
        <Box sx={{flex: 1}}>
          <Typography>Validating document signing</Typography>
        </Box>
        { loadinig &&
        <Box sx={{flex: 1}}>
          <CircularProgress />
        </Box>}
      </Container>
    </Box>
  );
}