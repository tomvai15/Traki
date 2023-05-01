import React from 'react';
import { View } from 'react-native';
import { ActivityIndicator } from 'react-native-paper';

type Props = {
  children?: React.ReactNode,
  loading: boolean
};

export function Loading ({children, loading}: Props) {
  return <View>{loading ?  <ActivityIndicator animating={true}/> : children}</View>;
};
