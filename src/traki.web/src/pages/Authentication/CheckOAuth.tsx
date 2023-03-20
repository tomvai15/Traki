/* eslint-disable */
import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import { CircularProgress, Container, LinearProgress } from '@mui/material';
import { useLocation, useParams } from 'react-router-dom';
import authService from '../../services/auth-service';
import { LoginOAuthRequest } from '../../contracts/auth/LoginOAuthRequest';

function useQuery() {
  const { search } = useLocation();

  return React.useMemo(() => new URLSearchParams(search), [search]);
}

export default function CheckOAuth() {

  const [loadinig, setLoading] = useState(true);
  let query = useQuery();

  useEffect(() => {
    console.log(query.get('code'));
    checkAuthCode();
  }, []);

  async function checkAuthCode() {

    const code = query.get('code');
    const state = query.get('state');

    const docusignRequest: LoginOAuthRequest = {
      code: code ?? '',
      state: state ?? ''
    }
    setLoading(true);
    await authService.loginDocusign(docusignRequest);
    setLoading(false);
  }

  return (
    <Box component="main" sx={{
      flexGrow: 1,
      height: '100vh',
      marginTop: 8
    }}>
      <Container sx={{display: 'flex', alignItems: 'center', marginTop: '30vh', flexDirection: 'column'}} >
        <Box sx={{flex: 1}}>
          <Typography>Handling oauth</Typography>
        </Box>
        { loadinig &&
        <Box sx={{flex: 1}}>
          <CircularProgress />
        </Box>}
      </Container>
    </Box>
  );
}