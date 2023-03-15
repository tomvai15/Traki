import 'react-native-gesture-handler';
import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { DefaultTheme, List, Provider as PaperProvider } from 'react-native-paper';
import { createDrawerNavigator } from '@react-navigation/drawer';
import ProjectTab from './src/tabs/ProjectTab';
import ProductTab from './src/tabs/ProductTab';
import TemplateTab from './src/tabs/TemplateTab';
import { Provider, useSelector } from 'react-redux';
import { RootState, store } from './src/store/store';
import './src/extensions/string.extensions';

const theme = {
  ...DefaultTheme,
  roundness: 1,
  colors: {
    ...DefaultTheme.colors,
    primary: '#e4ae3f',
    secondaryContainer: '#ffd47d',
    onSecondaryContainer: '#F14444',
    background: 'white',
    surface: '#f7f5f2',
    surfaceVariant: '#ffeecc', // TextInput
    outline: '#c7cfd4',
    accent: '#9ab1c0',
    error: '#F14444',
  },
};

const Drawer = createDrawerNavigator();

function TempScreen() {
  const id = useSelector((state: RootState) => state.project.id);
  
  return (
    <NavigationContainer>
    <Drawer.Navigator initialRouteName="Home">
    <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='folder' />, headerTitle: '' }} name={"Projektai"} component={ProjectTab} />
      <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='file-cad' />, headerTitle: 'Produktai' }} name="Produktai" component={ProductTab} />
      <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='format-list-checks' />, headerTitle: '' }} name="Šablonai" component={TemplateTab} />
    </Drawer.Navigator>
  </NavigationContainer>
  );
}

export default function App() {
  
  return (
    <PaperProvider theme={theme}>
      <Provider store={store}>
        <TempScreen/>
      </Provider>
    </PaperProvider>
  );
}
