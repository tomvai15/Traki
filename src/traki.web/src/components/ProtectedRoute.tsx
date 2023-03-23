import React from 'react';
import {Navigate} from 'react-router-dom';
import { useRecoilState } from 'recoil';
import { userState } from '../state/user-state';

type Props = {
  children?: React.ReactNode
};

export const ProtectedRoute: React.FC<Props> = ({children}) => {
  const [userInfo] = useRecoilState(userState);
  
  return userInfo && userInfo.id >= 0 ? <>{children}</> : <Navigate to="/login"/>;
};
