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

export default function SignIn() {
  const [userInfo, setUserInfo] = useRecoilState(userState);
  const navigate = useNavigate();

  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [error, setError] = useState<string>('');


  useEffect(() => {
    const listener = (event: { code: string; preventDefault: () => void; }) => {
      if (event.code === "Enter" || event.code === "NumpadEnter") {
        console.log("Enter key was pressed. Run your function.");
        event.preventDefault();
        handleSubmit();
      }
    };
    document.addEventListener("keydown", listener);
    return () => {
      document.removeEventListener("keydown", listener);
    };
  }, [email, password]);

  async function handleSubmit () {
    if (canSubmit()) {
      return;
    }
    const loginUserRequest: LoginRequest = {
      email: email,
      password: password,
    };
    try {
      const response = await authService.login(loginUserRequest);
      if (response.status == 200) {
        loginUser();
      } else {
        setError('Email or password is incorrect');
      }
    } catch (err) {
      setError('Service is currently not available');
    }
  }

  function canSubmit() {
    return !(email && password);
  }

  function loginUser() {
    setUserInfo({id: 1, loggedInDocuSign: false});
    navigate('/');
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
        <LoginIcon fontSize='large'/>
        <Typography component="h1" variant="h5">
          Sign in
        </Typography>
        <Box sx={{ mt: 1 }}>
          <TextField onChange={(e) => setEmail(e.target.value)}
            margin="normal"
            required
            fullWidth
            id="email"
            label="Email"
            name="email"
            autoComplete="email"
            autoFocus
          />
          <TextField onChange={(e) => setPassword(e.target.value)}
            margin="normal"
            required
            fullWidth
            name="password"
            label="Password"
            type="password"
            id="password"
            autoComplete="current-password"
          />
          <Typography id={'error'} fontSize={20} color={'red'}>
            {error}
          </Typography>
          <Button onClick={handleSubmit}
            id='submit'
            disabled={canSubmit()}
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Login
          </Button>
          <Grid container>
            <Grid item xs>
            </Grid>
            <Grid item>
              <Link href="/sign-up" variant="body2">
                {'Forgot password?'}
              </Link>
            </Grid>
          </Grid>
        </Box>
      </Card>
    </Box>
  );
}