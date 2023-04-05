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
import { DefectDetails } from '../../../components/defect/DefectDetails';
import { Rectangle } from '../../../components/types/Rectangle';
import defectService from '../../../services/defect-service';
import { CreateDefectRequest } from '../../../contracts/drawing/defect/CreateDefectRequest';

type DrawingImage = {
  id: number,
  image: string
};


export interface SimpleDialogProps {
  open: boolean;
  selectedValue: string;
  addProtocol: (id: number) => void;
  onclose: () => void;
}

const emails = ['username@gmail.com', 'user02@gmail.com'];

type DrawingWithImage = {
  drawing: Drawing,
  imageBase64: string
}

export function DefectsPage() {
  const navigate = useNavigate();
  const { projectId, productId } = useParams();

  const [drawings, setDrawings] = useState<DrawingWithImage[]>([]);
  const [defects, setDefects] = useState<Defect[]>([]);

  const [selectedDefect, setSelectedDefect] = useState<Defect>();
  const [selectedDrawing, setSelectedDrawing] = useState<DrawingWithImage>();

  const [tabIndex, setTabIndex] = React.useState(0);

  useEffect(() => {
    fetchDrawings();
  }, []);

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

    setSelectedDefect(newDefects[0]);
    setSelectedDrawing(drawingsWithImage[0]);
    setDefects(newDefects);
    setDrawings(drawingsWithImage);
  }

  function mapDefectToArea(defects: Defect[]): IArea[] {
    return defects.map(d => { 
      const area: IArea = {
        unit: '%',
        x: d.x, 
        y: d.y, 
        width: d.width, 
        height: d.height
      };
      return area;
    });
  } 

  const customRender = (areaProps: IAreaRendererProps) => {
    if (!areaProps.isChanging) {
      return (
        isSelectedRegion(areaProps.areaNumber) ? 
          <div key={areaProps.areaNumber} 
            onClick={() => setSelectedDefectById(areaProps.areaNumber)} 
            style={{width: '100%', height: '100%', borderColor: 'blue', borderWidth: 2, borderStyle: 'dashed'}}>
          </div> :
          <div key={areaProps.areaNumber} 
            onClick={() => setSelectedDefectById(areaProps.areaNumber)} 
            style={{width: '100%', height: '100%', borderColor: 'grey', borderWidth: 2, borderStyle: 'dashed'}}>
          </div>
      );
    }
  };

  const customRenderSelection = (areaProps: IAreaRendererProps) => {
    if (!areaProps.isChanging) {
      return (
        <div key={areaProps.areaNumber} 
          onClick={() => setSelectedDefectById(areaProps.areaNumber)} 
          style={{width: '100%', height: '100%', borderColor: 'orange', borderWidth: 2, borderStyle: 'dashed'}}>
        </div>
      );
    }
  };

  function isSelectedRegion(areaNumber: number) {
    const foundDefect = selectedDrawing?.drawing.defects[areaNumber-1];
    return selectedDefect?.id == foundDefect?.id;
  }

  function setSelectedDefectById(defectIndex: number) {
    const foundDefect = selectedDrawing?.drawing.defects[defectIndex-1];
    setSelectedDefect(foundDefect);
  }

  function setSelectedDefectAndDrawing(defect: Defect) {
    setSelectedDefect(defect);
    const foundDrawing = drawings.find(x=> x.drawing.id == defect.drawingId);
    if (foundDrawing == null) {
      return;
    }
    setSelectedDrawing(foundDrawing);
  }

  const [areas, setAreas] = useState<IArea[]>([]);
  const [isNewDefect, setIsNewDefect] = useState<boolean>(false);

  async function createDefect (title: string, description: string, imageName?: string, image?: FormData) {
    if (selectedDrawing == null) {
      return;
    }

    if (areas.length < 1) {
      return;
    }

    const rectangle = areas[0];

    const newDefect: Defect = {
      id: 0,
      title: title,
      description: description,
      imageName: imageName ? imageName : '',
      status: DefectStatus.NotFixed,
      x: rectangle.x,
      y: rectangle.y,
      width: rectangle.width,
      height: rectangle.height,
      drawingId: 0
    };

    const request: CreateDefectRequest = {
      defect: newDefect
    };

    if (image) {
      await pictureService.uploadPicturesFormData('item', image);
    }

    const response = await defectService.createDefect(selectedDrawing?.drawing.id, request);

    await fetchDrawings();

    setIsNewDefect(false);
    setTabIndex(0);

    if (response.defect) {
      setSelectedDefect(response.defect);
    }
  }

  return (
    <Grid container item xs={12} spacing={1}>
      <Grid item xs={5}>
        <DefectDetails canSubmitDefect={areas.length>0} tabIndex={tabIndex} setTabIndex={setTabIndex} createDefect={createDefect} onSelectNew={() => setIsNewDefect(true)} onSelectInformation={() => setIsNewDefect(false)}  selectedDefect={selectedDefect}/>
      </Grid>
      <Grid container item xs={7} spacing={1}>
        <Grid item xs={12}>
          <Card>
            <CardContent>
              <Box sx={{display: 'flex', flexDirection: 'row'}}>
                <Box sx={{padding: '5px', width: '100%', height: '100%'}}>
                  { isNewDefect ?
                    (selectedDrawing && <AreaSelector
                      maxAreas={1}
                      areas={areas}
                      unit='percentage'
                      wrapperStyle={{ border: '2px solid black' }} 
                      customAreaRenderer={customRenderSelection}
                      onChange={setAreas}>
                      <img style={{objectFit: 'contain'}} height={350} width={'100%'} src={selectedDrawing.imageBase64}/>
                    </AreaSelector>) :
                    (selectedDrawing && <AreaSelector
                      areas={mapDefectToArea(selectedDrawing.drawing.defects)}
                      unit='percentage'
                      wrapperStyle={{ border: '2px solid black' }} 
                      customAreaRenderer={customRender}
                      onChange={() => {return;}}>
                      <img style={{objectFit: 'contain'}} height={350} width={'100%'} src={selectedDrawing.imageBase64}/>
                    </AreaSelector>)}
                </Box>
                <Box sx={{padding: '5px'}}>
                  <ImageList sx={{ width: 180, height: 300 }} cols={1}>
                    {drawings.map((item, index) => (
                      <ImageListItem key={index}>
                        <img
                          style={{objectFit: 'contain'}}
                          width='200px'
                          height='200px'
                          src={item.imageBase64}
                          alt={item.drawing.title}
                          loading="lazy"
                        />
                        <ImageListItemBar
                          title={item.drawing.title}
                          actionIcon={
                            <IconButton 
                              onClick={() => {setSelectedDrawing(item); console.log(item);}}
                              sx={{ color: 'rgba(255, 255, 255, 0.54)' }}
                              aria-label={`info about ${item.drawing.title}`}
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
        <Grid item xs={12}>
          <Card>
            <CardContent>
              <List component="nav" sx={{overflow: 'auto', maxHeight: 250}}>
                { defects.map((item, index) =>
                  <ListItemButton key={index} alignItems="flex-start" onClick={() => setSelectedDefectAndDrawing(item)}>
                    <ListItemAvatar>
                      <Avatar alt="Tomas Vainoris" src="/static/images/avatar/1.jpg" />
                    </ListItemAvatar>
                    <ListItemText
                      primary={item.title}
                      secondary={
                        <React.Fragment>
                          <Typography
                            sx={{ display: 'inline' }}
                            component="span"
                            variant="body2"
                            color="text.primary"
                          >
                          </Typography>
                          {item.description}
                        </React.Fragment>
                      }
                    />
                  </ListItemButton>)}
              </List>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Grid>
  );
}