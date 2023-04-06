import React, { useEffect, useState } from 'react';
import { Grid, Card, CardContent, Typography, Breadcrumbs, Button, CardActions, CardMedia, Stack, Box } from '@mui/material';
import recommendationService from '../services/recommendation-service';
import { Defect } from '../contracts/drawing/defect/Defect';
import { DefectWithImage } from '../components/types/DefectWithImage';
import defectService from '../services/defect-service';
import pictureService from '../services/picture-service';
import { Product } from '../contracts/product/Product';
import { DrawingWithImage } from '../components/types/DrawingWithImage';
import drawingService from '../services/drawing-service';
import { Drawing } from '../contracts/drawing/Drawing';
import AssignmentIcon from '@mui/icons-material/Assignment';
import BuildIcon from '@mui/icons-material/Build';
import { useNavigate } from 'react-router-dom';

const notFoundImage = 'https://i0.wp.com/roadmap-tech.com/wp-content/uploads/2019/04/placeholder-image.jpg?resize=400%2C400&ssl=1';

export function HomePage() {
  const [defects, setDefects] = useState<DefectWithImage[]>([]);
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetchRecommendations();
  }, []);

  async function fetchRecommendations() {
    const response = await recommendationService.getRecommendations();
    setProducts(response.recommendation.products);
    fetchDefects(response.recommendation.defects);
  }

  async function fetchDefects(defects: Defect[]) {

    const defectsWithImage: DefectWithImage[] = [];

    for (let i = 0; i < defects.length; i++) {
      const newDefect = await fetchDefect(defects[i]);
      defectsWithImage.push(newDefect);
    }

    setDefects(defectsWithImage);
  }

  async function fetchDefect(defect: Defect): Promise<DefectWithImage> {
    const response = await defectService.getDefect(defect.drawingId, defect.id);
    let imageBase64 = '';
    if (response.defect.imageName != '') {
      imageBase64 = await pictureService.getPicture('item', response.defect.imageName);
    } 
    const defectWithImage: DefectWithImage = {
      defect: response.defect,
      imageBase64: imageBase64
    };

    return defectWithImage;
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
  defect: DefectWithImage
}

function DefectCard ({defect}: DefectCardProps) {
  const navigate = useNavigate();

  const [product, setProduct] = useState<Product>();

  useEffect(() => {

  }, []);

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
          image={defect.imageBase64 ? defect.imageBase64 : notFoundImage}
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
        <Button onClick={() => navigate(`/projects/${product.projectId}/products/${product.id}/defects`)} variant='contained' size="small">Details</Button>
        </CardActions>
      </Card>
    </Grid>
  );
}