import 'react-native-gesture-handler';
import React from 'react';
import { StyleSheet, View, Text } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { DefaultTheme, List, Provider as PaperProvider } from 'react-native-paper';
import { createDrawerNavigator } from '@react-navigation/drawer';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import ProjectTab from './src/tabs/ProjectTab';
import ProductTab from './src/tabs/ProductTab';
import TemplateTab from './src/tabs/TemplateTab';
import { Provider, useSelector } from 'react-redux';
import { RootState, store } from './src/store/store';

const theme = {
  ...DefaultTheme,
  roundness: 1,
  colors: {
    ...DefaultTheme.colors,
    primary: '#e4ae3f',
    accent: '#9ab1c0',
    error: '#F14444'
  },
};

function TempScreen() {
  const { message } = useSelector((state: RootState) => state.message);
  
  return (
    <NavigationContainer>
    <Drawer.Navigator initialRouteName="Home">
      <Drawer.Screen name="Produktai" component={ProductTab} />
      <Drawer.Screen name="Šablonai" component={TemplateTab} />
      <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='folder' />, headerTitle: '' }} name={"Projektai"} component={ProjectTab} />
      <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='human' />, headerTitle: '' }} name={message} component={TemplateTab} />
    </Drawer.Navigator>
  </NavigationContainer>
  );
}

const Tab = createBottomTabNavigator();
const Drawer = createDrawerNavigator();

export default function App() {
  
  return (
    <PaperProvider theme={theme}>
      <Provider store={store}>
        <TempScreen/>
      </Provider>
    </PaperProvider>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
