import React, { useEffect } from 'react';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Container from '@mui/material/Container';
import { Document, Page } from 'react-pdf/dist/esm/entry.webpack';
import reportService from '../services/report-service';
import { Button } from '@mui/material';

function DashboardContent() {

  const [pdf, setPdf] = React.useState<string>('');

  useEffect(() => {
    reportService.getReport().then((value) => {setPdf(value);});
  }, []);

  async function signDocument() {
    const res = await reportService.signReport();
    console.log(res);
    window.location.replace(res);
  }

  return (
    <Box
      component="main"
      sx={{
        backgroundColor: (theme) =>
          theme.palette.mode === 'light'
            ? theme.palette.grey[100]
            : theme.palette.grey[900],
        flexGrow: 1,
        height: '100vh',
      }}
    >
      <Toolbar />
      <Container maxWidth="lg" sx={{ mt: 4}}>
        <Document  file={`data:application/pdf;base64,${pdf}`} >
          <Page height={600} pageNumber={1}></Page>
        </Document>
        <Button onClick={signDocument} variant='contained'>Sign</Button>
      </Container>
    </Box>
  );
}

export default function Dashboard() {
  return <DashboardContent />;
}