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

function useQuery() {
  const { search } = useLocation();
  return React.useMemo(() => new URLSearchParams(search), [search]);
}

export default function CheckOAuth() {

  const navigate = useNavigate();
  const [loadinig, setLoading] = useState(true);
  const [userInfo, setUserInfo] = useRecoilState(userState);
  let query = useQuery();

  useEffect(() => {
    checkAuthCode();
  }, []);

  async function checkAuthCode() {

    const code = query.get('code');
    const state = query.get('state');

    const docusignRequest: LoginOAuthRequest = {
      code: code ?? '',
      state: state ?? ''
    }
    const path = atob(docusignRequest.state);

    setLoading(true);
    await authService.loginDocusign(docusignRequest);
    setUserInfo({ id: userInfo.id, loggedInDocuSign: true })
    setLoading(false);
    navigate(path);
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