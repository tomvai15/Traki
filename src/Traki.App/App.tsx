import 'react-native-gesture-handler';
import { StatusBar, StatusBarStyle } from 'react-native'
import './src/extensions/string.extensions';
import React, { useEffect, useState } from 'react';
import { View, Text } from 'react-native';
import { Button, IconButton, List, Provider as PaperProvider } from 'react-native-paper';
import { NavigationContainer } from '@react-navigation/native';
import { createDrawerNavigator } from '@react-navigation/drawer';
import { theme } from './src/themes/theme';
import { RecoilRoot, useRecoilState } from 'recoil';
import ReactNativeRecoilPersist, { ReactNativeRecoilPersistGate } from "react-native-recoil-persist";
import { userState } from './src/state/user-state';
import { ProductTab, TemplateTab, AuthTab } from './src/tabs';
import RecoilNexus from 'recoil-nexus'
import UserScreen from './src/screens/user/UserScreen';
import * as Notifications from "expo-notifications";
import { useNotifications } from './src/hooks/useNotifications';
import MainHeader from './src/tabs/layout/MainHeader';
import HomeTab from './src/tabs/HomeTab';

const Drawer = createDrawerNavigator();

const STYLES = ['default', 'dark-content', 'light-content'] as const;
const TRANSITIONS = ['fade', 'slide', 'none'] as const;

export default function App() {

  const [statusBarStyle, setStatusBarStyle] = useState<StatusBarStyle>(
    STYLES[1],
  );
  const [statusBarTransition, setStatusBarTransition] = useState<
    'fade' | 'slide' | 'none'
  >(TRANSITIONS[0]);

  return (
    <PaperProvider theme={theme}>
      <RecoilRoot>
        <ReactNativeRecoilPersistGate store={ReactNativeRecoilPersist}>
          <StatusBar
            animated={true}
            backgroundColor="#D5D5D5"
            barStyle={statusBarStyle}
            showHideTransition={statusBarTransition}
            hidden={false}
          />
          <RecoilNexus/>
          <Main/>
        </ReactNativeRecoilPersistGate>
      </RecoilRoot>
    </PaperProvider>
  );
}

function Main() {
  const [userInfo] = useRecoilState(userState);

  function loggedIn() {
    return userInfo.token != '';
  }

  const { registerForPushNotificationsAsync, handleNotificationResponse} = useNotifications();

  useEffect(() => {

    registerForPushNotificationsAsync();
    Notifications.setNotificationHandler({
      handleNotification: async () => ({
        shouldShowAlert: true,
        shouldPlaySound: true,
        shouldSetBadge: true,
      }),
    });

    const responseListener = Notifications.addNotificationResponseReceivedListener(handleNotificationResponse);

    return () => {
      if (responseListener) {
        Notifications.removeNotificationSubscription(responseListener);
      }
    }
  }, []);

  return (
    <NavigationContainer>
      { loggedIn() ?
      <Drawer.Navigator screenOptions={
        ({navigation}) => ({
          headerRight: () => (<MainHeader navigation={navigation}/>)
        })} initialRouteName="Home">
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='home' />, headerTitle: 'Home' }} name={"Home"} component={HomeTab} />
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='file-cad' />, headerTitle: 'Projects' }} name="Projects" component={ProductTab} />
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='format-list-checks' />, headerTitle: 'Templates' }} name="Protocol Templates" component={TemplateTab} />
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='account' />, headerTitle: 'Account Information' }} name="Account information" component={UserScreen} />
      </Drawer.Navigator> 
      :
        <AuthTab/>
      }
    </NavigationContainer>
  );
}
