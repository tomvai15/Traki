import React from 'react';
import { useRecoilState } from "recoil";
import { userState } from "state/user-state";

type Props = {
  children?: React.ReactNode,
  role: string[] | string
};

export const ProtectedComponent: React.FC<Props> = ({children, role}) => {

  const [userInfo] = useRecoilState(userState);
  
  return userInfo && userInfo.role &&  ( Array.isArray(role) ? role.includes(userInfo.role) : userInfo.role == role )  ? 
    <>{children}</> : <></>;
};