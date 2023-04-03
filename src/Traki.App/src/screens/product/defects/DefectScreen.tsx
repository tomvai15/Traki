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
import ImageWithViewer from '../../../components/ImageWithViewer';

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

const defects = [1,2,3,4,5,6];

export default function DefectScreen({route, navigation}: Props) {

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
    <ScrollView>
      <Title>Defects</Title>
      <Card mode='outlined' style={{marginTop:10}}>
        <Card.Title title="Missing something" 
          subtitle="saddsasadsda" 
          left={() => <Avatar.Text size={50} label="TV" />} 
          right={() => <View style={{margin: 10}}><ImageWithViewer source={image} width={60} height={100} ></ImageWithViewer></View>}
        />
      </Card>
      <View style={{ marginVertical: 10 }}>
        <TouchableHighlight style={{margin: 5}}>
          <AutoImage
            source={image}
            height={300}
          />
        </TouchableHighlight >
      </View>
      <Text>Comments</Text>
      <Card mode='outlined' style={{marginTop:10}}>
        <Card.Title title=''
          left={() => <Avatar.Text size={30} label="TV" />} 
        />
        <Card.Content>
          <View style={{padding: 5, display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}} >
            <TextInput disabled value='Missing something' style={{flex: 1, marginRight: 10}} multiline={true}></TextInput>
            <ImageWithViewer source={image} width={60} height={100} ></ImageWithViewer>
          </View>
        </Card.Content>
      </Card>
      <Text>New comment</Text>
      <Card mode='outlined' style={{marginTop:10}}>
        <Card.Title title=''
          left={() => <Avatar.Text size={30} label="TV" />} 
          right={() => <View style={{margin: 10}}><IconButton onPress={() => pickImage()} size={30} icon='camera' /></View>}
        />
        <Card.Content>
          <View style={{padding: 5, display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}} >
            <TextInput style={{flex: 1, marginRight: 10}} label={'comment'} multiline={true}></TextInput>
            {image && <ImageWithViewer source={image} width={60} height={100} ></ImageWithViewer>}
          </View>
          <Button mode='contained' onPress={() => console.log()}>Submit</Button>
        </Card.Content>
      </Card>
    </ScrollView>
  );
};