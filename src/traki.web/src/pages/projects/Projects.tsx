import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Breadcrumbs, Button, Card, CardContent, Divider, Grid, List, ListItem, ListItemButton, ListItemText, Table, TableBody, TableCell, TableRow, Typography } from '@mui/material';
import BuildCircleOutlinedIcon from '@mui/icons-material/BuildCircleOutlined';
import projectService from '../../services/project-service';
import productService from '../../services/product-service';
import { Link as BreadLink } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';
import AddIcon from '@mui/icons-material/Add';

type Project = {
  id: number,
  name: string
}

type Product = {
  id: number,
  name: string
}


const data: Project[] = [{id: 1, name: "Sample project 1"}, {id: 2, name: "Sample project 2"}];
const productData: Product[] = [{id: 1, name: "Sample product 1"}, {id: 2, name: "Sample product 2"}];


type ProjectProductsProps = {
  project: Project
}
function ProjectProducts({project}: ProjectProductsProps) {
  const navigate = useNavigate();
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetchProducts();
  }, []);

  async function fetchProducts() {
    const getProductsResponse = await productService.getProducts(project.id);
    setProducts(getProductsResponse.products);
  }
  return (
    <Card key={project.id} sx={{marginBottom: 2, minWidth: '700px', width: '75%', maxWidth: '1000px'}} title='Sample Project'>
      <CardContent>
        <Box sx={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between', marginBottom: '10px'}}>
          <Box>
            <Typography variant="h5" component="div">
              {project.name}
            </Typography>
            <Typography variant="subtitle1" component="div">
              Client: Sample Client
            </Typography>
          </Box>
          <Box>
            <Button onClick={() => navigate(`/projects/${project.id}/products/create`)} color='secondary' variant='contained' startIcon={<AddIcon/>}>Add Product</Button>
            <Button onClick={() => navigate(`/projects/${project.id}/edit`)}  sx={{marginLeft: '10px'}} variant='contained' startIcon={<EditIcon/>}>Edit Project</Button>
          </Box>
        </Box>
        <Divider/>
        <Box sx={{display: 'flex', justifyContent: 'space-between', marginTop: '10px'}}>
          <Box sx={{width: '100%'}}>
            <List>
              {products.map((product, index) => 
                <ListItem key={index} disablePadding>
                  <ListItemButton onClick={() => navigate(`/projects/${project.id}/products/${product.id}`)}>
                    <ListItemText primary={product.name}/>
                  </ListItemButton>
                </ListItem>)}
            </List>
          </Box>
          <Box>
            <img style={{maxWidth: '400px', height: 'auto', borderRadius: '2%',}} src='https://hips.hearstapps.com/hmg-prod/images/cute-cat-photos-1593441022.jpg?crop=1.00xw:0.753xh;0,0.153xh&resize=1200:*' />
          </Box>
        </Box>
      </CardContent>      
    </Card>
  );
}

export function Projects() {

  const navigate = useNavigate();
  const [projects, setProjects] = useState<Project[]>([]);

  useEffect(() => {
    fetchProjects();
  }, []);

  async function fetchProjects() {
    const getProjectsResponse = await projectService.getProjects();
    setProjects(getProjectsResponse.projects);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <Typography color="text.primary">Projects</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={12} md={12}>
        <Button onClick={() => navigate(`/projects/create`)} color='secondary' variant='contained' startIcon={<AddIcon/>}>Add Project</Button>
      </Grid>
      <Grid item xs={12} md={12}>
        { projects.map((item, index) =>
          <ProjectProducts key={index} project={item}></ProjectProducts>
        )}
      </Grid>
    </Grid>
  );
}