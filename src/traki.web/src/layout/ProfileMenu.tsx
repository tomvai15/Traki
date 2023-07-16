import * as React from 'react';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import { useNavigate } from 'react-router-dom';
import { Chip, Menu, MenuItem, Stack, useTheme } from '@mui/material';
import { userState } from 'state/user-state';
import { useRecoilState } from 'recoil';
import authService from 'services/auth-service';
import PersonIcon from '@mui/icons-material/Person';

export default function ProfileMenu() {
  const navigate = useNavigate();
  const theme = useTheme();

  const [userInfo, setUserInfo] = useRecoilState(userState);
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);
  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  async function logOut() {
    try {
      await authService.logout();
      setUserInfo({id:-1, loggedInDocuSign: false});
    } catch {
      setUserInfo({id:-1, loggedInDocuSign: false});
    }
    navigate('/login');
  }

  return (
    <Box>
      <Chip 
        sx={{
          height: '30px',
          alignItems: 'center',
          borderRadius: '27px',
          transition: 'all .2s ease-in-out',
          borderColor: theme.palette.primary.light,
          backgroundColor: theme.palette.primary.light,
          '&[aria-controls="menu-list-grow"], &:hover': {
            borderColor: theme.palette.primary.main,
            background: `${theme.palette.grey.A700}!important`,
            color: theme.palette.primary.light,
            '& svg': {
              stroke: theme.palette.primary.light
            }
          },
          '& .MuiChip-label': {
            lineHeight: 0
          }
        }}
        onClick={handleOpenUserMenu}
        label={userInfo.name} 
        icon={<PersonIcon/>}
      />
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
        <Stack>
          <MenuItem key={1}>
            { userInfo.user != undefined && 
              <Typography textAlign="center">
                {userInfo.user.name} {userInfo.user.surname}
              </Typography>}
          </MenuItem>
          <MenuItem key={2} onClick={()=> navigate('my-information')}>
            <Typography textAlign="center">My Information</Typography>
          </MenuItem>
          <MenuItem key={3} onClick={logOut}>
            <Typography color={'error'} textAlign="center">Logout</Typography>
          </MenuItem>
        </Stack>
      </Menu>
    </Box>
  );
}