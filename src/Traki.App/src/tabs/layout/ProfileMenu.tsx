import React from 'react';
import { Badge, Divider, IconButton, Menu, Button } from 'react-native-paper';
import { View, StyleSheet  } from 'react-native';
import { useUserInformation } from '../../hooks/useUserInformation';


type Props = {
  navigation: any
}
export default function ProfileMenu({navigation}: Props) {

  const {clearToken} = useUserInformation();

  const [visible, setVisible] = React.useState(false);
  const openMenu = () => setVisible(true);
  const closeMenu = () => setVisible(false);

  return (
    <Menu
      anchorPosition='bottom'
      visible={visible}
      onDismiss={closeMenu}
      anchor={
        <View style={styles.item}>
          <IconButton onPress={openMenu} icon="account" style={styles.button} />
        </View>
      }>
      <Menu.Item onPress={() => navigation.navigate('Account information', {screen: 'UserScreen'})} title="My information" />
      <Divider />
      <Menu.Item contentStyle={{borderColor: 'red'}} onPress={() => clearToken()} title='Logout' />
    </Menu>
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