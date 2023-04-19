import { Link as BreadLink, Breadcrumbs, Button, Card, CardActions, CardContent, Grid, Table, TableBody, TableCell, TableHead, TableRow, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Product } from '../../../contracts/product/Product';
import { Protocol } from '../../../contracts/protocol/Protocol';
import productService from '../../../services/product-service';
import { DrawingDefectsViewer } from 'features/products/components/DrawingDefectsViewer';
import AddProtocolDialog from 'features/products/components/AddProtocolDialog';

const initialProduct: Product = {
  id: 0,
  name: '',
  projectId: 0
};

const emails = ['username@gmail.com', 'user02@gmail.com'];

export function ProductPage() {
  const navigate = useNavigate();
  const { projectId, productId } = useParams();
  const [product, setProduct] = useState<Product>(initialProduct);

  const [protocols, setProtocols] = useState<Protocol[]>([]);

  const [open, setOpen] = React.useState(false);
  const [selectedValue, setSelectedValue] = React.useState(emails[1]);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const closeDialog = () => {
    setOpen(false);
  };

  useEffect(() => {
    fetchProduct();
  }, []);

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

  if (!productId) {
    return (<></>);
  }
  
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
            <CardContent>
              <Typography variant='h5'>{product.name}</Typography>
              <Typography>Author: {product.name}</Typography>
              <Typography>Creation date: {product.name}</Typography>
              <Typography>In progress</Typography>
              <Typography>6 not fixed defects</Typography>
              <Typography>2 protocols to fill</Typography>
              <></>
            </CardContent>
            <CardActions>
              <Button onClick={() => navigate('edit')} variant='contained' color='primary'>Edit information</Button>
            </CardActions>
          </Card>
        </Grid>
        <Grid item xs={7} >
          <Card sx={{height: '100%', display: 'flex', justifyContent: 'space-between', flexDirection: 'column'}}>
            <CardContent>
              <DrawingDefectsViewer productId={Number(productId)}/>
            </CardContent>
            <CardActions>
              <Button onClick={() => navigate('defects')} variant='contained' color='primary'>Defect details</Button>
            </CardActions>
          </Card>
        </Grid>
      </Grid>
      <Grid item xs={12} md={12} >
        <Card>
          <CardContent>
            <Typography variant='h5'>Assigned protocols</Typography>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
              <TableHead>
                <TableRow>
                  <TableCell>Protocol Name</TableCell>
                  <TableCell align="right">Due Date</TableCell>
                  <TableCell align="right"></TableCell>
                  <TableCell align="right"></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {protocols.map((item, index) => (
                  <TableRow
                    key={index}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell component="th" scope="row">{item.name}</TableCell>
                    <TableCell align="right">2021-04-06</TableCell>
                    <TableCell align="right"><Button onClick={() => navigate(`protocols/${item.id}/report`)} variant='contained'>Report</Button></TableCell>
                    <TableCell align="right"><Button onClick={() => navigate('protocols/'+ item.id)} variant='contained'>Fill</Button></TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </CardContent>
          <CardActions>
            <Button onClick={handleClickOpen} variant='contained'>Add protocol</Button>
          </CardActions>
        </Card>
      </Grid>
      <AddProtocolDialog
        selectedValue={selectedValue}
        open={open}
        addProtocol={handleClose}
        onclose={closeDialog}
      />
    </Grid>
  );
}