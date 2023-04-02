/* eslint-disable */
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import React, { useState, useRef } from 'react';
import { View, Image, StyleSheet, PanResponder, Dimensions } from 'react-native';
import Svg, { Rect } from 'react-native-svg';
import { ProductStackParamList } from './ProductStackParamList';
import { image } from './test';

interface Rectangle {
  x: number;
  y: number;
  width: number;
  height: number;
}

type Props = NativeStackScreenProps<ProductStackParamList, 'DefectsScreen'>;


export default function DefectsScreen({route, navigation}: Props) {

  const [rectangles, setRectangles] = useState<Rectangle[]>([]);

  const panResponder = useRef(
    PanResponder.create({
      onStartShouldSetPanResponder: () => true,
      onMoveShouldSetPanResponderCapture: () => true,
      onPanResponderGrant: (event) => {
        console.log('??');
        const { locationX, locationY } = event.nativeEvent;
        setRectangles((prevState) => [
          ...prevState,
          {
            x: locationX,
            y: locationY,
            width: 0,
            height: 0,
          },
        ]);
      },
      onPanResponderMove: (event, gestureState) => {
        const { dx, dy } = gestureState;
        const lastIndex = rectangles.length - 1;
        setRectangles((prevState) => {
          const lastRect = prevState[lastIndex];
          const newRect: Rectangle = {
            ...lastRect,
            width: dx,
            height: dy,
          };
          return [
            ...prevState.slice(0, lastIndex),
            newRect,
          ];
        });
      },
    })
  ).current;

  return (
    <View style={styles.container}>
      <View style={styles.imageContainer}>
        {image ? (
          <Image source={{ uri: image }} style={styles.image} {...panResponder.panHandlers} />
        ) : (
          <View style={styles.imagePlaceholder}>
          </View>
        )}
        <Svg style={StyleSheet.absoluteFill}>
          {rectangles.map((rect, index) => (
            <Rect
              key={index}
              x={rect.x}
              y={rect.y}
              width={rect.width}
              height={rect.height}
              stroke="black"
              strokeWidth="2"
              fill="transparent"
            />
          ))}
        </Svg>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF',
  },
  imageContainer: {
    flex: 1,
    alignSelf: 'stretch',
    alignItems: 'center',
    justifyContent: 'center',
  },
  image: {
    flex: 1,
    alignSelf: 'stretch',
  },
  imagePlaceholder: {
    backgroundColor: '#DDD',
    alignSelf: 'stretch',
    alignItems: 'center',
    justifyContent: 'center',
  },
});