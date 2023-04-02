/* eslint-disable */
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState, useRef } from 'react';
import { View, Image, StyleSheet, PanResponder, ScrollView, TouchableHighlight } from 'react-native';
import Svg, { Rect } from 'react-native-svg';
import { ProductStackParamList } from '../ProductStackParamList';
import { image } from '../test';
import { image2 } from '../test2';
import { image3 } from '../test3';
import * as ImagePicker from 'expo-image-picker';
import { DefaultTheme, List, Text, Provider as PaperProvider, Button, TextInput, Title, Portal, Dialog, IconButton } from 'react-native-paper';
import AutoImage from '../../../components/AutoImage';
import ImageView from "react-native-image-viewing";

interface Rectangle {
  x: number;
  y: number;
  width: number;
  height: number;
}

const rect1: Rectangle = {
  x: 0,
  y: 0,
  width: 1,
  height: 1
}

type Props = NativeStackScreenProps<ProductStackParamList, 'AddDefectScreen'>;

type ImageSize = {
  width: number,
  height: number
}

const images = [image, image2, image];

export default function AddDefectScreen({route, navigation}: Props) {
  const [rectangles, setRectangles] = useState<Rectangle[]>([rect1]);

  const [rectangle, setRectangle] = useState<Rectangle>(rect1);

  const [imageUri, setImageUri] = useState<string>('');

  const [selectedImage, setSelectedImage] = useState<string>(image);

  const [visible, setVisible] = React.useState(false);

  const pickImage = async () => {
    const result = await ImagePicker.launchCameraAsync({
      mediaTypes: ImagePicker.MediaTypeOptions.All,
      allowsEditing: true,
      aspect: [4, 3],
      quality: 1,
    });

    console.log(result);

    if (result.canceled) {
      return;
    }
    const localUri = result.assets[0].uri;

    setImageUri(localUri);
  };

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
       //console.log('start');

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
       //console.log('end');
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
    <View>
      <AddDefectDialog onClose={() => setVisible(false)} visible={visible}></AddDefectDialog>
      <Title>Add defect</Title>
      <View {...panResponder.panHandlers} style={{ borderColor: 'red', borderWidth: 2}}>
        <AutoImage
          source={selectedImage}
          width={405}
        />
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
      <View style={{ marginVertical: 10 }}>
        <ScrollView horizontal={true}>
          {images.map((img, index) => 
            <TouchableHighlight key={index} style={{margin: 5}} onPress={() => setSelectedImage(img)}>
              <AutoImage
                source={img}
                height={150}
              />
            </TouchableHighlight >
          )}
        </ScrollView>
      </View>
      <Button onPress={() => setVisible(true)} mode='contained'>SELECT REGION</Button>
    </View>
  );
};

type AddDefectDialogProps = {
  visible: boolean
  onClose: () => void
}

function AddDefectDialog({visible, onClose}: AddDefectDialogProps) {

  const [imageUri, setImageUri] = useState<string>();
  const [title, setTitle] = useState<string>();
  const [description, setDescription] = useState<string>();
  const [viewerActive, setViewerActive] = useState<boolean>(false);

  const pickImage = async () => {
    const result = await ImagePicker.launchCameraAsync({
      mediaTypes: ImagePicker.MediaTypeOptions.All,
      allowsEditing: true,
      aspect: [4, 3],
      quality: 1,
    });

    if (result.canceled) {
      return;
    }

    const localUri = result.assets[0].uri;
    setImageUri(localUri);
  };

  return (
    <Portal>
       { imageUri &&
        <ImageView
          images={[{uri: imageUri}]}
          imageIndex={0}
          visible={viewerActive}
          onRequestClose={() => setViewerActive(false)}
        />}
        <Dialog visible={visible} onDismiss={onClose}>
          <Dialog.Title>Add defect</Dialog.Title>
          <Dialog.Content>
            <TextInput label={'Title'} 
              multiline={true} 
              value={title} 
              onChangeText={(value) => setTitle(value)}>
            </TextInput>
            <TextInput label={'description'} 
              multiline={true} 
              value={description} 
              onChangeText={(value) => setDescription(value)}>     
            </TextInput>
          <View style={{margin: 10, display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}}>  
            {imageUri != '' && 
              <TouchableHighlight onPress={() => setViewerActive(true)}>
                <Image source={{uri: imageUri}} style={{width: 100, height: 100}} ></Image>
              </TouchableHighlight>
            }
            <IconButton onPress={() => pickImage()} size={30} icon='camera' />
          </View>
          </Dialog.Content>
          <Dialog.Actions>
            <Button onPress={() => onClose()}>Go back</Button>
            <Button onPress={() => onClose()}>Submit</Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>
  );

}

//<TextInput label="Title" multiline={true}></TextInput>
//<TextInput label="Description" multiline={true}></TextInput>
//       <Button mode='contained' onPress={pickImage} >Add Image</Button>
//      {imageUri &&  <Image source={{ uri: imageUri }} style={{ width: 100, height: 100 }} />}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF',
  },
  imageContainer: {
    width: 150, // replace with your desired width
    height: 150, // replace with your desired height
    margin: 5
  },
  image: {
    flex: 1,
    resizeMode: 'contain',
  },
  imagePlaceholder: {
    backgroundColor: '#DDD',
    alignSelf: 'stretch',
    alignItems: 'center',
    justifyContent: 'center',
  },
});