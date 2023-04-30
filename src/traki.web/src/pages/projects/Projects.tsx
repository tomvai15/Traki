import React, { useEffect, useState } from 'react';
import { Box, Breadcrumbs, Button, Card, CardContent, Chip, Divider, Grid, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Typography } from '@mui/material';
import { pictureService, productService, projectService } from '../../services';
import { useNavigate } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';
import AddIcon from '@mui/icons-material/Add';
import { Project } from '../../contracts/projects/Project';
import { Product } from '../../contracts/product/Product';
import { useRecoilState } from 'recoil';
import { pageState } from 'state/page-state';
import { ProtectedComponent } from 'components/ProtectedComponent';
import { HideableComponent } from 'components/HideableComponent';
import { AuthorBar } from 'components/AuthorBar';
import { CustomAvatar } from 'components/CustomAvatar';

type ProjectWithImage = {
  project: Project,
  imageBase64: string
}

export function Projects() {
  const navigate = useNavigate();
  const [projects, setProjects] = useState<ProjectWithImage[]>([]);
  const [page, setPageState] = useRecoilState(pageState);

  useEffect(() => {
    fetchProjects();
  }, []);

  async function fetchProjects() {
    const getProjectsResponse = await projectService.getProjects();
    await fetchProjectImages(getProjectsResponse.projects);
  }

  async function fetchProjectImages(projects: Project[]) {

    const projectWithImages: ProjectWithImage[] = [];

    console.log(projects);
    for (let i = 0; i < projects.length; i++) {
      let imageBase64 = '';
      if (projects[i].imageName) {
        imageBase64 = await pictureService.getPicture('company', projects[i].imageName);
      }
      const projectWithImage: ProjectWithImage = {
        project: projects[i],
        imageBase64: imageBase64
      };
      projectWithImages.push(projectWithImage);
    }

    setProjects(projectWithImages);
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <Typography color="text.primary">Projects</Typography>
        </Breadcrumbs>
      </Grid>
      <Button onClick={() => setPageState({...page, notFound: true})}>Press</Button>
      <Grid item xs={12} md={12}>
        <ProtectedComponent role={'ProjectManager'}>
          <Button onClick={() => navigate(`/projects/create`)} color='secondary' variant='contained' startIcon={<AddIcon/>}>
            Add Project
          </Button>
        </ProtectedComponent>
      </Grid>
      <Grid item xs={12} md={12}>
        { projects.map((item, index) =>
          <ProjectProducts key={index} project={item}></ProjectProducts>
        )}
      </Grid>
    </Grid>
  );
}

type ProjectProductsProps = {
  project: ProjectWithImage
}

function ProjectProducts({project}: ProjectProductsProps) {
  const navigate = useNavigate();
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetchProducts();
  }, []);

  async function fetchProducts() {
    const getProductsResponse = await productService.getProducts(project.project.id);
    setProducts(getProductsResponse.products);
    console.log(getProductsResponse.products);
  }
  
  return (
    <Card key={project.project.id} sx={{marginBottom: 2}} title='Sample Project'>
      <CardContent>
        <Box sx={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between', marginBottom: '10px'}}>
          <Box>
            <AuthorBar user={project.project.author}></AuthorBar>
            <Typography variant="h5" component="div">
              {project.project.name}
            </Typography>
            <Typography variant="subtitle1" component="div">
              Client: Sample Client
            </Typography>
          </Box>
          <Box>
            <ProtectedComponent role={'ProductManager'}>
              <Button onClick={() => navigate(`/projects/${project.project.id}/products/create`)} color='secondary' variant='contained' startIcon={<AddIcon/>}>Add Product</Button>
            </ProtectedComponent>
            <HideableComponent checkIfRender={(user) => user.id == project.project.author?.id}>
              <Button onClick={() => navigate(`/projects/${project.project.id}/edit`)}  sx={{marginLeft: '10px'}} variant='contained' startIcon={<EditIcon/>}>
                Edit Project
              </Button>
            </HideableComponent>
          </Box>
        </Box>
        <Divider/>
        <Box sx={{display: 'flex', justifyContent: 'space-between', marginTop: '10px'}}>
          <Box sx={{width: '100%'}}>
            <Typography variant='subtitle1'>Products</Typography>
            <List>
              {products.map((product, index) => 
                <Box key={index} >
                  <Divider></Divider>
                  <ListItem disablePadding>
                    <ListItemButton onClick={() => navigate(`/projects/${project.project.id}/products/${product.id}`)}>
                      <ListItemIcon>
                        <AuthorBar user={product.author} variant={'short'}/>
                      </ListItemIcon>
                      <ListItemText>
                        <Typography variant='h6'>{product.name}</Typography>
                      </ListItemText>
                      <ListItemIcon>
                        {product.status == 'Active' ? 
                          <Chip color='info' label={product.status} /> :
                          <Chip variant='outlined' label={product.status} />}
                      </ListItemIcon>
                    </ListItemButton>
                  </ListItem>
                  <Divider></Divider>
                </Box>)}
            </List>
          </Box>
          <Box>
            <img style={{maxWidth: 'auto', height: '300px', borderRadius: '2%',}} 
              src={project.imageBase64 ? project.imageBase64 : 'https://i0.wp.com/roadmap-tech.com/wp-content/uploads/2019/04/placeholder-image.jpg?resize=400%2C400&ssl=1'} />
          </Box>
        </Box>
      </CardContent>      
    </Card>
  );
}
