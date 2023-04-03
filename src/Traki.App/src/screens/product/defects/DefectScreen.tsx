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
import ImageWithViewer from '../../../components/ImageWithViewer';
import { Drawing } from '../../../contracts/drawing/Drawing';
import { Defect } from '../../../contracts/drawing/defect/Defect';
import drawingService from '../../../services/drawing-service';
import defectService from '../../../services/defect-service';
import pictureService from '../../../services/picture-service';
import ImageWithRegions from '../../../components/ImageWithRegions';
import { DefectComment } from '../../../contracts/drawing/defect/DefectComment';
import { CreateDefectCommentRequest } from '../../../contracts/drawing/defect/CreateDefectCommentRequest';

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

type Props = NativeStackScreenProps<ProductStackParamList, 'DefectScreen'>;

type ImageSize = {
  width: number,
  height: number
}

const Wrench = () => <Avatar.Icon size={50} style={{backgroundColor:'orange'}}  icon="wrench" />;

const images = [image, image2, image];

type DrawingWithImage = {
  drawing: Drawing,
  imageBase64: string
}

export default function DefectScreen({route, navigation}: Props) {
  const {productId, drawingId, defectId} = route.params;
  const [drawing, setDrawing] = useState<DrawingWithImage>();
  const [defect, setDefect] = useState<Defect>();

  const [commentText, setCommentText] = useState<string>('');

  useEffect(() => {
    fetchDefect();
    fetchDrawing();
  }, []);

  async function fetchDefect() {
    const response = await defectService.getDefect(drawingId, defectId);
    setDefect(response.defect);
  }

  async function fetchDrawing() {
    const response = await drawingService.getDrawing(productId, 1);
    const imageBase64 = await pictureService.getPicture('company', response.drawing.imageName);
    const newDrawingImage: DrawingWithImage = {drawing: response.drawing, imageBase64: imageBase64};
    setDrawing(newDrawingImage);
  }

  const [imageUri, setImageUri] = useState<string>('');

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

  function defectToRectangle(defect: Defect) : Rectangle {
    return {x: defect.x, y: defect.y, width: defect.width, height: defect.height};
  }

  async function createComment() {
    const defectComment: DefectComment = {
      id: 0,
      text: commentText,
      date: '',
      author: ''
    };

    const request: CreateDefectCommentRequest = {
      defectComment: defectComment
    };

    await defectService.createDefectComment(drawingId, defectId, request);
    await fetchDefect();

    setCommentText('');
  }

  return (
    <ScrollView>
      <Card mode='outlined' style={{marginTop:10}}>
        <Card.Title title={defect?.title}
          subtitle={defect?.description} 
          left={() => <Avatar.Text size={50} label="TV" />} 
          right={() => <View style={{margin: 10}}><ImageWithViewer source={image} width={60} height={100} ></ImageWithViewer></View>}
        />
      </Card>
      <View style={{ marginVertical: 10 }}>
        { defect && drawing && <ImageWithRegions source={drawing.imageBase64} height={300} rectangles={[defectToRectangle(defect)]}/>}
      </View>
      <Text>Comments</Text>
      { defect?.defectComments?.map((item, index) => <Card key={index} mode='outlined' style={{marginTop:10}}>
        <Card.Title title='' subtitle={item.date}
          left={() => <Avatar.Text size={30} label="TV" />} 
        />
        <Card.Content>
          <View style={{padding: 5, display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}} >
            <TextInput disabled value={item.text} style={{flex: 1, marginRight: 10}} multiline={true}></TextInput>
            <ImageWithViewer source={image} width={60} height={100} ></ImageWithViewer>
          </View>
        </Card.Content>
      </Card>)}
      <Text>New comment</Text>
      <Card mode='outlined' style={{marginTop:10}}>
        <Card.Title title=''
          left={() => <Avatar.Text size={30} label="TV" />} 
          right={() => <View style={{margin: 10}}><IconButton onPress={() => pickImage()} size={30} icon='camera' /></View>}
        />
        <Card.Content>
          <View style={{padding: 5, display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}} >
            <TextInput value={commentText} onChangeText={setCommentText} style={{flex: 1, marginRight: 10}} label={'comment'} multiline={true}></TextInput>
            {image && <ImageWithViewer source={image} width={60} height={100} ></ImageWithViewer>}
          </View>
          <Button mode='contained' onPress={createComment}>Submit</Button>
        </Card.Content>
      </Card>
    </ScrollView>
  );
};