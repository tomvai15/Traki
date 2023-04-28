import { UserInfo } from 'contracts/auth/UserInfo';
import React from 'react';
import { useRecoilState } from "recoil";
import { userState } from "state/user-state";

type Props = {
  children?: React.ReactNode,
  checkIfRender: (user: UserInfo) =>  boolean
};

export const HideableComponent: React.FC<Props> = ({children, checkIfRender}) => {

  const [userInfo] = useRecoilState(userState);
  
  return  userInfo.user && checkIfRender(userInfo.user)  ? 
    <>{children}</> : <></>;
};