import React from 'react';
import {Navigate, Outlet} from 'react-router-dom';
import { useRecoilState } from 'recoil';
import { userState } from '../state/user-state';

const ProtectedRoute = () => {
  const [userInfo] = useRecoilState(userState);
  
  return userInfo.id >= 0 ? <Outlet/> : <Navigate to="/login"/>;
};

export default ProtectedRoute;