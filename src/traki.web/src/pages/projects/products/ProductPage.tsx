import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Avatar, Button, Card, CardActions, CardContent, Dialog, DialogTitle, Divider, Grid, IconButton, ImageList, ImageListItem, ImageListItemBar, List, ListItem, ListItemAvatar, ListItemButton, ListItemText, Table, TableBody, TableCell, TableHead, TableRow, Typography } from '@mui/material';
import BuildCircleOutlinedIcon from '@mui/icons-material/BuildCircleOutlined';
import productService from '../../../services/product-service';
import { Product } from '../../../contracts/product/Product';
import { useNavigate, useParams } from 'react-router-dom';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { blue } from '@mui/material/colors';
import PersonIcon from '@mui/icons-material/Person';
import AddIcon from '@mui/icons-material/Add';
import protocolService from '../../../services/protocol-service';
import AssignmentIcon from '@mui/icons-material/Assignment';
import { AreaSelector, IArea } from '@bmunozg/react-image-area';
import InfoIcon from '@mui/icons-material/Info';

function createData(
  name: string,
  calories: number,
  fat: number,
  carbs: number,
  protein: number,
) {
  return { name, calories, fat, carbs, protein };
}

const itemData = [
  {
    img: 'https://images.unsplash.com/photo-1551963831-b3b1ca40c98e',
    title: 'Breakfast',
  },
  {
    img: 'https://images.unsplash.com/photo-1551782450-a2132b4ba21d',
    title: 'Burger',
  },
  {
    img: 'https://images.unsplash.com/photo-1522770179533-24471fcdba45',
    title: 'Camera',
  },
  {
    img: 'https://images.unsplash.com/photo-1444418776041-9c7e33cc5a9c',
    title: 'Coffee',
  },
  {
    img: 'https://images.unsplash.com/photo-1533827432537-70133748f5c8',
    title: 'Hats',
  },
  {
    img: 'https://images.unsplash.com/photo-1558642452-9d2a7deb7f62',
    title: 'Honey',
  },
  {
    img: 'https://images.unsplash.com/photo-1516802273409-68526ee1bdd6',
    title: 'Basketball',
  },
  {
    img: 'https://images.unsplash.com/photo-1518756131217-31eb79b20e8f',
    title: 'Fern',
  },
  {
    img: 'https://images.unsplash.com/photo-1597645587822-e99fa5d45d25',
    title: 'Mushrooms',
  },
  {
    img: 'https://images.unsplash.com/photo-1567306301408-9b74779a11af',
    title: 'Tomato basil',
  },
  {
    img: 'https://images.unsplash.com/photo-1471357674240-e1a485acb3e1',
    title: 'Sea star',
  },
  {
    img: 'https://images.unsplash.com/photo-1589118949245-7d38baf380d6',
    title: 'Bike',
  },
];


const initialProduct: Product = {
  id: 0,
  name: '',
  projectId: 0
};

export interface SimpleDialogProps {
  open: boolean;
  selectedValue: string;
  addProtocol: (id: number) => void;
  onclose: () => void;
}

const emails = ['username@gmail.com', 'user02@gmail.com'];


function AddProtocolDialog(props: SimpleDialogProps) {
  const { onclose, addProtocol, selectedValue, open } = props;

  const [protocols, setProtocols] = useState<Protocol[]>([]);

  useEffect(() => {
    fetchProtocols();
  }, []);

  async function fetchProtocols() {
    const getProtocolsResponse = await protocolService.getTemplateProtocols();
    setProtocols(getProtocolsResponse.protocols);
  }

  const handleListItemClick = (protocolId: number) => {
    console.log(protocolId);
    addProtocol(protocolId);
    console.log('????');
    onclose();
  };

  return (
    <Dialog onClose={onclose} open={open}>
      <DialogTitle>Select protocol</DialogTitle>
      <List sx={{ pt: 0 }}>
        {protocols.map((protocol, index) => (
          <ListItem key={index} disableGutters>
            <ListItemButton onClick={() => handleListItemClick(protocol.id)}>
              <ListItemAvatar>
                <AssignmentIcon />
              </ListItemAvatar>
              <ListItemText primary={  protocol.id +' ' + protocol.name} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Dialog>
  );
}

export function ProductPage() {
  const navigate = useNavigate();
  const { projectId, productId } = useParams();
  const [product, setProduct] = useState<Product>(initialProduct);
  const [protocols, setProtocols] = useState<Protocol[]>([]);

  const [open, setOpen] = React.useState(false);
  const [selectedValue, setSelectedValue] = React.useState(emails[1]);

  const [selectedImage, setSelectedImage] = React.useState<string>('');

  const [areas, setAreas] = useState<IArea[]>([]);

  const onChangeHandler = (areas: IArea[]) => {
    console.log(areas);
    setAreas(areas);
  };

  const handleClickOpen = () => {
    setOpen(true);
  };

  const closeDialog = () => {
    setOpen(false);
  };

  useEffect(() => {
    fetchProduct();
  }, []);

  async function addProtocol(protocolId: number) {
    console.log('??');
    await productService.addProtocol(Number(projectId), Number(productId), protocolId);
    await fetchProduct();
  }

  const handleClose = (protocolId: number) => {
    console.log('protocolId');
    addProtocol(protocolId);
    closeDialog();
  };

  async function fetchProduct() {
    const getProductResponse = await productService.getProduct(Number(projectId), Number(productId));
    setProduct(getProductResponse.product);
    const getProductProtocolsResponse = await productService.getProtocols(Number(projectId), Number(productId));
    setProtocols(getProductProtocolsResponse.protocols);
  }

  return (
    <Grid container spacing={2}>
      <Grid container spacing={2} item xs={12} md={12} >
        <Grid item xs={8} >
          <Card>
            <CardContent>
              <Typography variant='h5'>{product.name}</Typography>
              <Box sx={{display: 'flex', flexDirection: 'row', justifyContent: 'space-around'}}>
                <Box sx={{flex: 2, padding: '5px', width: '100%', height: '100%'}}>
                  <AreaSelector
                    areas={areas}
                    unit='percentage'
                    onChange={onChangeHandler}
                  >
                    <img width={'100%'} src={selectedImage} alt='my image'/>
                  </AreaSelector>
                </Box>
                <Box sx={{flex: 1, padding: '5px'}}>
                  <ImageList sx={{ width: 200, height: 400 }} cols={1}>
                    {itemData.map((item) => (
                      <ImageListItem key={item.img}>
                        <img
                          src={`${item.img}?w=164&h=164&fit=crop&auto=format`}
                          srcSet={`${item.img}?w=164&h=164&fit=crop&auto=format&dpr=2 2x`}
                          alt={item.title}
                          loading="lazy"
                        />
                        <ImageListItemBar
                          title={item.title}
                          actionIcon={
                            <IconButton 
                              onClick={() => setSelectedImage(item.img)}
                              sx={{ color: 'rgba(255, 255, 255, 0.54)' }}
                              aria-label={`info about ${item.title}`}
                            >
                              <InfoIcon />
                            </IconButton>
                          }
                        />
                      </ImageListItem>
                    ))}
                  </ImageList>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={4} >
          <Card sx={{height: '100%', display: 'flex', justifyContent: 'space-around', flexDirection: 'column'}}>
            <CardContent>
              <Typography variant='h5'>2 Defects</Typography>
              <Divider></Divider>
              <Typography variant='h6'>Bad weld</Typography>
              <Typography variant='h6'>Incorrect screw</Typography>
            </CardContent>
            <CardActions>
              <Button variant='contained' color='error'>New defect</Button>
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