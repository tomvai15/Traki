import React, { useEffect, useState } from 'react';
import { Grid, Card, CardContent, Typography, Breadcrumbs, Button, CardActions, CardMedia, Stack, Box } from '@mui/material';
import { drawingService, defectService, recommendationService, pictureService } from '../services';
import { Defect } from 'contracts/drawing/defect/Defect';
import { DefectWithImage } from 'features/defects/types/DefectWithImage';
import { Product } from 'contracts/product/Product';
import { DrawingWithImage } from 'components/types/DrawingWithImage';
import { Drawing } from 'contracts/drawing/Drawing';
import AssignmentIcon from '@mui/icons-material/Assignment';
import BuildIcon from '@mui/icons-material/Build';
import { useNavigate } from 'react-router-dom';
import { DefectRecomendation } from 'contracts/recommendation/DefectRecomendation';

const notFoundImage = 'https://i0.wp.com/roadmap-tech.com/wp-content/uploads/2019/04/placeholder-image.jpg?resize=400%2C400&ssl=1';

export function HomePage() {
  const [defects, setDefects] = useState<DefectRecomendation[]>([]);
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetchRecommendations();
  }, []);

  async function fetchRecommendations() {
    const response = await recommendationService.getRecommendations();
    setProducts(response.recommendation.products);
    setDefects(response.recommendation.defects);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <Typography color="text.primary">Home</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={12} md={12} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant='h5'>My Products</Typography>
          </CardContent>    
        </Card>
      </Grid>
      { products.map((item, index) => <ProductCard product={item} key={index}></ProductCard>)}
      <Grid item xs={12} md={12} >
        <Card>
          <CardContent sx={{ display: 'flex', flexDirection: 'column'}}>
            <Typography variant='h5'>Relevant defects</Typography>
          </CardContent>    
        </Card>
      </Grid>
      { defects.map((item, index) => <DefectCard defect={item} key={index}></DefectCard>)}
    </Grid>
  );
}

type ProductCardProps = {
  product: Product
}

function ProductCard ({product}: ProductCardProps) {
  const navigate = useNavigate();
  const [drawing, setDrawing] = useState<DrawingWithImage>();

  useEffect(() => {
    fetchDrawings();
  }, []);

  async function fetchDrawings() {
    const response = await drawingService.getDrawings(Number(product.id));
    await fetchDrawingPictures(response.drawings[0]);
  }

  async function fetchDrawingPictures(drawing: Drawing) {
    const imageBase64 = await pictureService.getPicture('company', drawing.imageName);
    const newDrawingImage: DrawingWithImage = {drawing: drawing, imageBase64: imageBase64};

    setDrawing(newDrawingImage);
  }

  return (
    <Grid item xs={12} sm={6} md={4}>
      <Card 
        sx={{ height: '100%', display: 'flex', flexDirection: 'column'}}
      >
        <CardMedia
          height='200'
          component="img"
          sx={{
            16:9
          }}
          image={drawing ? drawing.imageBase64 : notFoundImage}
          alt="random"
        />
        <CardContent sx={{ flexGrow: 1 }}>
          <Typography gutterBottom variant="h5" component="h2">
            {product.name}
          </Typography>
          <Stack direction='row' spacing={2} justifyContent='space-around'>
            <Box sx={{display: 'flex',flexDirection: 'column', alignItems: 'center'}}>
              <BuildIcon />
              <Typography>4 Defects</Typography>
            </Box>
            <Box sx={{display: 'flex',flexDirection: 'column', alignItems: 'center'}}>
              <AssignmentIcon />
              <Typography>3 Protocols</Typography>
            </Box>
          </Stack>
        </CardContent>
        <CardActions>
          <Button onClick={() => navigate(`/projects/${product.projectId}/products/${product.id}`)} variant='contained' size="small">Details</Button>
        </CardActions>
      </Card>
    </Grid>
  );
}

type DefectCardProps = {
  defect: DefectRecomendation
}

function DefectCard ({defect}: DefectCardProps) {
  const navigate = useNavigate();

  const [defectWithImage, setDefectWithImage] = useState<DefectWithImage>();

  useEffect(() => {
    fetchDefect(defect.defect);
  }, []);


  async function fetchDefect(defect: Defect) {
    const response = await defectService.getDefect(defect.drawingId, defect.id);
    let imageBase64 = '';
    if (response.defect.imageName != '') {
      imageBase64 = await pictureService.getPicture('item', response.defect.imageName);
    } 
    const defectWithImage: DefectWithImage = {
      defect: response.defect,
      imageBase64: imageBase64
    };

    setDefectWithImage(defectWithImage);
  }

  return (
    <Grid item xs={12} sm={6} md={4}>
      <Card 
        sx={{ height: '100%', display: 'flex', flexDirection: 'column'}}
      >
        <CardMedia
          height='200'
          component="img"
          sx={{
            16:9
          }}
          image={defectWithImage && defectWithImage.imageBase64 ? defectWithImage.imageBase64 : notFoundImage}
          alt="random"
        />
        <CardContent sx={{ flexGrow: 1 }}>
          <Typography gutterBottom variant="h5" component="h2">
            {defect.defect.title}
          </Typography>
          <Typography sx={{
            display: '-webkit-box',
            overflow: 'hidden',
            WebkitBoxOrient: 'vertical',
            WebkitLineClamp: 3,
          }}>
            Product: test
          </Typography>
        </CardContent>
        <CardActions>
          <Button onClick={() => navigate(`/projects/${defect.projectId}/products/${defect.productId}/defects`, { state: { defectId: defect.defect.id } })} variant='contained' size="small">Details</Button>
        </CardActions>
      </Card>
    </Grid>
  );
}