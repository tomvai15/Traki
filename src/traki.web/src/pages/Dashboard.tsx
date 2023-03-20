import React, { useEffect } from 'react';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Container from '@mui/material/Container';
import { Document, Page } from 'react-pdf/dist/esm/entry.webpack';
import reportService from '../services/report-service';
import { Button, Card, CircularProgress, IconButton, Typography } from '@mui/material';
import CreateIcon from '@mui/icons-material/Create';
import { useRecoilState } from 'recoil';
import { userState } from '../state/user-state';
import authService from '../services/auth-service';

function DashboardContent() {

  const [pdf, setPdf] = React.useState<string>('');
  const [userInfo, setUserInfo] = useRecoilState(userState);

  useEffect(() => {
    reportService.getReport().then((value) => {setPdf(value);});
  }, []);

  async function signDocument() {
    const res = await reportService.signReport();
    console.log(res);
    window.location.replace(res);
  }

  async function getCodeUrl() {
    const res = await authService.getAuthorisationCodeUrl();
    window.location.replace(res);
  }

  return (
    <Box component="main" sx={{
      flexGrow: 1,
      height: '100vh',
      display: 'flex', 
      flexDirection: 'column'
    }}>
      <Box sx={{height: 60,  backgroundColor: 'red'}}>
        <Typography>Handling oauth</Typography>
      </Box>
      <Box sx={{flex: 1, padding: 2,  display: 'flex', backgroundColor: (theme) => theme.palette.grey[100], flexDirection: 'row'}}>
        <Box sx={{flex: 1, padding: 5, justifyContent: 'center', display: 'flex', backgroundColor: (theme) => theme.palette.grey[100]}}>
          <Box sx={{backgroundColor: (theme) => theme.palette.grey[100]}}>
            <Card sx={{ padding: 10}}>
              <Document  file={`data:application/pdf;base64,${pdf}`} >
                <Page height={500}  renderTextLayer={false}  pageNumber={1}  renderAnnotationLayer={false}></Page>
              </Document>
            </Card>
          </Box>
        </Box>
        <Box sx={{padding: 3, flex: 2, display: 'flex', flexDirection: 'column', backgroundColor: (theme) => theme.palette.grey[100]}}>
          <Box sx={{flex: 3, padding: 3, display: 'flex',  backgroundColor: (theme) => theme.palette.grey[100]}}>
            <Card sx={{flex: 1}}>
              <Typography>Test</Typography>
            </Card>
          </Box>
          <Box sx={{flex: 1, padding: 3, display: 'flex', backgroundColor: (theme) => theme.palette.grey[100]}}>
            <Card sx={{flex: 1, padding: 5}}>{userInfo.loggedInDocuSign ?
              <Button onClick={signDocument} variant="contained" endIcon={<CreateIcon />}>
                Sign document with DocuSign
              </Button> :
              <Button onClick={getCodeUrl} variant="contained" endIcon={<CreateIcon />}>
                Log in to DocuSign
              </Button>}
            </Card>
          </Box>
        </Box>
      </Box>
    </Box>
  );

  /*
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
  );*/
}

export default function Dashboard() {
  return <DashboardContent />;
}