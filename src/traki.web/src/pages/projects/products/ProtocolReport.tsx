import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import CreateIcon from '@mui/icons-material/Create';
import { Button, Card, CardActions, CardContent, Checkbox, CircularProgress, Divider, FormControlLabel, IconButton, Stack, TextField, Tooltip, Typography } from '@mui/material';
import Box from '@mui/material/Box';
import React, { useEffect, useState } from 'react';
import { Document, Page } from 'react-pdf/dist/esm/entry.webpack';
import { useLocation, useParams } from 'react-router-dom';
import { useRecoilState } from 'recoil';
import { AuthorisationCodeRequest } from '../../../contracts/auth/AuthorisationCodeRequest';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { GenerateReportRequest } from '../../../contracts/report/GenerateReportRequest';
import { SignDocumentRequest } from '../../../contracts/report/SignDocumentRequest';
import authService from '../../../services/auth-service';
import protocolService from '../../../services/protocol-service';
import reportService from '../../../services/report-service';
import { userState } from '../../../state/user-state';
import { saveAs } from 'file-saver';
import DownloadIcon from '@mui/icons-material/Download';
import InfoIcon from '@mui/icons-material/Info';
import InputIcon from '@mui/icons-material/Input';
import { sectionService } from 'services';
import { Section } from 'contracts/protocol';

type SectionWithFlag = {
  section: Section,
  include: boolean
}

export function ProtocolReport() {
  const { projectId, productId, protocolId } = useParams();
  const location = useLocation();

  const [reportName, setReportName] = useState<string>('');
  const [useColors, setUseColors] = useState<boolean>(true);

  const [pdfBase64, setPdf] = useState<string>('');
  const [userInfo, setUserInfo] = useRecoilState(userState);
  const [loadingSignIn, setLoadingSignIn] = useState(false);
  const [protocol, setProtocol] = useState<Protocol>();

  const [sections, setSections] = useState<SectionWithFlag[]>([]);

  useEffect(() => {
    fetchReport();
    fetchProtocol();
    fetchSections();
  }, []);

  async function fetchReport() {
    const response = await reportService.getReport(Number(protocolId));

    if (!response.exists) {
      await generateReport();
    } else {
      setPdf(response.reportBase64);
    }
  }

  async function fetchSections() {
    const getSectionsResponse = await sectionService.getSections(Number(protocolId));
    console.log(getSectionsResponse.sections);
    setSections(getSectionsResponse.sections.map((item): SectionWithFlag => {
      return {section: item, include: true};
    }));
  }

  async function generateReport() {
    const generateReportRequest: GenerateReportRequest = {
      reportTitle: reportName,
      useColors: useColors,
      sectionsToNotInclude: sections.filter(x => x.include==false).map(x=> x.section.id)
    };

    console.log(generateReportRequest);

    await reportService.generateReport(Number(protocolId), generateReportRequest);
    const response = await reportService.getReport(Number(protocolId));
    setPdf(response.reportBase64);
  }

  async function fetchProtocol() {
    const response = await protocolService.getProtocol(Number(protocolId));
    setProtocol(response.protocol);
    setReportName(response.protocol.name);
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

  async function  downloadPDF () 
  {
    const { data } = await reportService.getReportPdf(Number(protocolId));
    console.log(data);
    const blob = new Blob([data], { type: 'application/pdf' });
    saveAs(blob, "ataskaita.pdf");
  }

  async function  updateSectionWithFlag(id: number, include: boolean) {

    setSections(sections.map((item) => item.section.id == id ? {...item, include: include} : item ));
  }

  return (
    <Box>
      <Box sx={{flex: 1,  display: 'flex', backgroundColor: (theme) => theme.palette.grey[100], flexDirection: 'row'}}>
        <Box sx={{padding: 3, flex: 2, display: 'flex', flexDirection: 'column', backgroundColor: (theme) => theme.palette.grey[100]}}>
          <Box sx={{flex: 3, padding: 3, display: 'flex',  flexDirection: 'column',  backgroundColor: (theme) => theme.palette.grey[100]}}>
            <Card>
              { !(protocol && !protocol.isSigned) && 
                  <CardContent sx={{paddingTop: 0}}>
                    <Tooltip title="Download report" placement="top">
                      <IconButton onClick={downloadPDF}>
                        <DownloadIcon />
                      </IconButton>
                    </Tooltip>
                  </CardContent>}
              {
                protocol && !protocol.isSigned && 
                <Box>
                  <CardContent sx={{paddingBottom: 0}}>
                    <Stack direction={'column'}>
                      <TextField
                        id="standard-read-only-input"
                        label="Report name"
                        value={reportName}
                        onChange={(e)=>setReportName(e.target.value)}
                        variant="standard"/>
                      <Divider/>
                      <Typography variant='caption'>Sections</Typography>
                      {sections.map((item, index) => 
                        <FormControlLabel key={index} control={<Checkbox checked={item.include} onChange={(e)=> updateSectionWithFlag(item.section.id, e.target.checked)} />} label={item.section.name} />)}
                      <Divider sx={{marginTop: '10px'}}/>
                      <Typography variant='caption'>Visuals</Typography>
                      <FormControlLabel control={<Checkbox checked={useColors} onChange={(e)=> setUseColors(e.target.checked)} />} label="Use colors" />
                    </Stack>
                  </CardContent>
                  <CardContent sx={{paddingTop: 0}}>
                    <Stack direction={'row'} justifyContent={'space-between'} sx={{marginBottom: '10px'}}>
                      <Button onClick={generateReport}>
                        Regenerate report
                      </Button>
                      <Tooltip title="Download report" placement="top">
                        <IconButton onClick={downloadPDF}>
                          <DownloadIcon />
                        </IconButton>
                      </Tooltip>
                    </Stack>
                    <Divider/>
                    <Card color='secondary' sx={{marginTop: '20px'}}>
                      <Stack direction={'column'} spacing={1}>
                        <Stack direction={'row'} spacing={1}>
                          <Button sx={{width: '100%'}} disabled={!userInfo.loggedInDocuSign} onClick={signDocument} variant="contained" endIcon={ loadingSignIn ? <CircularProgress size={20} color='secondary' /> : <CreateIcon />}>
                            Sign document with DocuSign
                          </Button> 
                          { !userInfo.loggedInDocuSign && <Tooltip title="You need to sign to DocuSign first" placement="top">
                            <IconButton>
                              <InfoIcon />
                            </IconButton>
                          </Tooltip>}
                        </Stack>
                        { !userInfo.loggedInDocuSign && <Button onClick={getCodeUrl} variant="contained" endIcon={<InputIcon />}>
                          Login to DocuSign
                        </Button>}
                      </Stack>
                    </Card>
                  </CardContent>
                </Box>}
            </Card>
          </Box>
        </Box>
        <Box sx={{flex: 1, justifyContent: 'center', display: 'flex', backgroundColor: (theme) => theme.palette.grey[100]}}>
          <Box sx={{borderColor: 'grey', padding: '5px'}}>
            <Stack sx={{marginBottom: '5px'}} spacing={1} direction={'row'} justifyContent={'center'}>
              <Button startIcon={<ArrowBackIcon/>} variant='contained' disabled={currentPage==1} onClick={() => setCurrentPage(currentPage-1)}>
                Previous page
              </Button>
              <Button endIcon={<ArrowForwardIcon/>} variant='contained' disabled={currentPage==numberOfPages} onClick={() => setCurrentPage(currentPage+1)}>
                Next page
              </Button>
            </Stack>
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