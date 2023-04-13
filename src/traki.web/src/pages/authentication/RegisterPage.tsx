import React, { useEffect, useState } from 'react';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LoginIcon from '@mui/icons-material/Login';
import Typography from '@mui/material/Typography';
import authService from '../../services/auth-service';
import { LoginRequest } from '../../contracts/auth/LoginRequest';
import { Card } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useRecoilState } from 'recoil';
import { userState } from '../../state/user-state';
import { useQuery } from 'features/auth/hooks/useQuery';
import { ActivateAccountRequest } from 'contracts/auth/ActivateAccountRequest';
import { userService } from 'services';
import { User } from 'contracts/user/User';

export default function RegisterPage() {
  const navigate = useNavigate();
  const query = useQuery();

  const [user, setUser] = useState<User>();

  const [password, setPassword] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');

  const [code, setCode] = useState<string>();
  const [accountId, setAccountId] = useState<string>();

  useEffect(() => {
    setCode(query.get('code') ?? '');
    
    const registerId = query.get('acc') ?? '';
    fetchUser(registerId);
    setAccountId(query.get('acc') ?? '');
  }, []);

  async function fetchUser (registerId: string) {
    const response = await userService.getUserForActivation(registerId);
    setUser(response.user);
  }

  async function handleSubmit () {
    const request: ActivateAccountRequest = {
      registerId: accountId ?? '',
      code: code ?? '',
      password: password
    };

    await authService.activate(request);

    navigate('/login');
  }

  function canSubmit(): boolean {
    return password != '' && password == confirmPassword;
  }

  return (
    <Box component="main" style={{height: '100vh', width: '100vw', backgroundImage: `url("/img/image.svg")`, paddingTop: 100 }}>
      <CssBaseline />
      <Card
        sx={{
          width: 600,
          marginLeft: 'auto',
          marginRight: 'auto',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          p: 10
        }}
      >
        <Typography component="h1" variant="h5">
          Activate account
        </Typography>
        <Typography>
          {user?.email}
        </Typography>
        <Box sx={{ mt: 1 }}>
          <TextField onChange={(e) => setPassword(e.target.value)}
            value={password}
            margin="normal"
            required
            fullWidth
            label="Password"
            autoComplete="email"
            autoFocus
          />
          <TextField onChange={(e) => setConfirmPassword(e.target.value)}
            value={confirmPassword}
            margin="normal"
            required
            fullWidth
            name="password"
            label="Confirm Password"
            type="password"
            id="password"
            autoComplete="current-password"
          />
          <Button onClick={handleSubmit}
            disabled={!canSubmit()}
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Submit
          </Button>
        </Box>
      </Card>
    </Box>
  );
}