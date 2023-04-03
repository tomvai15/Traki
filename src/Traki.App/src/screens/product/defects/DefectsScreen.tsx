/* eslint-disable */
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState, useRef, useEffect } from 'react';
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
import ImageWithRegions from '../../../components/ImageWithRegions';
import { Rectangle } from '../../../components/types/Rectangle';
import { Drawing } from '../../../contracts/drawing/Drawing';
import drawingService from '../../../services/drawing-service';
import pictureService from '../../../services/picture-service';

const rect1: Rectangle = {height: 0.40555190067590374, width: 0.07161458333333333, x: 0.24264356825086805, y: 0.15273983724257026};

type Props = NativeStackScreenProps<ProductStackParamList, 'DefectsScreen'>;

type ImageSize = {
  width: number,
  height: number
}

const Wrench = () => <Avatar.Icon size={50} style={{backgroundColor:'orange'}}  icon="wrench" />;

const images = [image, image2, image];

const defects = [1,2,3,4,5,6];

type DrawingWithImage = {
  drawing: Drawing,
  imageBase64: string
}

export default function DefectsScreen({route, navigation}: Props) {

  const {productId} = route.params;
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
    const drawingsWithImage: DrawingWithImage[] = [];

    for (let i = 0; i < drawings.length; i++) {
      let item = drawings[i];
      const imageBase64 = await pictureService.getPicture('company', item.imageName);
      const newDrawingImage: DrawingWithImage = {drawing: item, imageBase64: imageBase64};
      drawingsWithImage.push(newDrawingImage);
    };
    setDrawings(drawingsWithImage);
  }

  return (
    <View>
      <Title>Defects</Title>
      <View style={{ marginVertical: 10 }}>
        <ScrollView horizontal={true}>
          {drawings.map((img, index) => 
            <TouchableHighlight key={index} style={{margin: 5}}>
              <ImageWithRegions source={img.imageBase64} height={300} rectangles={[rect1]}/>
            </TouchableHighlight >
          )}
        </ScrollView>
      </View>
      <Button style={{marginVertical: 5}} buttonColor='red' onPress={() => navigation.navigate('AddDefectScreen', {productId: productId})} mode='contained'>Add Defect</Button>
      <ScrollView style={{height: 400}}>
        {defects.map((item, index) => 
          <Card key={index} mode='outlined' style={{marginTop:10}}>
            <TouchableOpacity onPress={() => navigation.navigate('DefectScreen', {productId: productId})}>
              <Card.Title title="Missing something" subtitle="saddsasadsda" left={Wrench} right={() => <CommentIcon text={'0'}/>} />
            </TouchableOpacity >
          </Card>
        )}
      </ScrollView>
    </View>
  );
};