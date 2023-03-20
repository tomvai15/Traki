import React, { useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import ProtectedRoute from '../components/ProtectedRoute';
import { DrawerAndHeader } from '../layout/DrawerAndHeader';
import SignIn from './Authentication/SignIn';
import Dashboard from './Dashboard';
import { useRecoilState } from 'recoil';
import { userState } from '../state/user-state';
import authService from '../services/auth-service';
import CheckOAuth from './Authentication/CheckOAuth';

export function Main() {

  const [userInfo, setUserInfo] = useRecoilState(userState);

  useEffect(() => {
    fetchUser();
  }, []);

  async function fetchUser() {
    try {
      const getUserResponse = await authService.getUserInfo();
      setUserInfo({ id: getUserResponse.user.id });
      console.log('ok');
    } catch {
      setUserInfo({ id: -1 });
      console.log('ne ok');
    }
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route path='' element={<ProtectedRoute/>}>
          <Route path='' element={<DrawerAndHeader/>}>
            <Route index element={<Dashboard/>}/>
            <Route path='checkoauth' element={<CheckOAuth/>}/>
          </Route>
        </Route>
        <Route path='/login' element={<SignIn/>}/>
        <Route path='*' element={<Navigate to='/'/>} />
      </Routes>
    </BrowserRouter>
  );
}
