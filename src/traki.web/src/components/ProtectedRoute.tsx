import React, { useEffect } from 'react';
import {Navigate, useLocation} from 'react-router-dom';
import { useRecoilState } from 'recoil';
import { userState } from '../state/user-state';
import { useUserInformation } from 'hooks/useUserInformation';

type Props = {
  children?: React.ReactNode
};

export const ProtectedRoute: React.FC<Props> = ({children}) => {

  const location = useLocation();
  const { fetchUser, fetchFullUserInformation } = useUserInformation();

  const [userInfo] = useRecoilState(userState);

  useEffect(() => {
    fetchUser();
  //  fetchFullUserInformation();
  }, [location]);

  useEffect(() => {
    fetchFullUserInformation();
    console.log('???--++++');
  }, [userInfo]);
  
  return userInfo && userInfo.id >= 0 ? <>{children}</> : <Navigate to="/login"/>;
};
