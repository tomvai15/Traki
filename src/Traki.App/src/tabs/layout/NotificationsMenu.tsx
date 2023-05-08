import React, { useEffect } from 'react';
import { Badge, Divider, IconButton, Menu } from 'react-native-paper';
import { View, StyleSheet, Text, Pressable  } from 'react-native';
import { useUpdateNotifications } from '../../hooks/useUpdateNotifications';
import { useRecoilState } from 'recoil';
import { notificationsState } from '../../state/notification-state';
import { DefectNotification } from '../../contracts/drawing/defect/DefectNotification';

type NotificationData = {
  projectId: number,
  productId: number,
  drawingId: number,
  defectId: number
}

/* eslint-disable */
type Props = {
  navigation: any
}

export default function NotificationsMenu({navigation}: Props) {

  const { updateNotifications } = useUpdateNotifications();
  const [notifications] = useRecoilState(notificationsState);

  const [visible, setVisible] = React.useState(false);
  const openMenu = () => setVisible(true);
  const closeMenu = () => setVisible(false);

  useEffect(() => {
    updateNotifications();
  }, []);

  function handleNotification(item: DefectNotification) {
    const data: NotificationData = JSON.parse(item.data);
    console.log(data);

    navigation.navigate('Projects', { screen: 'DefectScreen', params: { productId: data.productId, drawingId: data.drawingId, defectId: data.defectId}}); 
    closeMenu();
  }

  return (
    <Menu
      anchorPosition='bottom'
      visible={visible}
      onDismiss={closeMenu}
      anchor={
        <View style={styles.item}>
          <IconButton onPress={openMenu} icon="bell" style={styles.button} />
          <Badge size={20} visible={notifications.length != 0} style={styles.badge}>
            {notifications.length}
          </Badge>
        </View>
      }>
      <Menu.Item title="Notifications" />
      {notifications.length == 0 ?
        <Menu.Item onPress={() => {}} title="No notifications" /> :
        notifications.map((item, index) => 
          <Pressable key={index} onPress={() => handleNotification(item)}
            style={({pressed}) => [
              {
                backgroundColor: pressed ? 'rgb(210, 230, 255)' : 'white',
              },
            ]}>
          
            <View style={{width: 200, paddingHorizontal: 15, marginVertical: 5}}>
              <Text style={{fontSize: 18}}>{item.title}</Text>
              <Text style={{fontSize: 15}}>{item.body}</Text>
            </View>
            <Divider/>
          </Pressable>)}
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
/* eslint-disable */