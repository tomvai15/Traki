import React, { useEffect } from 'react';
import {useLocation} from 'react-router-dom';
import { useUserInformation } from 'hooks/useUserInformation';

type Props = {
  children?: React.ReactNode
};

export const ProtectedRoute: React.FC<Props> = ({children}) => {

  const location = useLocation();
  const { fetchUser, fetchFullUserInformation } = useUserInformation();

  useEffect(() => {
    fetchUser();
    fetchFullUserInformation();
  }, [location]);

  useEffect(() => {
    fetchFullUserInformation();
  }, []);
  
  return <>{children}</>;
};
