import React from 'react';
import { View } from 'react-native';

type Props = {
  children?: React.ReactNode
};

export const ScreenView: React.FC<Props> = ({children}) => {
  return <View style={{padding: 10}}>{children}</View>;
};
