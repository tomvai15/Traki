import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import SignInScreen from '../screens/auth/SignInScreen';

const AuthStack = createNativeStackNavigator();

export default function AuthTab() {
  return (
    <AuthStack.Navigator>
      <AuthStack.Screen name="SignIn" options={{header: () => <></>}} component={SignInScreen} />
    </AuthStack.Navigator>
  );
}