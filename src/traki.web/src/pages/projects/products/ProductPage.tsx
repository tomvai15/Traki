import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Button, Card, CardContent, Divider, Grid, Table, TableBody, TableCell, TableRow, Typography } from '@mui/material';
import BuildCircleOutlinedIcon from '@mui/icons-material/BuildCircleOutlined';
import productService from '../../../services/product-service';
import { Product } from '../../../contracts/product/Product';
import { useNavigate, useParams } from 'react-router-dom';
import { Protocol } from '../../../contracts/protocol/Protocol';

const initialProduct: Product = {
  id: 0,
  name: '',
  projectId: 0
};

export function ProductPage() {
  const navigate = useNavigate();
  const { projectId, productId } = useParams();
  const [product, setProduct] = useState<Product>(initialProduct);
  const [protocols, setProtocols] = useState<Protocol[]>([]);

  useEffect(() => {
    fetchProduct();
  }, []);

  async function fetchProduct() {
    const getProductResponse = await productService.getProduct(Number(projectId), Number(productId));
    setProduct(getProductResponse.product);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12} >
        <Typography>{product.name}</Typography>
      </Grid>
      <Grid item xs={12} md={12} >
        <Card sx={{ padding: 2}}>
          <Typography>Assigned protocols</Typography>
          {
            protocols.length == 0 ? <Typography>No protocols assigned</Typography> : <Typography>lol</Typography>
          }
        </Card>
      </Grid>
    </Grid>
  );
}