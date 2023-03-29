import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Document, Page } from 'react-pdf/dist/esm/entry.webpack';
import reportService from '../../../services/report-service';
import { Button, Card, CardActions, CircularProgress, Typography } from '@mui/material';
import CreateIcon from '@mui/icons-material/Create';
import { useRecoilState } from 'recoil';
import { userState } from '../../../state/user-state';
import authService from '../../../services/auth-service';
import { useLocation, useParams } from 'react-router-dom';
import { AuthorisationCodeRequest } from '../../../contracts/auth/AuthorisationCodeRequest';
import { SignDocumentRequest } from '../../../contracts/report/SignDocumentRequest';

export function ProtocolReport() {
  const { projectId, productId, protocolId } = useParams();
  const location = useLocation();

  const [pdfBase64, setPdf] = useState<string>('');
  const [userInfo, setUserInfo] = useRecoilState(userState);
  const [loadingSignIn, setLoadingSignIn] = useState(false);

  useEffect(() => {
    console.log('??' + location.pathname);
    reportService.getReport(Number(protocolId)).then((value) => {
      setPdf(value);
    });
  }, []);

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
              <CardActions>
                {userInfo.loggedInDocuSign ?
                  <Button onClick={signDocument} variant="contained" endIcon={ loadingSignIn ? <CircularProgress size={20} color='secondary' /> : <CreateIcon />}>
                  Sign document with DocuSign
                  </Button> :
                  <Button onClick={getCodeUrl} variant="contained" endIcon={<CreateIcon />}>
                  Log in to DocuSign
                  </Button>}
              </CardActions>
            </Card>
          </Box>
        </Box>
        <Box sx={{flex: 1, justifyContent: 'center', display: 'flex', backgroundColor: (theme) => theme.palette.grey[100]}}>
          <Box sx={{borderColor: 'grey', padding: '5px'}}>
            <Button variant='contained' disabled={currentPage==1} onClick={() => setCurrentPage(currentPage-1)}>
              Previous page
            </Button>
            <Button variant='contained' disabled={currentPage==numberOfPages} onClick={() => setCurrentPage(currentPage+1)}>
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