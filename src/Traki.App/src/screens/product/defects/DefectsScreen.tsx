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
import { Defect } from '../../../contracts/drawing/defect/Defect';

const rect1: Rectangle = {height: 5.379220725988574, width: 8.324027355806326, x: 84.9352424527392, y: 23.948985593123503};

type Props = NativeStackScreenProps<ProductStackParamList, 'DefectsScreen'>;

type ImageSize = {
  width: number,
  height: number
}

const Wrench = () => <Avatar.Icon size={50} style={{backgroundColor:'orange'}}  icon="wrench" />;

const images = [image, image2, image];

//const defects = [1,2,3,4,5,6];

type DrawingWithImage = {
  drawing: Drawing,
  imageBase64: string
}

export default function DefectsScreen({route, navigation}: Props) {

  const {productId} = route.params;
  const [drawings, setDrawings] = useState<DrawingWithImage[]>([]);
  const [defects, setDefects] = useState<Defect[]>([]);
  
  useEffect(() => {
    fetchDrawings();
  }, [])

  async function fetchDrawings() {
    const response = await drawingService.getDrawings(Number(productId));
    await fetchDrawingPictures(response.drawings);
  }

  async function fetchDrawingPictures(drawings: Drawing[]) {
    const drawingsWithImage: DrawingWithImage[] = [];
    const newDefects: Defect[] = [];
    for (let i = 0; i < drawings.length; i++) {
      let item = drawings[i];
      newDefects.push(...item.defects);
      const imageBase64 = await pictureService.getPicture('company', item.imageName);
      const newDrawingImage: DrawingWithImage = {drawing: item, imageBase64: imageBase64};
      drawingsWithImage.push(newDrawingImage);
    };

    setDefects(newDefects);
    setDrawings(drawingsWithImage);
  }

  function defectsToRectangles(defects: Defect[]): Rectangle[] {
    return defects.map(x=> defectToRectangle(x));
  }

  function defectToRectangle(defect: Defect) : Rectangle {
    return {x: defect.x, y: defect.y, width: defect.width, height: defect.height};
  }

  return (
    <View>
      <Title>Defects</Title>
      <View style={{ marginVertical: 10 }}>
        <ScrollView horizontal={true}>
          {drawings.map((img, index) => 
            <TouchableHighlight key={index} style={{margin: 5}}>
              <ImageWithRegions source={img.imageBase64} height={300} rectangles={defectsToRectangles(img.drawing.defects)}/>
            </TouchableHighlight >
          )}
        </ScrollView>
      </View>
      <Button style={{marginVertical: 5}} buttonColor='red' onPress={() => navigation.navigate('AddDefectScreen', {productId: productId})} mode='contained'>Add Defect</Button>
      <ScrollView style={{height: 400}}>
        {defects.map((item, index) => 
          <Card key={index} mode='outlined' style={{marginTop:10}}>
            <TouchableOpacity onPress={() => navigation.navigate('DefectScreen', {productId: productId, drawingId: item.drawingId, defectId: item.id})}>
              <Card.Title title={item.title} subtitle={item.description} left={Wrench} right={() => <CommentIcon text={'0'}/>} />
            </TouchableOpacity >
          </Card>
        )}
      </ScrollView>
    </View>
  );
};