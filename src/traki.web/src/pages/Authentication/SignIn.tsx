import React, { useState } from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LoginIcon from '@mui/icons-material/Login';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import authService from '../../services/auth-service';
import { LoginRequest } from '../../contracts/auth/LoginRequest';
import { Axios, AxiosError } from 'axios';
import { Card } from '@mui/material';

export default function SignIn() {
  /*
	const location = useLocation();
	const dispatch = useAppDispatch();
	const navigate = useNavigate();*/

  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [error, setError] = useState<string>('');

  async function handleSubmit () {
    const loginUserRequest: LoginRequest = {
      email: email,
      password: password,
    };
    try {
      const response = await authService.login(loginUserRequest);

      if (response.status == 200) {
        setError('mldc');
      } else {
        setError('El.paštas arba slaptažodis yra netesingas');
      }
    } catch (err) {
      setError('Svetainė šiuo metu nepasiekiama');
    }

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
          Prisijungimas
        </Typography>
        <Box sx={{ mt: 1 }}>
          <TextField onChange={(e) => setEmail(e.target.value)}
            margin="normal"
            required
            fullWidth
            id="email"
            label="El. Paštas"
            name="email"
            autoComplete="email"
            autoFocus
          />
          <TextField onChange={(e) => setPassword(e.target.value)}
            margin="normal"
            required
            fullWidth
            name="password"
            label="Slaptažodis"
            type="password"
            id="password"
            autoComplete="current-password"
          />
          <Typography fontSize={20} color={'red'}>
            {error}
          </Typography>
          <Button onClick={handleSubmit}
            disabled={!(email && password)}
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Prisijungti
          </Button>

          <Button onClick={ async ()=> console.log(await authService.getUserInfo())}
            disabled={!(email && password)}
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            TEst
          </Button>
          <Grid container>
            <Grid item xs>
            </Grid>
            <Grid item>
              <Link href="/sign-up" variant="body2">
                {'Neturi paskyros? Prisiregistruok čia'}
              </Link>
            </Grid>
          </Grid>
        </Box>
      </Card>
    </Box>
  );
}