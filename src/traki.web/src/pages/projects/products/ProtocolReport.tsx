import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Document, Page } from 'react-pdf/dist/esm/entry.webpack';
import reportService from '../../../services/report-service';
import { Button, Card, CardActions, CardContent, CircularProgress, TextField, Typography } from '@mui/material';
import CreateIcon from '@mui/icons-material/Create';
import { useRecoilState } from 'recoil';
import { userState } from '../../../state/user-state';
import authService from '../../../services/auth-service';
import { useLocation, useParams } from 'react-router-dom';
import { AuthorisationCodeRequest } from '../../../contracts/auth/AuthorisationCodeRequest';
import { SignDocumentRequest } from '../../../contracts/report/SignDocumentRequest';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import protocolService from '../../../services/protocol-service';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { GenerateReportRequest } from '../../../contracts/report/GenerateReportRequest';

export function ProtocolReport() {
  const { projectId, productId, protocolId } = useParams();
  const location = useLocation();

  const [pdfBase64, setPdf] = useState<string>('');
  const [userInfo, setUserInfo] = useRecoilState(userState);
  const [loadingSignIn, setLoadingSignIn] = useState(false);
  const [protocol, setProtocol] = useState<Protocol>();

  useEffect(() => {
    fetchReport();
    fetchProtocol();
  }, []);

  async function fetchReport() {
    const response = await reportService.getReport(Number(protocolId));

    if (!response.exists) {
      await generateReport();
    } else {
      setPdf(response.reportBase64);
    }
  }

  async function generateReport() {
    const generateReportRequest: GenerateReportRequest = {
      reportTitle: ""
    };

    await reportService.generateReport(Number(protocolId), generateReportRequest);
    const response = await reportService.getReport(Number(protocolId));
    setPdf(response.reportBase64);
  }

  async function fetchProtocol() {
    const response = await protocolService.getProtocol(Number(protocolId));
    setProtocol(response.protocol);
  }

  async function signDocument() {
    setLoadingSignIn(true);
    const request: SignDocumentRequest = {
      protocolId: Number(protocolId),
      state: btoa(location.pathname + ':' + protocolId)
    };

    const res = await reportService.signReport(request);
    setLoadingSignIn(false);
    window.location.replace(res);
  }

  async function getCodeUrl() {
    const request: AuthorisationCodeRequest = {
      state: btoa(location.pathname)
    };
    const res = await authService.getAuthorisationCodeUrl(request);
    window.location.replace(res);
  }



  const [numberOfPages, setNumberOfPages] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);

  /*
  function onDocumentLoadSuccess({ numPages }) {
    setNumberOfPages(numPages);
  }*/

  return (
    <Box>
      <Box sx={{flex: 1,  display: 'flex', backgroundColor: (theme) => theme.palette.grey[100], flexDirection: 'row'}}>
        <Box sx={{padding: 3, flex: 2, display: 'flex', flexDirection: 'column', backgroundColor: (theme) => theme.palette.grey[100]}}>
          <Box sx={{flex: 3, padding: 3, display: 'flex',  flexDirection: 'column',  backgroundColor: (theme) => theme.palette.grey[100]}}>
            <Card>
              <CardContent>
                <TextField
                  id="standard-read-only-input"
                  label="Report name"
                  defaultValue='Sample name'
                  InputProps={{
                    readOnly: true,
                  }}
                  variant="standard"/>
              </CardContent>
              {
                protocol && !protocol.isSigned && 
                <CardActions>
                  <Button onClick={generateReport}>
                    Generate report
                  </Button>
                  {userInfo.loggedInDocuSign ?
                    <Button onClick={signDocument} variant="contained" endIcon={ loadingSignIn ? <CircularProgress size={20} color='secondary' /> : <CreateIcon />}>
                    Sign document with DocuSign
                    </Button> :
                    <Button onClick={getCodeUrl} variant="contained" endIcon={<CreateIcon />}>
                    Login to DocuSign
                    </Button>}
                </CardActions>}
            </Card>
          </Box>
        </Box>
        <Box sx={{flex: 1, justifyContent: 'center', display: 'flex', backgroundColor: (theme) => theme.palette.grey[100]}}>
          <Box sx={{borderColor: 'grey', padding: '5px'}}>
            <Button startIcon={<ArrowBackIcon/>} variant='contained' disabled={currentPage==1} onClick={() => setCurrentPage(currentPage-1)}>
              Previous page
            </Button>
            <Button sx={{marginLeft: 2}} endIcon={<ArrowForwardIcon/>} variant='contained' disabled={currentPage==numberOfPages} onClick={() => setCurrentPage(currentPage+1)}>
              Next page
            </Button>
            <Card sx={{backgroundColor: 'grey'}}>
              <Document file={`data:application/pdf;base64,${pdfBase64}`} onLoadSuccess={(i)=> setNumberOfPages(i.numPages)} >
                <Page height={800}  renderTextLayer={false}  pageNumber={currentPage}  renderAnnotationLayer={false}></Page>
              </Document>
            </Card>
          </Box>
        </Box>
      </Box>
    </Box>
  );
}