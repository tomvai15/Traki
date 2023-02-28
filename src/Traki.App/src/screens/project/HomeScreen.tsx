import React from 'react';
import { View, Text, Button } from 'react-native';
import { RootStackParamList } from './RootStackPatamList';
import { NativeStackScreenProps } from '@react-navigation/native-stack';

type Props = NativeStackScreenProps<RootStackParamList, 'Home'>;

export  default function HomeScreen({ navigation }: Props) {
  return (
    <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center' }}>
      <Text>Home Screen</Text>
      <Button
        title="Go to projects"
        onPress={() => navigation.navigate('Projects')}
      />
    </View>
  );
}