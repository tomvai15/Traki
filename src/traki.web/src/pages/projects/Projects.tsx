import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Button, Card, CardContent, Divider, Table, TableBody, TableCell, TableRow, Typography } from '@mui/material';
import BuildCircleOutlinedIcon from '@mui/icons-material/BuildCircleOutlined';
import projectService from '../../services/project-service';
import productService from '../../services/product-service';
import { useNavigate } from 'react-router-dom';

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
    <Card key={project.id} sx={{marginBottom: 2}} title='Sample Project'>
      <CardContent>
        <Typography variant="h5" component="div">
          {project.name}
        </Typography>
        <Divider/>
        <Table>
          <TableBody>
            {products.map((product) => (
              <TableRow
                key={product.id}
                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
              >
                <TableCell component="th" scope="row" sx={{display: 'flex', flexDirection: 'row'}}>
                  <BuildCircleOutlinedIcon/>
                  <Typography fontSize={20} sx={{marginLeft: 2}}>
                    {product.name}
                  </Typography>
                  <Button variant='contained' onClick={() => navigate(`/projects/${project.id}/products/${product.id}`)}>View Product</Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>      
    </Card>
  );
}

export function Projects() {

  const [projects, setProjects] = useState<Project[]>([]);

  useEffect(() => {
    fetchProjects();
  }, []);

  async function fetchProjects() {
    const getProjectsResponse = await projectService.getProjects();
    setProjects(getProjectsResponse.projects);
  }

  return (
    <Box>
      { projects.map((item, index) =>
        <ProjectProducts key={index} project={item}></ProjectProducts>
      )}
    </Box>
  );
}