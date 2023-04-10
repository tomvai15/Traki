import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import UserScreen from '../screens/user/UserScreen';

const UserStack = createNativeStackNavigator();

export default function UserTab() {
  return (
    <UserStack.Navigator screenOptions={{ header: ()=> <></>}}>
      <UserStack.Screen name="User" options={{title: 'User',}} component={UserScreen} />
    </UserStack.Navigator>
  );
}
