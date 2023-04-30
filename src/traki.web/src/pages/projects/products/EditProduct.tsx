import { Box, Link as BreadLink, Breadcrumbs, Button, Card, CardActions, CardContent, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Divider, Grid, IconButton, Stack, Table, TableBody, TableCell, TableHead, TableRow, TextField, Typography } from '@mui/material';
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
import { useAlert } from 'hooks/useAlert';
import { alertInitialState } from 'state/alert-state';
import ClearIcon from '@mui/icons-material/Clear';
import { UpdateProductRequest } from 'contracts/product/UpdateProductRequest';
import { AuthorBar } from 'components/AuthorBar';

const initialProduct: Product = {
  id: 0,
  name: '',
  projectId: 0,
  status: '',
  creationDate: ''
};

export function EditProduct() {
  const navigate = useNavigate();
  const { displaySuccess  } = useAlert();

  const { projectId, productId } = useParams();

  const [previewImage, setPreviewImage] = useState<string>();
  const [file, setFile] = useState<File>();

  const [product, setProduct] = useState<Product>(initialProduct);
  const [initialProductJson, setInitialProductJson] = useState<string>('');

  const [drawings, setDrawings] = useState<DrawingWithImage[]>([]);
  const [drawingName, setDrawingName] = useState<string>('');
  const [selectedDrawing, setSelectedDrawing] = useState<number>();

  const [openDialog, setOpenDialog] = React.useState(false);
  const [openProductDialog, setOpenProductDialog] = React.useState(false);

  const handleClickOpen = () => {
    setOpenDialog(true);
  };

  const handleClose = () => {
    setOpenDialog(false);
  };

  const handleProductDialogClose = () => {
    setOpenProductDialog(false);
  };

  useEffect(() => {
    fetchProduct();
    fetchDrawings();
  }, []);

  function canUpdateProduct(): boolean {
    return initialProductJson != JSON.stringify(product);
  }

  function canCreateDrawing(): boolean {
    return drawingName != '' && file != undefined;
  }

  async function fetchProduct() {
    const getProductResponse = await productService.getProduct(Number(projectId), Number(productId));
    setProduct(getProductResponse.product);
    setInitialProductJson(JSON.stringify(getProductResponse.product));
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

  async function deletedDrawing() {
    if (!selectedDrawing) {
      return;
    }
    await drawingService.deleteDrawing(Number(productId), selectedDrawing);
    await fetchDrawings();
  }

  async function updateProduct() {
    const request: UpdateProductRequest = {
      product: product
    };

    await productService.updateProduct(Number(projectId), Number(productId), request);
    await fetchProduct();
    displaySuccess("Product was updated successfully");
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
    await fetchDrawings();
    displaySuccess("Drawing was added successfully");

    setDrawingName('');
    setPreviewImage('');
    setFile(undefined);
  }

  async function deleteProduct() {
    await productService.deleteProduct(product.projectId, product.id);
    navigate(`/projects`);
    handleProductDialogClose();
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
      <Grid container spacing={2} item xs={12} md={12}>
        <Grid item container xs={5} spacing={2}>
          <Grid item xs={12} >
            <Card sx={{display: 'flex', justifyContent: 'space-between', flexDirection: 'column'}}>
              <CardContent>
                <Grid container spacing={2}>
                  <Grid item xs={12} md={12}>
                    <Stack direction={'row'} justifyContent={'space-between'} alignItems={'center'}>
                      <Typography>Product Information</Typography>
                      <Button onClick={() => setOpenProductDialog(true)} variant='contained' color='error'>Delete</Button>
                    </Stack>
                  </Grid>
                  <Grid item xs={12} md={12}>
                    <Divider></Divider>
                  </Grid>
                  <Grid item xs={12} md={12}>
                    <Box sx={{marginBottom: '15px'}}>
                      <AuthorBar user={product.author}/>
                    </Box>
                    <TextField 
                      sx={{width: '100%'}}
                      label='Name'
                      value={product.name}
                      onChange={(e) => setProduct({...product, name: e.target.value})}
                    />
                  </Grid>
                  <Grid item xs={12} md={12}>
                    <Button disabled={!canUpdateProduct()} onClick={updateProduct} variant='contained' color='primary'>Update information</Button>
                  </Grid>
                </Grid>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12}>
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
                    <Stack direction={'row'}>
                      <TextField 
                        sx={{width: '100%'}}
                        label='Drawing name'
                        value={drawingName}
                        onChange={(e) => setDrawingName(e.target.value)}
                      />
                      <IconButton color="secondary" aria-label="upload picture" component="label">
                        <input hidden accept="image/*" type="file" onChange={selectFile} />
                        <PhotoCamera />
                      </IconButton>
                    </Stack>
                  </Grid>
                  <Grid item xs={12} md={12}>
                    <Stack direction={'column'}  alignItems={'flex-start'}>
                      { previewImage &&
                      <img style={{maxHeight: '200px', width: 'auto', borderRadius: '2%',}} 
                        src={previewImage ? previewImage : 'https://i0.wp.com/roadmap-tech.com/wp-content/uploads/2019/04/placeholder-image.jpg?resize=400%2C400&ssl=1'}> 
                      </img>}
                    </Stack>
                  </Grid>
                  <Grid item xs={12} md={12}>
                    <Button variant='contained' disabled={!canCreateDrawing()} onClick={createDrawing}>Add drawing</Button>
                  </Grid>
                </Grid>
              </CardContent>  
            </Card>
          </Grid>
        </Grid>
        <Grid item xs={7} >
          <Card sx={{display: 'flex', justifyContent: 'space-between', flexDirection: 'column'}}>
            <CardContent>
              <Stack direction={'row'} spacing={2} sx={{ flexWrap: 'wrap', gap: 1 }}>
                {drawings.map((item, index) => 
                  <Box key={index} sx={{borderColor: 'black', borderWidth: 1, borderStyle: 'solid'}}>
                    <ImageWithViewer source={item.imageBase64} height={200}/>
                    <Stack direction={'row'} alignItems={'center'} justifyContent={'space-around'}>
                      <Typography>{item.drawing.title}</Typography>
                      <IconButton onClick={() => {handleClickOpen(); setSelectedDrawing(item.drawing.id);}}>
                        <ClearIcon color={'error'}/>
                      </IconButton> 
                    </Stack>
                  </Box>)}
              </Stack>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
      <Dialog open={openDialog} onClose={handleClose}>
        <DialogTitle color={'error'}>Delete drawing</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete drawing?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button color='error' onClick={() => {handleClose(); deletedDrawing();}}>Delete</Button>
        </DialogActions>
      </Dialog>
      <Dialog open={openProductDialog} onClose={handleProductDialogClose}>
        <DialogTitle>Delete product</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete product {product.name}?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button color='inherit' onClick={handleProductDialogClose}>Cancel</Button>
          <Button color='error' onClick={() => deleteProduct()}>Delete</Button>
        </DialogActions>
      </Dialog>
    </Grid>
  );
}