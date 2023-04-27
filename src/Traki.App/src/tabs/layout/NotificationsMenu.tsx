import React, { useEffect } from 'react';
import { Badge, Divider, IconButton, Menu, Button } from 'react-native-paper';
import { View, StyleSheet  } from 'react-native';
import { useUpdateNotifications } from '../../hooks/useUpdateNotifications';
import { useRecoilState } from 'recoil';
import { notificationsState } from '../../state/notification-state';

export default function NotificationsMenu() {

  const { updateNotifications } = useUpdateNotifications();
  const [notifications, setNotifications] = useRecoilState(notificationsState);

  const [visible, setVisible] = React.useState(false);
  const openMenu = () => setVisible(true);
  const closeMenu = () => setVisible(false);

  useEffect(() => {
    updateNotifications();
  }, []);

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
      <Menu.Item onPress={() => {}} title="No notifications" />
      <Menu.Item onPress={() => {}} title="Item 2" />
      <Divider />
      <Menu.Item onPress={() => {}} title="Item 3" />
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