import * as React from 'react';
import Box from '@mui/material/Box';
import List from '@mui/material/List';
import Typography from '@mui/material/Typography';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import Badge from '@mui/material/Badge';
import NotificationsIcon from '@mui/icons-material/Notifications';
import { Link, useNavigate } from 'react-router-dom';
import { Card, Grid, ListItem, ListItemSecondaryAction, ListItemText, Menu, Stack, useTheme } from '@mui/material';
import { userState } from 'state/user-state';
import { useRecoilState } from 'recoil';
import { useEffect, useState } from 'react';
import { DefectNotification } from 'contracts/drawing/defect/DefectNotification';
import { notificationService } from 'services';
import PerfectScrollbar from 'react-perfect-scrollbar';
import { useUpdateNotifications } from 'hooks/useUpdateNotifications';
import { notificationsState } from 'state/notification-state';

type NotificationData = {
  projectId: number,
  productId: number,
  drawingId: number,
  defectId: number
}

export default function NotificationsMenu() {

  const navigate = useNavigate();
  const theme = useTheme();
  const { updateNotifications } = useUpdateNotifications();

  const [notifications, setNotifications] = useRecoilState(notificationsState);

  const [userInfo, setUserInfo] = useRecoilState(userState);
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);

  useEffect(() => {
    updateNotifications();
  }, []);

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  function handleNotification(item: DefectNotification) {
    const data: NotificationData = JSON.parse(item.data);
    navigate(`/projects/${data.projectId}/products/${data.productId}/defects`, { state: { defectId: data.defectId }}); 
    handleCloseUserMenu();
  }

  return (
    <Box>
      <IconButton onClick={handleOpenUserMenu} color="inherit">
        <Badge badgeContent={notifications.length} color="secondary">
          <NotificationsIcon />
        </Badge>
      </IconButton>
      <Menu
        sx={{ mt: '45px' }}
        id="menu-appbar"
        anchorEl={anchorElUser}
        anchorOrigin={{
          vertical: 'top',
          horizontal: 'right',
        }}
        keepMounted
        transformOrigin={{
          vertical: 'top',
          horizontal: 'right',
        }}
        open={Boolean(anchorElUser)}
        onClose={handleCloseUserMenu}
      >
        <Card>
          <Grid container direction="column" spacing={2}>
            <Grid item xs={12}>
              <Grid container alignItems="center" justifyContent="space-between" spacing={2} sx={{ pt: 2, px: 2 }}>
                <Grid item>
                  <Stack direction="row" spacing={2}>
                    <Typography variant="subtitle1">All Notification</Typography>
                  </Stack>
                </Grid>
                <Grid item>
                  <Typography component={Link} to="#" variant="subtitle2" color="primary">
                    Mark as all read
                  </Typography>
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
              <PerfectScrollbar
                style={{ height: '100%', maxHeight: 'calc(100vh - 205px)', overflowX: 'hidden' }}
              >
                <Grid container direction="column" spacing={2}>
                  <Grid item xs={12} p={0}>
                    <Divider sx={{ my: 0 }} />
                  </Grid>
                </Grid>
              </PerfectScrollbar>
            </Grid>
          </Grid>
          <Divider />
          <List 
            sx={{
              width: '100%',
              maxWidth: 330,
              py: 0,
              borderRadius: '10px',
              [theme.breakpoints.down('md')]: {
                maxWidth: 300
              },
              '& .MuiListItemSecondaryAction-root': {
                top: 22
              },
              '& .MuiDivider-root': {
                my: 0
              },
              '& .list-container': {
                pl: 7
              }
            }}>
            {notifications.length == 0 && <>
              <ListItem alignItems="center">
                <ListItemText primary="No notifications" />
              </ListItem>
            </>}
            {notifications.map((item, index) => 
              <Box component="button" key={index} onClick={() => handleNotification(item)}>
                <ListItem alignItems="center">
                  <ListItemText primary={item.title} />
                  <ListItemSecondaryAction>
                    <Grid container justifyContent="flex-end">
                      <Grid item xs={12}>
                        <Typography variant="caption" display="block" gutterBottom>
                          1 min ago
                        </Typography>
                      </Grid>
                    </Grid>
                  </ListItemSecondaryAction>
                </ListItem>
                <ListItem alignItems="center">
                  <Typography variant="subtitle2">{item.body}</Typography>
                </ListItem>
                <Divider />
              </Box>)}
          </List>
        </Card>
      </Menu>
    </Box>
  );
}