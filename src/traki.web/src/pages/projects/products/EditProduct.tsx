import { Box, Link as BreadLink, Breadcrumbs, Button, Card, CardActions, CardContent, Divider, Grid, IconButton, Stack, Table, TableBody, TableCell, TableHead, TableRow, TextField, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Product } from '../../../contracts/product/Product';
import productService from '../../../services/product-service';
import { Drawing } from 'contracts/drawing/Drawing';
import { Defect } from 'contracts/drawing/defect/Defect';
import { DrawingWithImage } from 'features/products/types/DrawingWithImage';
import { drawingService, pictureService } from 'services';
import { PhotoCamera } from '@mui/icons-material';
import ImageWithViewer from 'components/ImageWithViewer';
import { v4 as uuid } from 'uuid';
import { CreateDrawingRequest } from 'contracts/drawing/CreateDrawingRequest';

const initialProduct: Product = {
  id: 0,
  name: '',
  projectId: 0
};

export function EditProduct() {
  const navigate = useNavigate();
  const { projectId, productId } = useParams();

  const [previewImage, setPreviewImage] = useState<string>();
  const [file, setFile] = useState<File>();

  const [product, setProduct] = useState<Product>(initialProduct);
  const [drawings, setDrawings] = useState<DrawingWithImage[]>([]);
  const [drawingName, setDrawingName] = useState<string>('');

  const [open, setOpen] = React.useState(false);

  useEffect(() => {
    fetchProduct();
    fetchDrawings();
  }, []);

  function canCreateDrawing(): boolean {
    return drawingName != '' && file != undefined;
  }

  async function fetchProduct() {
    const getProductResponse = await productService.getProduct(Number(projectId), Number(productId));
    setProduct(getProductResponse.product);
  }

  async function fetchDrawings() {
    const response = await drawingService.getDrawings(Number(productId));
    await fetchDrawingPictures(response.drawings);
  }

  async function fetchDrawingPictures(drawings: Drawing[]) {
    const drawingsWithImage: DrawingWithImage[] = [];
    const newDefects: Defect[] = [];
    for (let i = 0; i < drawings.length; i++) {
      const item = drawings[i];
      newDefects.push(...item.defects);
      const imageBase64 = await pictureService.getPicture('company', item.imageName);
      const newDrawingImage: DrawingWithImage = {drawing: item, imageBase64: imageBase64};
      drawingsWithImage.push(newDrawingImage);
    }
    
    setDrawings(drawingsWithImage);
  }

  function selectFile (event: React.ChangeEvent<HTMLInputElement>) {
    const { files } = event.target;
    const selectedFiles = files as FileList;
    setFile(selectedFiles?.[0]);
    setPreviewImage(URL.createObjectURL(selectedFiles?.[0]));
  }

  async function createDrawing() {
    if (!canCreateDrawing()) {
      return;
    }
    let pictureName = '';
    if (previewImage != '' && file) {
      pictureName = `${uuid()}${file.type.replace('image/','.')}`;
      const formData = new FormData();
      formData.append(pictureName, file, pictureName);
      await pictureService.uploadPicturesFormData('company', formData);
    }

    const drawing: Drawing = {
      id: 0,
      title: drawingName,
      imageName: pictureName,
      defects: []
    };

    const request: CreateDrawingRequest = {
      drawing: drawing
    };

    await drawingService.createDrawing(Number(productId), request);
  }

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
              <Grid container spacing={2}>
                <Grid item xs={12} md={12}>
                  <Typography>Product Information</Typography>
                </Grid>
                <Grid item xs={12} md={12}>
                  <Divider></Divider>
                </Grid>
                <Grid item xs={12} md={12}>
                  <TextField 
                    sx={{width: '100%'}}
                    label='Name'
                    value={product.name}/>
                </Grid>
              </Grid>
            </CardContent>
            <CardActions>
              <Button onClick={() => navigate('defects')} variant='contained' color='primary'>Update information</Button>
            </CardActions>
          </Card>
        </Grid>
        <Grid item xs={7}>
          <Card sx={{height: "100%"}}>
            <CardContent>
              <Grid container spacing={2}>
                <Grid item xs={12} md={12}>
                  <Typography>New drawing</Typography>
                </Grid>
                <Grid item xs={12} md={12}>
                  <Divider></Divider>
                </Grid>
                <Grid item xs={12} md={12}>
                  <Stack direction={'column'}  alignItems={'flex-start'}>
                    <img style={{maxHeight: '200px', width: 'auto', borderRadius: '2%',}} 
                      src={previewImage ? previewImage : 'https://i0.wp.com/roadmap-tech.com/wp-content/uploads/2019/04/placeholder-image.jpg?resize=400%2C400&ssl=1'}> 
                    </img>
                    <IconButton color="secondary" aria-label="upload picture" component="label">
                      <input hidden accept="image/*" type="file" onChange={selectFile} />
                      <PhotoCamera />
                    </IconButton>
                  </Stack>
                </Grid>
                <Grid item xs={12} md={12}>
                  <TextField 
                    sx={{width: '100%'}}
                    label='Drawing name'
                    value={drawingName}
                    onChange={(e) => setDrawingName(e.target.value)}
                  />
                </Grid>
                <Grid item xs={12} md={12}>
                  <Button disabled={!canCreateDrawing()} onClick={createDrawing}>Create drawing</Button>
                </Grid>
              </Grid>
            </CardContent>  
          </Card>
        </Grid>
        <Grid item xs={12} >
          <Card sx={{height: '100%', display: 'flex', justifyContent: 'space-between', flexDirection: 'column'}}>
            <CardContent>
              <Stack direction={'row'} spacing={2} sx={{ flexWrap: 'wrap', gap: 1 }}>
                {drawings.map((item, index) => 
                  <Box key={index} sx={{borderColor: 'black', borderWidth: 1, borderStyle: 'solid'}}>
                    <ImageWithViewer source={item.imageBase64} height={200}/>
                  </Box>)}
              </Stack>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Grid>
  );
}