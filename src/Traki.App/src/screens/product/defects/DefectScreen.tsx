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
import ImageWithViewer from '../../../components/ImageWithViewer';
import { Drawing } from '../../../contracts/drawing/Drawing';
import { Defect } from '../../../contracts/drawing/defect/Defect';
import drawingService from '../../../services/drawing-service';
import defectService from '../../../services/defect-service';
import pictureService from '../../../services/picture-service';
import ImageWithRegions from '../../../components/ImageWithRegions';
import { DefectComment } from '../../../contracts/drawing/defect/DefectComment';
import { CreateDefectCommentRequest } from '../../../contracts/drawing/defect/CreateDefectCommentRequest';
import uuid from 'react-native-uuid';
import { ScreenView } from '../../../components/ScreenView';

interface Rectangle {
  x: number;
  y: number;
  width: number;
  height: number;
}


type Props = NativeStackScreenProps<ProductStackParamList, 'DefectScreen'>;

type DrawingWithImage = {
  drawing: Drawing,
  imageBase64: string
}

type DefectWithImage = {
  defect: Defect,
  imageBase64: string
}

type CommentWithImage = {
  defectComment: DefectComment,
  imageBase64: string
}

export default function DefectScreen({route, navigation}: Props) {
  const {productId, drawingId, defectId} = route.params;
  const [drawing, setDrawing] = useState<DrawingWithImage>();
  const [defect, setDefect] = useState<DefectWithImage>();

  const [comments, setComments] = useState<CommentWithImage[]>([]);

  const [commentText, setCommentText] = useState<string>('');

  useEffect(() => {
    fetchDefect();
    fetchDrawing();
  }, []);

  async function fetchDefect() {
    const response = await defectService.getDefect(drawingId, defectId);
    let imageBase64 = '';
    if (response.defect.imageName != '') {
      imageBase64 = await pictureService.getPicture('item', response.defect.imageName);
    } 
    const defectWithImage: DefectWithImage = {
      defect: response.defect,
      imageBase64: imageBase64
    }
    setDefect(defectWithImage);
    if (response.defect.defectComments) {
      await fetchComments(response.defect.defectComments);
    }
  }

  async function fetchDrawing() {
    const response = await drawingService.getDrawing(productId, drawingId);
    const imageBase64 = await pictureService.getPicture('company', response.drawing.imageName);
    const newDrawingImage: DrawingWithImage = {drawing: response.drawing, imageBase64: imageBase64};
    setDrawing(newDrawingImage);
  }

  async function fetchComments(defectComments: DefectComment[]) {
    
    const commentsWithImage: CommentWithImage [] = [];

    for (let i = 0; i < defectComments.length; i++) {
      let imageBase64 = '';
      if (defectComments[i].imageName != '') {
        imageBase64 = await pictureService.getPicture('item', defectComments[i].imageName);
      }
      const newCommentWithImage: CommentWithImage = {
        defectComment: defectComments[i], 
        imageBase64: imageBase64
      };

      commentsWithImage.push(newCommentWithImage);
    }

    setComments(commentsWithImage);
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

  async function uploadImage(imageUri: string, pictureName: string) {
    let filename = imageUri.split('/').pop() ?? '';
    let match = /\.(\w+)$/.exec(filename);
    let type = match ? `image/${match[1]}` : `image`;

    let formData = new FormData();
    formData.append('photo', JSON.parse(JSON.stringify({ uri: imageUri, name: pictureName, type })));
    await pictureService.uploadPicturesFormData('item', formData)
  }

  async function createComment() {
    if (!defect) {
      return;
    }

    let pictureName = '';
    if (imageUri != '') {
      pictureName = `${uuid.v4().toString()}.jpeg`;
      await uploadImage(imageUri, pictureName);
    }

    const defectComment: DefectComment = {
      id: 0,
      text: commentText,
      imageName: pictureName,
      date: '',
    };

    const request: CreateDefectCommentRequest = {
      defectComment: defectComment
    };

    await defectService.createDefectComment(drawingId, defectId, request);


    const response = await defectService.getDefect(drawingId, defectId);
    const defectWithImage: DefectWithImage = {
      defect: response.defect,
      imageBase64: defect?.imageBase64
    }
    setDefect(defectWithImage);
    if (response.defect.defectComments) {
      await fetchComments(response.defect.defectComments);
    }

    setImageUri('');
    setCommentText('');
  }

  return (
    <ScrollView>
      <ScreenView>
        <Card mode='outlined' style={{marginTop:10}}>
          <Card.Title title={defect?.defect.title}
            subtitle={defect?.defect.description} 
            left={() => <Avatar.Text size={50} label="TV" />} 
            right={() => { return (defect && defect.imageBase64!= '') ? <View style={{margin: 10}}><ImageWithViewer source={defect?.imageBase64} width={60} height={100} ></ImageWithViewer></View> : <View></View>}}
          />
        </Card>
        <View style={styles.box}>
          { defect && drawing && <ImageWithRegions source={drawing.imageBase64} width={390} rectangles={[defectToRectangle(defect.defect)]}/>}
        </View>
        <Text>New comment</Text>
        <Card mode='outlined' style={{marginTop:10}}>
          <Card.Title title=''
            left={() => <Avatar.Text size={30} label="TV" />} 
            right={() => <View style={{margin: 10}}><IconButton onPress={() => pickImage()} size={30} icon='camera' /></View>}
          />
          <Card.Content>
            <View style={{padding: 5, display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}} >
              <TextInput value={commentText} onChangeText={setCommentText} style={{flex: 1, marginRight: 10}} label={'Comment...'} multiline={true}></TextInput>
              {imageUri && <ImageWithViewer source={imageUri} width={60} height={100} ></ImageWithViewer>}
            </View>
            <Button disabled={commentText==''} mode='contained' onPress={createComment}>Submit</Button>
          </Card.Content>
        </Card>
        <Text>Comments</Text>
        { comments.map((item, index) => <Card key={index} mode='outlined' style={{marginTop:10}}>
          <Card.Title title='' subtitle={item.defectComment.date}
            left={() => <Avatar.Text size={30} label="TV" />} 
          />
          <Card.Content>
            <View style={{padding: 5, display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}} >
              <TextInput editable = {false} value={item.defectComment.text} style={{flex: 1, marginRight: 10, backgroundColor: 'white', borderTopColor: 'grey', borderTopWidth: 1, borderBottomColor: 'grey', borderBottomWidth: 0}} multiline={true}></TextInput>
              { item.imageBase64 != '' && <ImageWithViewer source={item.imageBase64} width={60} height={100} ></ImageWithViewer>}
            </View>
          </Card.Content>
        </Card>)}
      </ScreenView>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  box: {
    // ...
    marginVertical: 10,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.5,
    shadowRadius: 2,
    elevation: 2,
  },
})