import React, { useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { DrawerAndHeader } from '../layout/DrawerAndHeader';
import SignIn from './Authentication/SignIn';
import Dashboard from './Dashboard';
import { useRecoilState } from 'recoil';
import { userState } from '../state/user-state';
import authService from '../services/auth-service';
import CheckOAuth from './Authentication/CheckOAuth';
import { Projects } from './projects/Projects';
import { ProtectedRoute } from '../components/ProtectedRoute';

export function Main() {

  const [userInfo, setUserInfo] = useRecoilState(userState);

  useEffect(() => {
    fetchUser();
  }, []);

  async function fetchUser() {
    try {
      const getUserResponse = await authService.getUserInfo();
      setUserInfo({ id: getUserResponse.user.id, loggedInDocuSign: getUserResponse.loggedInDocuSign });
      console.log('ok');
    } catch {
      setUserInfo({ id: -1, loggedInDocuSign: false });
      console.log('ne ok');
    }
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route path='' element={<ProtectedRoute><DrawerAndHeader/></ProtectedRoute>}>
          <Route index element={<Navigate to='/projects'/>}/>
          <Route path='projects' element={<Projects/>}/>
          <Route path='report' element={<Dashboard/>}/>
          <Route path='checkoauth' element={<CheckOAuth/>}/>
        </Route>
        <Route path='/login' element={<SignIn/>}/>
        <Route path='*' element={<Navigate to='/'/>} />
      </Routes>
    </BrowserRouter>
  );
}
