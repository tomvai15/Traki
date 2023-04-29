/* eslint-disable */
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState, useRef, useEffect } from 'react';
import { View, Image, StyleSheet, PanResponder, ScrollView, TouchableHighlight } from 'react-native';
import Svg, { Rect } from 'react-native-svg';
import { ProductStackParamList } from '../ProductStackParamList';
import { image } from '../test';
import { image2 } from '../test2';
import { image3 } from '../test3';
import * as ImagePicker from 'expo-image-picker';
import { DefaultTheme, List, Text, Provider as PaperProvider, Button, TextInput, Title, Portal, Dialog, IconButton, HelperText } from 'react-native-paper';
import AutoImage from '../../../components/AutoImage';
import ImageView from "react-native-image-viewing";
import defectService from '../../../services/defect-service';
import { Drawing } from '../../../contracts/drawing/Drawing';
import { Defect } from '../../../contracts/drawing/defect/Defect';
import { DefectStatus } from '../../../contracts/drawing/defect/DefectStatus';
import drawingService from '../../../services/drawing-service';
import pictureService from '../../../services/picture-service';
import { CreateDefectRequest } from '../../../contracts/drawing/defect/CreateDefectRequest';
import uuid from 'react-native-uuid';
import { manipulateAsync, FlipType, SaveFormat } from 'expo-image-manipulator';
import { validate, validationRules } from '../../../utils/textValidation';

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

type DrawingWithImage = {
  drawing: Drawing,
  imageBase64: string
}

type Props = NativeStackScreenProps<ProductStackParamList, 'AddDefectScreen'>;

const images = [image, image2, image];

export default function AddDefectScreen({route, navigation}: Props) {

  const {productId} = route.params;
  const [selectedDrawing, setSelectedDrawing] = useState<DrawingWithImage>();
  const [rectangle, setRectangle] = useState<Rectangle>(rect1);

  const [selectedImage, setSelectedImage] = useState<string>(image);
  const [visible, setVisible] = React.useState(false);
  const [imageWidth, setImageWidth] = useState<number>(405);
  const [drawings, setDrawings] = useState<DrawingWithImage[]>([]);
  
  useEffect(() => {
    fetchDrawings();
  }, [])

  async function fetchDrawings() {
    const response = await drawingService.getDrawings(Number(productId));
    await fetchDrawingPictures(response.drawings);
  }

  async function fetchDrawingPictures(drawings: Drawing[]) {
    console.log(drawings);

    if (drawings.length == 0) {
      return;
    }
    const drawingsWithImage: DrawingWithImage[] = [];

    for (let i = 0; i < drawings.length; i++) {
      let item = drawings[i];
      const imageBase64 = await pictureService.getPicture('company', item.imageName);
      const newDrawingImage: DrawingWithImage = {drawing: item, imageBase64: imageBase64};
      drawingsWithImage.push(newDrawingImage);
    };
    setSelectedDrawing(drawingsWithImage[0]);
    setDrawings(drawingsWithImage);
  }

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

        const rect: Rectangle = {
          x: prevState.x,
          y: prevState.y,
          width: dx,
          height: dy
        } 

        console.log(rect);
        
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
  
  async function createDefect(title: string, description: string, pictureName: string) {
    if (selectedDrawing == null) {
      return;
    }

    Image.getSize(selectedImage, async (sourceImageWidth, sourceImageHeight) => {

       console.log(sourceImageWidth + ' ' + sourceImageHeight);
       const newWidth = imageWidth;
       // TODO: investigate  reason behind -20
       const newHeight = ((sourceImageHeight*imageWidth)/sourceImageWidth) - 20;

       console.log(newWidth + ' ' + newHeight);
       console.log(rectangle.x + ' ' + rectangle.y);
      
       const xPerc = rectangle.x/newWidth;
       const yPerc = rectangle.y/newHeight;
       const widthPerc = rectangle.width/newWidth;
       const heightPerc = rectangle.height/newHeight;

       const rectPerc: Rectangle  = {
        x: xPerc * 100,
        y: yPerc * 100,
        width: widthPerc * 100,
        height: heightPerc * 100,
       }

       const newDefect: Defect = {
        id: 0,
        title: title,
        description: description,
        imageName: pictureName,
        status: DefectStatus.NotFixed,
        x: rectPerc.x,
        y: rectPerc.y,
        width: rectPerc.width,
        height: rectPerc.height,
        drawingId: 0
       };

       console.log(rectPerc);
       console.log(newDefect);

       const request: CreateDefectRequest = {
        defect: newDefect
       }
       const response = await defectService.createDefect(selectedDrawing.drawing.id, request);
       const createdDefect = response.defect;
       setVisible(false);
       navigation.navigate('DefectScreen', {productId: productId, drawingId: createdDefect.drawingId, defectId: createdDefect.id});
    });
  }

  return (
    <View>
      <AddDefectDialog onSubmit={createDefect} onClose={() => setVisible(false)} visible={visible}></AddDefectDialog>
      <Title>Select Region</Title>
      { selectedDrawing && <View {...panResponder.panHandlers} style={{ display: 'flex', borderColor: 'red', borderWidth: 2}}>
        <AutoImage
          source={selectedDrawing.imageBase64}
          width={imageWidth}
        />
        <Svg style={StyleSheet.absoluteFill} color={'red'}>
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
      </View>}
      <View>
        <ScrollView horizontal={true}>
          {drawings.map((drawing, index) => 
            <TouchableHighlight key={index} style={{margin: 5}} onPress={() => setSelectedDrawing(drawing)}>
              <AutoImage
                source={drawing.imageBase64}
                height={150}
              />
            </TouchableHighlight >
          )}
        </ScrollView>
      </View>
      <Button onPress={() => setVisible(true)} mode='contained'>Confirm Region</Button>
    </View>
  );
};

