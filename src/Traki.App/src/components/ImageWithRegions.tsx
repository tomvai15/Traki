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
import { Defect } from '../contracts/drawing/defect/Defect';

const rect1: Rectangle = {
  x: 0,
  y: 0,
  width: 1,
  height: 1,
  name: ""
}

type ImageWithRegionsProps = {
  width?: number,
  height?: number,
  source: string,
  defects: Defect[]
}

export default function ImageWithRegions({width, height, source, defects}: ImageWithRegionsProps) {

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

      const newRectangles = defects.map((defect) => {
        const newRect: Rectangle = {
          x: defect.x * newWidth * 0.01,
          y: defect.y * newHeight * 0.01,
          width: defect.width * newWidth * 0.01,
          height: defect.height * newHeight * 0.01,
          name: defect.title
        }
        return newRect;
      });

      setTransformedRectangles(newRectangles);
   });
  }, [width, height, defects, source]);

  const textRef = useRef<SVGTextElement | null>(null);
  const props = textRef.current?.getBBox() ?? { width: 0 };
  
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
            <Rect width={rectangle.name.length*12} y={-18} height={20} x={-10} fill="white" opacity={0.4} rx={5} ry={5}/>
            <Text fontStyle='italic' textAnchor='start' x={0} fontSize="16" fill="black">
              {rectangle.name}
            </Text>
          </G>
      </Svg>)}
    </View>
  );
};