import  React, { useState } from 'react';
import { View } from 'react-native';
import { Button, Text, TextInput } from 'react-native-paper';
import { userState } from '../../state/user-state';
import { useRecoilState } from 'recoil';

export default function UserScreen() {
  const [userInfo, setUserInfo] = useRecoilState(userState);

  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');

  async function logOut() {
    setUserInfo({...userInfo, token: ''})
    return;
  }

  return (
    <View style={{padding: 20, marginTop: 150}}>
      <Button style={{marginTop: 10}} mode='contained' onPress={logOut}>
        Log Out
      </Button>
    </View>
  );
}