type AddDefectDialogProps = {
  visible: boolean
  onClose: () => void,
  onSubmit: (title: string, description: string, filename: string) => void
}

function AddDefectDialog({visible, onClose, onSubmit}: AddDefectDialogProps) {

  const [imageUri, setImageUri] = useState<string>('');
  const [title, setTitle] = useState<string>('');
  const [description, setDescription] = useState<string>('');
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

    result.assets[0].uri;

    const manipResult = await manipulateAsync(
      result.assets[0].uri,
      [],
      { compress: 0.1 }
    )
    const localUri = manipResult.uri;
    setImageUri(localUri);
  };

  async function createDefect() {
    let pictureName = '';
    if (imageUri != null) {
      pictureName = `${uuid.v4().toString()}.jpeg`;
      await uploadImage(imageUri, pictureName);
    }
    onSubmit(title, description, pictureName);
    setDescription('');
    setTitle('');
    setImageUri('');
  }

  async function uploadImage(imageUri: string, pictureName: string) {
    let filename = imageUri.split('/').pop() ?? '';
    let match = /\.(\w+)$/.exec(filename);
    let type = match ? `image/${match[1]}` : `image`;

    let formData = new FormData();
    formData.append('photo', JSON.parse(JSON.stringify({ uri: imageUri, name: pictureName, type })));
    await pictureService.uploadPicturesFormData('item', formData)
  }

  function canSubmit(): boolean {
    return title!='' && description !='' &&
            !validate(title, [validationRules.noSpecialSymbols]).message &&
            !validate(description, [validationRules.noSpecialSymbols]).message;
  }

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
              maxLength={50}
              error={validate(title, [validationRules.noSpecialSymbols]).invalid}
              multiline={true} 
              value={title} 
              onChangeText={(value) => setTitle(value)}>
            </TextInput>
            <HelperText type="error" visible={validate(title, [validationRules.noSpecialSymbols]).invalid}>
              {validate(title, [validationRules.noSpecialSymbols]).message}
            </HelperText>
            <TextInput 
              maxLength={250}
              error={validate(description, [validationRules.noSpecialSymbols]).invalid}
              label={'Description'} 
              multiline={true} 
              value={description} 
              onChangeText={(value) => setDescription(value)}>     
            </TextInput>
            <HelperText type="error" visible={validate(description, [validationRules.noSpecialSymbols]).invalid}>
              {validate(description, [validationRules.noSpecialSymbols]).message}
            </HelperText>
          <View style={{margin: 10, display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}}>  
            {imageUri != '' && imageUri != undefined ? 
              <TouchableHighlight onPress={() => setViewerActive(true)}>
                <Image source={{uri: imageUri}} style={{width: 100, height: 100}} ></Image>
              </TouchableHighlight> :
              <View></View>
            }
            <IconButton onPress={() => pickImage()} size={30} icon='camera' />
          </View>
          </Dialog.Content>
          <Dialog.Actions>
            <Button onPress={() => onClose()}>Go back</Button>
            <Button disabled={!canSubmit()} mode='contained' onPress={() => createDefect()}>Submit</Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>
  );
}