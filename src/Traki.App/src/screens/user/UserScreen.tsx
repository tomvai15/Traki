import  React, { useState } from 'react';
import { View } from 'react-native';
import { Button, Text, TextInput } from 'react-native-paper';
import { userState } from '../../state/user-state';
import { useRecoilState } from 'recoil';
import { CustomAvatar } from '../../components/CustomAvatar';

export default function UserScreen() {
  const [userInfo, setUserInfo] = useRecoilState(userState);

  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');

  async function logOut() {
    setUserInfo({...userInfo, token: ''})
    return;
  }

  return (
    <View style={{padding: 20}}>
      <CustomAvatar size={50} user={userInfo.user}></CustomAvatar>
      <TextInput style={{marginTop: 10}} mode='outlined' label={'Email'} value={userInfo.user?.email} editable={false}></TextInput>
      <TextInput style={{marginTop: 10}} mode='outlined' label={'Name'} value={userInfo.user?.name} editable={false}></TextInput>
      <TextInput style={{marginTop: 10}} mode='outlined' label={'Surname'} value={userInfo.user?.surname} editable={false}></TextInput>
    </View>
  );
}