/* eslint-disable */
import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import { CircularProgress, Container } from '@mui/material';
import { useLocation, useNavigate } from 'react-router-dom';
import { useRecoilState } from 'recoil';
import { userState } from '../../state/user-state';
import reportService from '../../services/report-service';
import { useQuery } from 'features/auth/hooks/useQuery';

export function SignValidation() {

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