import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Breadcrumbs, Button, Card, CardContent, Grid, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material';
import { userService } from 'services';
import { User } from 'contracts/user/User';
import { useNavigate } from 'react-router-dom';

export function UsersPage() {

  const navigate = useNavigate();
  const [users, setUsers] = useState<User[]>([]);

  useEffect(()=> {
    fetchUsers();
  }, []);
 
  async function fetchUsers() {
    const response = await userService.getUsers();
    setUsers(response.users);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <Typography color="text.primary">Users</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={12} md={12}>
        <Card>
          <CardContent>
            <TableContainer>
              <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                  <TableRow>
                    <TableCell align="left">Name</TableCell>
                    <TableCell align="left">Surname</TableCell>
                    <TableCell align="left">Email</TableCell>
                    <TableCell align="left">Role</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {users.map((user, index) => (
                    <TableRow key={index} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                      <TableCell align="left">{user.name}</TableCell>
                      <TableCell align="left">{user.surname}</TableCell>
                      <TableCell align="left">{user.email}</TableCell>
                      <TableCell align="left">{user.role}</TableCell>
                      <TableCell align="center">
                        <Button onClick={()=> navigate(`/admin/users/${user.id}`)}>Details</Button>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </CardContent>
        </Card>
      </Grid>
      <Grid item xs={2} md={2}>
        <Card>
          <CardContent>
            <Button onClick={()=> navigate(`/admin/users/create`)}>Create User</Button>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
}