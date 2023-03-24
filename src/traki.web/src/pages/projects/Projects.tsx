import React from 'react';
import Box from '@mui/material/Box';
import { Card, CardContent, Divider, Table, TableBody, TableCell, TableRow, Typography } from '@mui/material';
import BuildCircleOutlinedIcon from '@mui/icons-material/BuildCircleOutlined';

type Project = {
  id: number,
  name: string
}

type Product = {
  id: number,
  name: string
}


const data: Project[] = [{id: 1, name: "Sample project 1"}, {id: 2, name: "Sample project 2"}];
const productData: Product[] = [{id: 1, name: "Sample product 1"}, {id: 2, name: "Sample product 2"}];

export function Projects() {
  return (
    <Box>
      { data.map(item =>
        <Card key={item.id} sx={{marginBottom: 2}} title='Sample Project'>
          <CardContent>
            <Typography variant="h5" component="div">
              {item.name}
            </Typography>
            <Divider/>
            <Table>
              <TableBody>
                {productData.map((row) => (
                  <TableRow
                    key={row.id}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell component="th" scope="row" sx={{display: 'flex', flexDirection: 'row'}}>
                      <BuildCircleOutlinedIcon/>
                      <Typography fontSize={20} sx={{marginLeft: 2}}>
                        {row.name}
                      </Typography>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </CardContent>      
        </Card>
      )}
    </Box>
  );
}