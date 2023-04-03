/* eslint-disable */
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState, useRef, useEffect } from 'react';
import { View, Image, StyleSheet, PanResponder, ScrollView, TouchableHighlight } from 'react-native';
import Svg, { Rect } from 'react-native-svg';
import * as ImagePicker from 'expo-image-picker';
import { DefaultTheme, List, Text, Provider as PaperProvider, Button, TextInput, Title, Portal, Dialog, IconButton } from 'react-native-paper';
import AutoImage from './AutoImage';
import ImageView from "react-native-image-viewing";
import { Rectangle } from './types/Rectangle';

const rect1: Rectangle = {
  x: 0,
  y: 0,
  width: 1,
  height: 1
}

type ImageWithRegionsProps = {
  width?: number,
  height?: number,
  source: string,
  rectangles: Rectangle[]
}

export default function ImageWithRegions({width, height, source, rectangles}: ImageWithRegionsProps) {

  const [transformedRectangles, setTransformedRectangles] = useState<Rectangle[]>([]);

  useEffect(() => {
    Image.getSize(source, (sourceImageWidth, sourceImageHeight) => {
      let newWidth = sourceImageWidth;
      let newHeight = sourceImageHeight;
      if (width == undefined && height != undefined) {
        newWidth = (sourceImageWidth*height)/sourceImageHeight;
        newHeight = height;
      } else if (width != undefined && height == undefined) {
        newWidth = width;
        newHeight = (sourceImageHeight*width)/sourceImageWidth;;
      }

      const newRectangles = rectangles.map((rectangle) => {
        const newRect: Rectangle = {
          x: rectangle.x * newWidth,
          y: rectangle.y * newHeight,
          width: rectangle.width * newWidth,
          height: rectangle.height * newHeight,
        }
        return newRect;
      });

      setTransformedRectangles(newRectangles);
   });
  }, [width, height, rectangles, source]);
  
  return (
    <View elevation={5}>
      <AutoImage
        source={source}
        width={width}
        height={height}
      />
      {transformedRectangles.map((rectangle, index) =>
        <Svg key={index} style={StyleSheet.absoluteFill}>
          <Rect
            x={rectangle.x}
            y={rectangle.y}
            width={rectangle.width}
            height={rectangle.height}
            stroke="black"
            strokeWidth="2"
            fill="transparent"
          />
      </Svg>)}
    </View>
  );
};