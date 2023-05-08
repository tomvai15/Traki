import React from 'react';
import { View } from 'react-native';
import { ActivityIndicator } from 'react-native-paper';

type Props = {
  children?: React.ReactNode,
  loading: boolean,
  marginTop?: number
};

export function Loading ({children, loading, marginTop}: Props) {
  return <View>{loading ?  <ActivityIndicator style={{marginTop: marginTop ?? 10}} animating={true}/> : children}</View>;
}
