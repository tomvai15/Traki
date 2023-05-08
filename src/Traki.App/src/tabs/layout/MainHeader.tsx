import React from 'react';
import { View } from 'react-native';
import ProfileMenu from './ProfileMenu';
import NotificationsMenu from './NotificationsMenu';

/* eslint-disable */
type Props = {
  navigation: any
}

export default function MainHeader({navigation}: Props) {
  return (
    <View style={{display: 'flex', flexDirection: 'row'}}>
      <NotificationsMenu  navigation={navigation}/>
      <ProfileMenu navigation={navigation}/>
    </View>
  );
}
/* eslint-disable */