import  React, { useEffect, useState } from 'react';
import { View } from 'react-native';
import { Button, Text, TextInput } from 'react-native-paper';
import { userState } from '../../state/user-state';
import { useRecoilState } from 'recoil';
import { authService } from '../../services';
import { LoginRequest } from '../../contracts/auth/LoginRequest';
import { RegisterDeviceRequest } from '../../contracts/auth/RegisterDeviceRequest';
import { deviceState } from '../../state/device-state';
import { useUserInformation } from '../../hooks/useUserInformation';

export default function SignInScreen() {

  const { fetchFullUserInformation } = useUserInformation();

  const [userInfo, setUserInfo] = useRecoilState(userState);
  const [deviceInfo, setDeviceInfo] = useRecoilState(deviceState);

  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');

  useEffect(() => {
    fetchFullUserInformation();
  }, []);

  function canLogin() {
    return email != '' && password != '';
  }

  async function login() {
    const request: LoginRequest = {
      email: email,
      password: password
    };
    try {
      const response = await authService.login(request);
      console.log(response);
      setUserInfo({...userInfo, token: response.token, refreshToken: response.refreshToken});

      const registerDeviceRequest: RegisterDeviceRequest = {
        deviceToken: deviceInfo.token
      };
      await fetchFullUserInformation();

      console.log( '?? ' + registerDeviceRequest);

      await authService.registerDevice(registerDeviceRequest);

    } catch (err) {
      console.log(err);
    }
    
    return;
  }

  return (
    <View style={{padding: 20, marginTop: 150}}>
      <Text variant="titleLarge"  style={{alignSelf: 'center'}}>
        Sign In
      </Text>
      <TextInput
        outlineStyle={{borderRadius: 15}}
        error={false}
        mode='outlined'
        label="Email"
        value={email}
        onChangeText={text => setEmail(text)}
      />
      <TextInput
        outlineStyle={{borderRadius: 15}}
        error={false}
        secureTextEntry={true}
        mode='outlined'
        label="Password"
        value={password}
        onChangeText={text => setPassword(text)}
      />
      <Button disabled={!canLogin()} style={{marginTop: 10}} mode='contained' onPress={login}>
        Login
      </Button>
    </View>
  );
}