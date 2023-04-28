import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import HomeScreen from '../screens/home/HomeScreen';
import { HomeStackParamList } from '../screens/home/HomeStackParamList';

const HomeStack = createNativeStackNavigator<HomeStackParamList>();

export default function HomeTab() {
  return (
    <HomeStack.Navigator>
      <HomeStack.Screen name='HomeScreen' options={{header: () => <></>}} component={HomeScreen} />
    </HomeStack.Navigator>
  );
}