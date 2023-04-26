import React from 'react';
import { View } from 'react-native';
import { Avatar } from 'react-native-paper';
import { UserInfo } from '../contracts/auth/UserInfo';
import { Author } from '../contracts/drawing/defect/Author';
import {formatInitials} from '../utils/initialsHelper';

type Props = {
  user?: UserInfo | Author
  size: number
}

export function CustomAvatar ({user, size}: Props) { 

  if (!user) {
    return (
      <Avatar.Text color='red' size={size} label='??'/>
    );
  }

  return (
  <View>
    { user.userIconBase64 != undefined  && user.userIconBase64 != '' && user.userIconBase64 != null ? 
    <Avatar.Image size={size} source={{uri: user.userIconBase64}}/> :
    <Avatar.Text color='black' size={size} label={formatInitials(user.name, user.surname)}/>}
  </View>);
}
