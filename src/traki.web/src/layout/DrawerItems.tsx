import * as React from 'react';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import DashboardIcon from '@mui/icons-material/Dashboard';
import { useNavigate } from 'react-router-dom';
import AssignmentIcon from '@mui/icons-material/Assignment';
import BusinessIcon from '@mui/icons-material/Business';
import { Divider } from '@mui/material';
import HomeIcon from '@mui/icons-material/Home';
import GroupIcon from '@mui/icons-material/Group';
import { ProtectedComponent } from 'components/ProtectedComponent';

export default function DrawerItems() {

  const navigate = useNavigate();

  return (
    <React.Fragment>
      <ProtectedComponent role={['ProjectManager', 'ProductManager']}>
        <ListItemButton id='home-drawer' onClick={() => navigate('home')}>
          <ListItemIcon>
            <HomeIcon />
          </ListItemIcon>
          <ListItemText primary="Home" />
        </ListItemButton>
        <ListItemButton id='projects-drawer' onClick={() => navigate('projects')}>
          <ListItemIcon>
            <DashboardIcon />
          </ListItemIcon>
          <ListItemText primary="Projects" />
        </ListItemButton>
        <ListItemButton id='protocols-drawer' onClick={() => navigate('/templates/protocols')}>
          <ListItemIcon>
            <AssignmentIcon />
          </ListItemIcon>
          <ListItemText primary="Templates" />
        </ListItemButton>
        <Divider/>
      </ProtectedComponent>
      <ProtectedComponent role={['Administrator']}>
        <ListItemButton onClick={() => navigate('company')}>
          <ListItemIcon>
            <BusinessIcon />
          </ListItemIcon>
          <ListItemText primary="Company" />
        </ListItemButton>
        <ListItemButton onClick={() => navigate('/admin/users')}>
          <ListItemIcon>
            <GroupIcon />
          </ListItemIcon>
          <ListItemText primary="Users" />
        </ListItemButton>
        <Divider/>
      </ProtectedComponent>
    </React.Fragment>
  );
}