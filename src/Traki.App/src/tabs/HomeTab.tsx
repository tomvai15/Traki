import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import HomeScreen from '../screens/home/HomeScreen';

const HomeStack = createNativeStackNavigator();

export default function HomeTab() {
  return (
    <HomeStack.Navigator>
      <HomeStack.Screen name="Home" options={{header: () => <></>}} component={HomeScreen} />
    </HomeStack.Navigator>
  );
}