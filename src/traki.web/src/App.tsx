import React from 'react';
import './App.css';
import ThemeProvider from '@mui/material/styles/ThemeProvider';
import createTheme from '@mui/material/styles/createTheme';
import { RecoilRoot } from 'recoil';
import { Main } from './pages/Main';

const mdTheme = createTheme({ 
  palette: {
    primary: {
      main: '#e4ae3f',
    },
    secondary: {
      main: '#9ab1c0',
    }
  }
});

function App() {
  return (
    <ThemeProvider theme={mdTheme}>
      <RecoilRoot>
        <Main></Main>
      </RecoilRoot>
    </ThemeProvider>
  );
}

export default App;
