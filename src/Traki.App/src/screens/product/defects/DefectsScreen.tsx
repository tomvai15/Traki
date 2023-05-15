/* eslint-disable */
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState, useEffect } from 'react';
import { View, ScrollView, TouchableHighlight, TouchableOpacity } from 'react-native';
import { ProductStackParamList } from '../ProductStackParamList';
import { Title, Avatar, Card } from 'react-native-paper';
import ImageWithRegions from '../../../components/ImageWithRegions';
import { Drawing } from '../../../contracts/drawing/Drawing';
import drawingService from '../../../services/drawing-service';
import pictureService from '../../../services/picture-service';
import { Defect } from '../../../contracts/drawing/defect/Defect';
import { CustomAvatar } from '../../../components/CustomAvatar';

type Props = NativeStackScreenProps<ProductStackParamList, 'DefectsScreen'>;

const Wrench = () => <Avatar.Icon size={50} style={{backgroundColor:'red'}}  icon="wrench" />;

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

  return (
    <ScrollView>
      <Title>Defects</Title>
      <View style={{ marginVertical: 10 }}>
        <ScrollView horizontal={true}>
          {drawings.map((img, index) => 
            <TouchableHighlight key={index} style={{margin: 5}}>
              <ImageWithRegions source={img.imageBase64} height={300} defects={img.drawing.defects}/>
            </TouchableHighlight >
          )}
        </ScrollView>
      </View>
      <Card mode='outlined' style={{marginTop:10}}>
        <TouchableOpacity onPress={() => navigation.navigate('AddDefectScreen', {productId: productId})}>
          <Card.Title title="Add defect" left={Wrench} />
        </TouchableOpacity >
      </Card>
      <View style={{height: 400, marginTop: 10}}>
        {defects.map((item, index) => 
          <Card key={index} mode='outlined' style={{marginTop:10}}>
            <TouchableOpacity onPress={() => navigation.navigate('DefectScreen', {productId: productId, drawingId: item.drawingId, defectId: item.id})}>
              <Card.Title title={item.title} subtitle={item.description} left={() => <CustomAvatar user={item.author} size={50}/>} />
            </TouchableOpacity >
          </Card>
        )}
      </View>
    </ScrollView>
  );
};