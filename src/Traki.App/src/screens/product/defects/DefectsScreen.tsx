/* eslint-disable */
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState, useRef } from 'react';
import { View, Image, StyleSheet, PanResponder, ScrollView, TouchableHighlight, TouchableOpacity } from 'react-native';
import Svg, { Rect } from 'react-native-svg';
import { ProductStackParamList } from '../ProductStackParamList';
import { image } from '../test';
import { image2 } from '../test2';
import { image3 } from '../test3';
import * as ImagePicker from 'expo-image-picker';
import { DefaultTheme, List, Text, Provider as PaperProvider, Button, TextInput, Title, Portal, Dialog, IconButton, Avatar, Card } from 'react-native-paper';
import AutoImage from '../../../components/AutoImage';
import ImageView from "react-native-image-viewing";
import { CommentIcon } from '../../../components/CommentIcon';

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

type Props = NativeStackScreenProps<ProductStackParamList, 'DefectsScreen'>;

type ImageSize = {
  width: number,
  height: number
}

const Wrench = () => <Avatar.Icon size={50} style={{backgroundColor:'orange'}}  icon="wrench" />;

const images = [image, image2, image];

const defects = [1,2,3,4,5,6];

export default function DefectsScreen({route, navigation}: Props) {

  const {productId} = route.params;
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

  return (
    <View>
      <Title>Defect</Title>
      <ScrollView style={{height: 300}}>
        {defects.map((item, index) => 
          <Card key={index} mode='outlined' style={{marginTop:10}}>
            <TouchableOpacity onPress={() => navigation.navigate('DefectScreen', {productId: productId})}>
              <Card.Title title="Missing something" subtitle="saddsasadsda" left={Wrench} right={() => <CommentIcon text={'0'}/>} />
            </TouchableOpacity >
          </Card>
        )}
      </ScrollView>
      <View style={{ marginVertical: 10 }}>
        <ScrollView horizontal={true}>
          {images.map((img, index) => 
            <TouchableHighlight key={index} style={{margin: 5}} onPress={() => setSelectedImage(img)}>
              <AutoImage
                source={img}
                height={300}
              />
            </TouchableHighlight >
          )}
        </ScrollView>
      </View>
      <Button buttonColor='red' onPress={() => navigation.navigate('AddDefectScreen', {productId: productId})} mode='contained'>Add Defect</Button>
    </View>
  );
};