import React, { useEffect } from 'react';
import { Grid, Card, CardContent, Typography } from '@mui/material';

export function HomePage() {

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant='h5'>Recent Products</Typography>
          </CardContent>    
        </Card>
      </Grid>
      <Grid item xs={8} md={8} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant='h6'>SH.1 / 01.2.21.1.0016</Typography>
          </CardContent>    
        </Card>
      </Grid>
      <Grid item xs={8} md={8} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant='h6'>SH.2 / 01.2.21.1.0016 GT</Typography>
          </CardContent>    
        </Card>
      </Grid>
    </Grid>
  );
}