import 'react-native-gesture-handler';
import React, { useState } from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { DefaultTheme, List, Text, Provider as PaperProvider, Button, TextInput } from 'react-native-paper';
import { createDrawerNavigator } from '@react-navigation/drawer';
import ProjectTab from './src/tabs/ProjectTab';
import ProductTab from './src/tabs/ProductTab';
import TemplateTab from './src/tabs/TemplateTab';
import { Provider, useDispatch, useSelector } from 'react-redux';
import { RootState, store } from './src/store/store';
import './src/extensions/string.extensions';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { View } from 'react-native';
import { setId } from './src/store/project-slice';

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

const Stack = createNativeStackNavigator();

const Drawer = createDrawerNavigator();

function SignInScreen() {
  const dispatch = useDispatch();
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');

  function canLogin() {
    return email != '' && password != '';
  }

  return (
    <View style={{padding: 20, marginTop: 150}}>
      <Text variant="titleLarge"  style={{alignSelf: 'center'}}>
        Sign In
      </Text>
      <TextInput
        error={false}
        mode='outlined'
        label="Email"
        value={email}
        onChangeText={text => setEmail(text)}
      />
      <TextInput
        error={false}
        secureTextEntry={true}
        mode='outlined'
        label="Password"
        value={password}
        onChangeText={text => setPassword(text)}
      />
      <Button disabled={!canLogin()} style={{marginTop: 10}} mode='contained' onPress={() => dispatch(setId(2))}>
        Login
      </Button>
    </View>
  );
}


function TempScreen() {
  const id = useSelector((state: RootState) => state.project.id);
  const [loggedIn, setLoggedIn] = useState(false);
  return (
    <NavigationContainer>
      { id >= 0 ?
      <Drawer.Navigator initialRouteName="Home">
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='folder' />, headerTitle: 'Projects' }} name={"Company Projects"} component={ProjectTab} />
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='file-cad' />, headerTitle: 'Products' }} name="Project Products" component={ProductTab} />
        <Drawer.Screen options={{ drawerIcon: () => <List.Icon icon='format-list-checks' />, headerTitle: 'Templates' }} name="Protocol Templates" component={TemplateTab} />
      </Drawer.Navigator> 
      :
      <Stack.Navigator>
        <Stack.Screen name="SignIn" options={{header: () => <></>}} component={SignInScreen} />
      </Stack.Navigator>
      }
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
