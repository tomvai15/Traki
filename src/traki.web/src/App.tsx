import React from 'react';
import { Routes, Route, Outlet, BrowserRouter, Navigate } from 'react-router-dom';
import './App.css';
import Dashboard from './pages/Dashboard';
import ThemeProvider from '@mui/material/styles/ThemeProvider';
import createTheme from '@mui/material/styles/createTheme';
import SignIn from './pages/Authentication/SignIn';
import { RecoilRoot, atom, useRecoilState } from 'recoil';
import { recoilPersist } from 'recoil-persist';
import { Card } from '@mui/material';
import ProtectedRoute from './components/ProtectedRoute';
import { DrawerAndHeader } from './layout/DrawerAndHeader';

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

const { persistAtom } = recoilPersist();

const counterState = atom({
  key: 'count',
  default: 0,
  effects_UNSTABLE: [persistAtom],
});

function App() {
  return (
    <ThemeProvider theme={mdTheme}>
      <RecoilRoot>
        <BrowserRouter>
          <Routes>
            <Route path='' element={<ProtectedRoute/>}>
              <Route path='' element={<DrawerAndHeader/>}>
                <Route index element={<Dashboard/>}/>
              </Route>
            </Route>
            <Route path='/login' element={<SignIn/>}/>
            <Route path='*' element={<Navigate to='/login'/>} />
          </Routes>
        </BrowserRouter>
      </RecoilRoot>
    </ThemeProvider>
  );
}

export default App;
