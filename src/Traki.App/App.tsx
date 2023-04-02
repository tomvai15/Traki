import 'react-native-gesture-handler';
import React, { useEffect, useRef, useState } from 'react';
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
import { Animated, PanResponder, View, StyleSheet, Image } from 'react-native';
import { setId } from './src/store/project-slice';
import { image } from './src/screens/product/test';
import Svg, { Rect } from 'react-native-svg';

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

interface Rectangle {
  x: number;
  y: number;
  width: number;
  height: number;
}

const rect1: Rectangle = {
  x: 0,
  y: 0,
  width: 100,
  height: 100
}

const DefectPrototype = () => {
  const [rectangles, setRectangles] = useState<Rectangle[]>([rect1]);

  const [rectangle, setRectangle] = useState<Rectangle>(rect1);

  const panResponder = useRef(
    PanResponder.create({
     // Ask to be the responder:
     onStartShouldSetPanResponder: (evt, gestureState) => true,
     onStartShouldSetPanResponderCapture: (evt, gestureState) =>
       true,
     onMoveShouldSetPanResponder: (evt, gestureState) => true,
     onMoveShouldSetPanResponderCapture: (evt, gestureState) =>
       true,

     onPanResponderGrant: (evt, gestureState) => {
       // The gesture has started. Show visual feedback so the user knows
       // what is happening!
       // gestureState.d{x,y} will be set to zero now
       console.log('start');

       const rect: Rectangle = {
        x: evt.nativeEvent.locationX,
        y: evt.nativeEvent.locationY,
        width: 1,
        height: 1
       }

       setRectangle(rect);
     },
     onPanResponderMove: (evt, gestureState) => {
       // The most recent move distance is gestureState.move{X,Y}
       // The accumulated gesture distance since becoming responder is
       // gestureState.d{x,y}

       const a = evt.nativeEvent.locationX;
       const b = evt.nativeEvent.locationY;

       setRectangle((prevState => {
        let dx = a - prevState.x;
        let dy = b - prevState.y;

      //  dx = dx < 0 ? -1*dx : dx;
      //  dy = dy < 0 ? -1*dy : dy;

        const rect: Rectangle = {
          x: prevState.x,
          y: prevState.y,
          width: dx,
          height: dy
        } 
        return rect;
       }));
     },
     onPanResponderTerminationRequest: (evt, gestureState) =>
       true,
     onPanResponderRelease: (evt, gestureState) => {
       // The user has released all touches while this view is the
       // responder. This typically means a gesture has succeeded
       console.log('end');
     },
     onPanResponderTerminate: (evt, gestureState) => {
       // Another component has become the responder, so this gesture
       // should be cancelled
     },
     onShouldBlockNativeResponder: (evt, gestureState) => {
       // Returns whether this component should block native components from becoming the JS
       // responder. Returns true by default. Is currently only supported on android.
       return true;
     }
    }),
  ).current;

  return (
    <View style={styles.container}>
      <View {...panResponder.panHandlers} style={{width: 400, height: 400, borderColor: 'red', borderWidth: 2}}>
        <Image style={{width: '100%', height: '100%'}} source={{ uri: image }} />
        <Svg style={StyleSheet.absoluteFill}>
          <Rect
            x={rectangle.x}
            y={rectangle.y}
            width={rectangle.width}
            height={rectangle.height}
            stroke="black"
            strokeWidth="2"
            fill="transparent"
          />
        </Svg>
      </View>
    </View>
  );
};

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
        <Stack.Screen name="SignIn" options={{header: () => <></>}} component={DefectPrototype} />
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

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  titleText: {
    fontSize: 14,
    lineHeight: 24,
    fontWeight: 'bold',
  },
  box: {
    height: 150,
    width: 150,
    backgroundColor: 'blue',
    borderRadius: 5,
  },
});
