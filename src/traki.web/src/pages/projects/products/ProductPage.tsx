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
import { AreaSelector, IArea, IAreaRendererProps } from '@bmunozg/react-image-area';
import InfoIcon from '@mui/icons-material/Info';
import { Drawing } from '../../../contracts/drawing/Drawing';
import { type } from 'os';
import pictureService from '../../../services/picture-service';
import { Defect } from '../../../contracts/drawing/defect/Defect';
import { DefectStatus } from '../../../contracts/drawing/defect/DefectStatus';
import drawingService from '../../../services/drawing-service';

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

const initialDefects: Defect = {
  id: 1,
  title: 'Title 1',
  description: 'Description',
  status: DefectStatus.NotFixed,
  xPosition: 10,
  yPosition: 10,
  width: 10,
  height: 10
};

const initialDrawings: Drawing[] = [
  { id: 1, title: 'Drawing 1', imageName: 'Preftek-full-logo.png', defects: []},
  { id: 2, title: 'Drawing 2', imageName: 'a8q1bzQ_700b.jpg', defects: [initialDefects]},
  { id: 3, title: 'Drawing 2', imageName: 'a8q1bzQ_700b.jpg', defects: []},
];


type DrawingImage = {
  id: number,
  image: string
};

const drawingImageList: DrawingImage[] = [
  { id: 1, image: 'Preftek-full-logo.png'},
  { id: 2, image: 'a8q1bzQ_700b.jpg'},
  { id: 3, image: 'a8q1bzQ_700b.jpg'},
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

  const [drawings, setDrawings] = useState<Drawing[]>(initialDrawings);
  const [drawingImages, setDrawingImages] = useState<DrawingImage[]>(drawingImageList);
  const [selectedDrawing, setSelectedDrawing] = React.useState<Drawing>(initialDrawings[0]);

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
    fetchDrawings();
  }, []);

  async function fetchProduct() {
    const getProductResponse = await productService.getProduct(Number(projectId), Number(productId));
    setProduct(getProductResponse.product);
    const getProductProtocolsResponse = await productService.getProtocols(Number(projectId), Number(productId));
    setProtocols(getProductProtocolsResponse.protocols);
  }

  async function fetchDrawings() {
    const response = await drawingService.getDrawings(Number(productId));
    await fetchPictures(response.drawings);

    console.log(response.drawings);
    setDrawings(response.drawings);
    setSelectedDrawing(response.drawings[0]);
  }

  async function fetchPictures(drawings: Drawing[]) {
    const copiedImages: DrawingImage[] = [];
    drawings.forEach( async (item, index) => {
      const imageBase64 = await pictureService.getPicture('company', item.imageName);
      const newDrawingImage: DrawingImage = {id: item.id, image: imageBase64};
      copiedImages.push(newDrawingImage);
    });
    console.log(copiedImages);
    setDrawingImages(copiedImages);
  }

  function mapDefectToArea(defects: Defect[]): IArea[] {
    return defects.map(d => { 
      const area: IArea = {
        unit: '%',
        x: d.xPosition, 
        y: d.yPosition, 
        width: d.width, 
        height: d.height
      };
      return area;
    });
  } 

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


  const customRender = (areaProps: IAreaRendererProps) => {
    if (!areaProps.isChanging) {
      return (
        <div key={areaProps.areaNumber}>
        </div>
      );
    }
  };
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
                    areas={selectedDrawing?.defects != null ? mapDefectToArea(selectedDrawing?.defects) : []}
                    unit='percentage'
                    onChange={onChangeHandler}
                    customAreaRenderer={customRender}
                    wrapperStyle={{border: '2px solid black'}}
                  >
                    <img style={{objectFit: 'contain'}} width={'100%'} src={drawingImages.find(x=> x.id == selectedDrawing.id)?.image ?? ''} alt={drawingImages.find(x=> x.id == selectedDrawing.id)?.image ?? ''}/>
                  </AreaSelector>
                </Box>
                <Box sx={{flex: 1, padding: '5px'}}>
                  <ImageList sx={{ width: 200, height: 400 }} cols={1}>
                    {drawings.map((item, index) => (
                      <ImageListItem key={index}>
                        <img
                          style={{objectFit: 'contain'}}
                          width='200px'
                          height='200px'
                          src={drawingImages.find(x=> x.id == item.id)?.image}
                          alt={item.title}
                          loading="lazy"
                        />
                        <ImageListItemBar
                          title={item.title}
                          actionIcon={
                            <IconButton 
                              onClick={() => {setSelectedDrawing(item); console.log(item);}}
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
          <Card sx={{height: '100%', display: 'flex', justifyContent: 'space-between', flexDirection: 'column'}}>
            <CardContent>
              <Typography variant='h5'>2 Defects</Typography>
              <Divider></Divider>
              <List component="nav">
                <ListItemButton alignItems="flex-start" onClick={() => console.log()}>
                  <ListItemAvatar>
                    <Avatar alt="Tomas Vainoris" src="/static/images/avatar/1.jpg" />
                  </ListItemAvatar>
                  <ListItemText
                    primary="Bad weld"
                    secondary={
                      <React.Fragment>
                        <Typography
                          sx={{ display: 'inline' }}
                          component="span"
                          variant="body2"
                          color="text.primary"
                        >
                        </Typography>
                        {"fix it!!"}
                      </React.Fragment>
                    }
                  />
                </ListItemButton>
              </List>
              <Divider></Divider>
              <List>
                <ListItemButton alignItems="flex-start">
                  <ListItemAvatar>
                    <Avatar alt="J B" src="/static/images/avatar/1.jpg" />
                  </ListItemAvatar>
                  <ListItemText
                    primary="Incorrect screw"
                    secondary={
                      <React.Fragment>
                        <Typography
                          sx={{ display: 'inline' }}
                          component="span"
                          variant="body2"
                          color="text.primary"
                        >
                        </Typography>
                        {"it should use different screw"}
                      </React.Fragment>
                    }
                  />
                </ListItemButton>
              </List>
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