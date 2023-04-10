import React from 'react';
import './utils/string.extensions';
import './App.css';
import ThemeProvider from '@mui/material/styles/ThemeProvider';
import { RecoilRoot } from 'recoil';
import { Main } from './pages/Main';
import theme from 'themes/theme';

const mdTheme = theme();

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
