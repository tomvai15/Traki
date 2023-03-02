import React from 'react';
import { StyleSheet, View, Text } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { DefaultTheme, List, Provider as PaperProvider } from 'react-native-paper';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import ProjectTab from './src/tabs/ProjectTab';
import ProductTab from './src/tabs/ProductTab';

const theme = {
  ...DefaultTheme,
  roundness: 1,
  colors: {
    ...DefaultTheme.colors,
    primary: '#e4ae3f',
    accent: '#9ab1c0',
  },
};

function TempScreen() {
  return (
    <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
      <Text>Temp screen!</Text>
    </View>
  );
}

const Tab = createBottomTabNavigator();

export default function App() {
  return (
    <PaperProvider theme={theme}>
      <NavigationContainer>
        <Tab.Navigator screenOptions={{ headerShown: false }}>
          <Tab.Screen options={{ tabBarIcon: () => <List.Icon icon='folder' />, tabBarBadge: 1 }} name="Projektai" component={ProjectTab} />
          <Tab.Screen options={{ tabBarIcon: () => <List.Icon icon='group' />, tabBarBadge: 1 }} name="Produktai" component={ProductTab} />
        </Tab.Navigator>
      </NavigationContainer>
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
