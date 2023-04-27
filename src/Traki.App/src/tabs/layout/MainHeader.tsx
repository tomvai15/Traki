import React from 'react';
import { Badge, Divider, IconButton, Menu, Button } from 'react-native-paper';
import { View, StyleSheet  } from 'react-native';
import ProfileMenu from './ProfileMenu';
import NotificationsMenu from './NotificationsMenu';


type Props = {
  navigation: any
}

export default function MainHeader({navigation}: Props) {

  const [visible, setVisible] = React.useState(false);
  const openMenu = () => setVisible(true);
  const closeMenu = () => setVisible(false);

  return (
    <View style={{display: 'flex', flexDirection: 'row'}}>
      <NotificationsMenu />
      <ProfileMenu navigation={navigation}/>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  row: {
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  item: {
    margin: 0,
  },
  button: {
    opacity: 0.6,
  },
  badge: {
    position: 'absolute',
    top: 4,
    right: 0,
  },
  label: {
    flex: 1,
  },
});