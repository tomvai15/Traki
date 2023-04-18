import 'react-native-gesture-handler';
import React, { useEffect } from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { List, Provider as PaperProvider } from 'react-native-paper';
import { createDrawerNavigator } from '@react-navigation/drawer';
import './src/extensions/string.extensions';
import { theme } from './src/themes/theme';
import { RecoilRoot, useRecoilState } from 'recoil';
import ReactNativeRecoilPersist, { ReactNativeRecoilPersistGate } from "react-native-recoil-persist";
import { userState } from './src/state/user-state';
import { ProjectTab, ProductTab, TemplateTab, AuthTab } from './src/tabs';
import RecoilNexus from 'recoil-nexus'
import UserScreen from './src/screens/user/UserScreen';
import * as Notifications from "expo-notifications";
import { useNotifications } from './src/hooks/useNotifications';

const Drawer = createDrawerNavigator();

export default function App() {

  const { registerForPushNotificationsAsync, handleNotification, handleNotificationResponse} = useNotifications();

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
    <PaperProvider theme={theme}>
      <RecoilRoot>
        <ReactNativeRecoilPersistGate store={ReactNativeRecoilPersist}>
          <RecoilNexus/>
          <Main/>
        </ReactNativeRecoilPersistGate>
      </RecoilRoot>
    </PaperProvider>
  );
}

function Main() {
  const [userInfo, setUserInfo] = useRecoilState(userState);

  function loggedIn() {
    return userInfo.token != '';
  }

  return (
    <NavigationContainer>
      { loggedIn() ?
      <Drawer.Navigator initialRouteName="Home">
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='folder' />, headerTitle: 'Projects' }} name={"Company Projects"} component={ProjectTab} />
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='file-cad' />, headerTitle: 'Products' }} name="Project Products" component={ProductTab} />
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='format-list-checks' />, headerTitle: 'Templates' }} name="Protocol Templates" component={TemplateTab} />
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='account' />, headerTitle: 'Account Information' }} name="Account information" component={UserScreen} />
      </Drawer.Navigator> 
      :
        <AuthTab/>
      }
    </NavigationContainer>
  );
}
