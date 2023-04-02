/* eslint-disable */
import React from 'react';
import { View, Text } from 'react-native';
import { List } from 'react-native-paper';

type CommentIconProps = {
  text: string
}

export function CommentIcon ({text}: CommentIconProps) { 
  return (
  <View style={{display: 'flex', flexDirection: 'row'}}>
    <Text>
      {text}
    </Text>
    <List.Icon icon="comment" />
  </View>);
}
