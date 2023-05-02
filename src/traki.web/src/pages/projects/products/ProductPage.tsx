import { Box, Link as BreadLink, Breadcrumbs, Button, Card, CardActions, CardContent, CardHeader, Chip, Divider, Grid, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Product } from '../../../contracts/product/Product';
import { Protocol } from '../../../contracts/protocol/Protocol';
import productService from '../../../services/product-service';
import { DrawingDefectsViewer } from 'features/products/components/DrawingDefectsViewer';
import AddProtocolDialog from 'features/products/components/AddProtocolDialog';
import { useRecoilState } from 'recoil';
import { pageState } from 'state/page-state';
import { DeleteItemDialog } from 'components/DeleteItemDialog';
import { protocolService } from 'services';
import { Defect } from 'contracts/drawing/defect/Defect';
import { formatDate } from 'utils/dateHelpers';
import { AuthorBar } from 'components/AuthorBar';
import AssignmentIcon from '@mui/icons-material/Assignment';
import WarningIcon from '@mui/icons-material/Warning';
import { DefectStatus } from 'contracts/drawing/defect/DefectStatus';

const initialProduct: Product = {
  id: 0,
  name: '',
  projectId: 0,
  status,
  creationDate: ''
};

const emails = ['username@gmail.com', 'user02@gmail.com'];

export function ProductPage() {
  const navigate = useNavigate();
  const { projectId, productId } = useParams();
  const [product, setProduct] = useState<Product>();

  const [page, setPageState] = useRecoilState(pageState);

  const [protocols, setProtocols] = useState<Protocol[]>([]);

  const [open, setOpen] = useState(false);
  const [selectedValue, setSelectedValue] = useState(emails[1]);


  const [openProductDialog, setOpenProductDialog] = useState(false);
  const [selectedProtocol, setSelectedProtocol] = useState<Protocol>();

  const handleProductDialogClose = () => {
    setOpenProductDialog(false);
  };

  const handleClickOpen = () => {
    setOpen(true);
  };

  const closeDialog = () => {
    setOpen(false);
  };

  useEffect(() => {
    notFoundCatcher(fetchProduct);
  }, []);


  async function notFoundCatcher(func: () => Promise<void>): Promise<void> {
    try {
      await func();
    } catch (err) {
      setPageState({...page, notFound: true});
      console.log(err);
    }
  }

  async function fetchProduct() {
    const getProductResponse = await productService.getProduct(Number(projectId), Number(productId));
    setProduct(getProductResponse.product);
    const getProductProtocolsResponse = await productService.getProtocols(Number(projectId), Number(productId));
    setProtocols(getProductProtocolsResponse.protocols);
  }

  async function addProtocol(protocolId: number) {
    await productService.addProtocol(Number(projectId), Number(productId), protocolId);
    await fetchProduct();
  }

  const handleClose = (protocolId: number) => {
    addProtocol(protocolId);
    closeDialog();
  };

  async function deleteProtocol() {
    if (!selectedProtocol) {
      return;
    }
    await protocolService.deleteProtocol(selectedProtocol.id);
    await fetchProduct();
    handleProductDialogClose();
  }

  if (!productId) {
    return (<></>);
  }

  const [defects, setDefects] = useState<Defect[]>([]);

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <BreadLink color="inherit" href="/projects">
            Projects
          </BreadLink>
          <Typography color="text.primary">Product</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid container spacing={2} item xs={12} md={12} >
        <Grid item xs={5} >
          <Card sx={{height: '100%', display: 'flex', justifyContent: 'space-between', flexDirection: 'column'}}>
            {product && 
              <CardContent>
                <Typography id={'product-name'} variant='h5'>{product.name}</Typography>
                <AuthorBar user={product.author}></AuthorBar>           
                <Table>
                  <TableBody>
                    <TableRow>
                      <TableCell><Typography id='date-label'>Creation date</Typography></TableCell>
                      <TableCell align="right">{formatDate( new Date(product.creationDate))}</TableCell>
                    </TableRow>
                    <TableRow>
                      <TableCell>
                        <Stack direction={'row'} alignItems={'center'} spacing={1}>
                          <WarningIcon/>
                          <Typography id='unfixed-defect-label'>Unfixed defects</Typography>
                        </Stack>
                      </TableCell>
                      <TableCell align="right">{defects.filter(x=> x.status == DefectStatus.NotFixed).length}</TableCell>
                    </TableRow>
                    <TableRow>
                      <TableCell>
                        <Stack direction={'row'} alignItems={'center'} spacing={1}>
                          <AssignmentIcon/>
                          <Typography id='protocol-to-fill-label'>Protocols</Typography>
                        </Stack>
                      </TableCell>
                      <TableCell align="right">{protocols.length}</TableCell>
                    </TableRow>
                    <TableRow>
                      <TableCell><Typography id='status-label'>Status</Typography></TableCell>
                      <TableCell align="right"> {product.status == 'Active' ? 
                        <Chip color='info' label={product.status} /> :
                        <Chip variant='outlined' label={product.status} />}
                      </TableCell>
                    </TableRow>
                  </TableBody>
                </Table>
              </CardContent>}
            <CardActions>
              <Button onClick={() => navigate('edit')} variant='contained' color='primary'>Edit information</Button>
            </CardActions>
          </Card>
        </Grid>
        <Grid item xs={7} >
          <Card sx={{height: '100%', display: 'flex', justifyContent: 'space-between', flexDirection: 'column'}}>
            <CardContent>
              <DrawingDefectsViewer defectsCallback={setDefects} productId={Number(productId)}/>
              <Button id="defect-details" onClick={() => navigate('defects')} variant='contained' color='primary'>Defect details</Button>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
      <Grid item xs={12} md={12} >
        <Card>
          <CardHeader title={'Assigned protocols'}
            action={<Button onClick={handleClickOpen} variant='contained'>Add protocol</Button>}>
          </CardHeader>
          <Divider></Divider>
          <CardContent>
            { protocols.length == 0 ?
              <Box sx={{marginTop: '10px'}}>
                <Typography>No Protocols</Typography>
              </Box> : 
              (<Box sx={{marginTop: '10px'}}>
                <Typography></Typography>
                {protocols.map((item, index) => (
                  <Box key={index} sx={{marginBottom: '10px'}}>
                    <Stack sx={{marginBottom: '5px'}} key={index} direction={'row'} justifyContent={'space-between'} alignItems={'center'}> 
                      <Typography>{item.name}</Typography>
                      <Stack direction={'row'} spacing={1}>
                        <Button onClick={() => navigate('protocols/'+ item.id)} variant='contained'>Details</Button>
                        <Button onClick={() => navigate(`protocols/${item.id}/report`)} variant='contained'>Report</Button>
                        <Button onClick={() => {setSelectedProtocol(item); setOpenProductDialog(true);}} color={'error'} variant='contained'>Delete</Button>
                      </Stack>                          
                    </Stack>
                    <Divider/> 
                  </Box>
                ))}
              </Box>)}
          </CardContent>
        </Card>
      </Grid>
      <AddProtocolDialog
        selectedValue={selectedValue}
        open={open}
        addProtocol={handleClose}
        onclose={closeDialog}
      />
      <DeleteItemDialog
        open={openProductDialog}
        handleClose={handleProductDialogClose}
        title={'Delete protocol'}
        body={`Are you sure you want to delete protocol ${selectedProtocol?.name}?`}
        action={deleteProtocol}  
      />
    </Grid>
  );
}