/* eslint-disable */
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState, useRef, useEffect } from 'react';
import { View, Image, StyleSheet, PanResponder, ScrollView, TouchableHighlight } from 'react-native';
import Svg, { G, Rect, Text } from 'react-native-svg';
import * as ImagePicker from 'expo-image-picker';
import { DefaultTheme, List, Provider as PaperProvider, Button, TextInput, Title, Portal, Dialog, IconButton } from 'react-native-paper';
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
          x: rectangle.x * newWidth * 0.01,
          y: rectangle.y * newHeight * 0.01,
          width: rectangle.width * newWidth * 0.01,
          height: rectangle.height * newHeight * 0.01,
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
          <G  x={rectangle.x}
              y={rectangle.y}>
            <Rect
              rx="5"
              width={rectangle.width}
              height={rectangle.height}
              stroke="black"
              strokeWidth="2"
              fill="transparent"
            />
            <Text x={50} fontStyle='italic' textAnchor='end' fontSize="16" fill="black">
              Defect 1
            </Text>
          </G>
      </Svg>)}
    </View>
  );
};