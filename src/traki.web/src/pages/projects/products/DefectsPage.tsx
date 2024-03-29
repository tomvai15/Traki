import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Avatar, Breadcrumbs, Card, CardContent, Checkbox, FormControlLabel, Grid, IconButton, ImageList, ImageListItem, ImageListItemBar, List, ListItemAvatar, ListItemButton, ListItemText, Stack, Typography } from '@mui/material';
import { useLocation, useParams } from 'react-router-dom';
import { AreaSelector, IArea, IAreaRendererProps } from '@bmunozg/react-image-area';
import InfoIcon from '@mui/icons-material/Info';
import { Drawing } from '../../../contracts/drawing/Drawing';
import { Defect } from '../../../contracts/drawing/defect/Defect';
import { DefectStatus } from '../../../contracts/drawing/defect/DefectStatus';
import { CreateDefectRequest } from '../../../contracts/drawing/defect/CreateDefectRequest';
import { Link as BreadLink } from '@mui/material';
import { DefectDetails } from 'features/defects/components/DefectDetails';
import { drawingService, pictureService, defectService, productService, projectService } from 'services';
import { DrawingWithImage } from 'features/products/types/DrawingWithImage';
import { PaginatedList } from 'components/PaginatedList';
import { useAlert } from 'hooks/useAlert';
import { useNotFoundCatcher } from 'hooks/useNotFoundCatcher';

export interface SimpleDialogProps {
  open: boolean;
  selectedValue: string;
  addProtocol: (id: number) => void;
  onclose: () => void;
}

export function DefectsPage() {
  const {state} = useLocation();
  const { displaySuccess  } = useAlert();
  const { projectId, productId } = useParams();

  const [drawings, setDrawings] = useState<DrawingWithImage[]>([]);
  const [defects, setDefects] = useState<Defect[]>([]);

  const [selectedDefect, setSelectedDefect] = useState<Defect>();
  const [selectedDrawing, setSelectedDrawing] = useState<DrawingWithImage>();

  const [tabIndex, setTabIndex] = useState(0);
  const [includeFixedDefects, setIncludeFixedDefects] = useState(false);

  const {catchNotFound} = useNotFoundCatcher();

  useEffect(() => {
    catchNotFound(() => fetchDrawings());
  }, []);

  async function fetchDrawings() {
    const response = await drawingService.getDrawings(Number(productId));
    await fetchDrawingPictures(response.drawings);
    await fetchProduct();
  }

  async function fetchProduct() {
    await projectService.getProject(Number(projectId));
    await productService.getProduct(Number(projectId), Number(productId));
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

    if (!state) {
      setSelectedDefect(newDefects[0]);
      setSelectedDrawing(drawingsWithImage[0]);
    } else {
      const foundDefect = newDefects.find(x => x.id == state.defectId);
      if (foundDefect) {
        setSelectedDefect(foundDefect);
        const foundDrawing = drawingsWithImage.find(x=> x.drawing.id == foundDefect.drawingId);
        if (foundDrawing == null) {
          return;
        }
        setSelectedDrawing(foundDrawing);
      } else {
        setSelectedDefect(newDefects[0]);
        setSelectedDrawing(drawingsWithImage[0]);
      }
    }

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
      drawingId: 0,
      creationDate: new Date()
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
    displaySuccess(`Defect ${newDefect.title} was created successfully`);

    if (response.defect) {
      setSelectedDefect(response.defect);
    }
  }

  return (
    <Grid container item xs={12} spacing={1}>
      <Grid item xs={12} md={12}>
        <Breadcrumbs aria-label="breadcrumb">
          <BreadLink color="inherit" href="/projects">
            Projects
          </BreadLink>
          <BreadLink color="inherit" href={`/projects/${projectId}/products/${productId}`}>
            Product
          </BreadLink>
          <Typography color="text.primary">Defects</Typography>
        </Breadcrumbs>
      </Grid>
      <Grid item xs={5}>
        <DefectDetails canSubmitDefect={areas.length>0} tabIndex={tabIndex} setTabIndex={setTabIndex} createDefect={createDefect} onSelectNew={() => setIsNewDefect(true)} onSelectInformation={() => setIsNewDefect(false)}  selectedDefect={selectedDefect}/>
      </Grid>
      <Grid item xs={7} spacing={1}>
        <Stack spacing={1}>
          <Grid item xs={12}>
            <Card>
              <CardContent>
                <Box sx={{display: 'flex', flexDirection: 'row'}}>
                  <Box sx={{padding: '5px', width: '100%'}}>
                    { isNewDefect ?
                      (selectedDrawing && <AreaSelector
                        maxAreas={1}
                        areas={areas}
                        unit='percentage'
                        wrapperStyle={{ border: '2px solid black' }} 
                        customAreaRenderer={customRenderSelection}
                        onChange={setAreas}>
                        <img id="drawing" style={{objectFit: 'contain'}} height={350} width={'100%'} src={selectedDrawing.imageBase64}/>
                      </AreaSelector>) :
                      (selectedDrawing && <AreaSelector
                        areas={mapDefectToArea(selectedDrawing.drawing.defects.filter(x=> x.status >= Number(!includeFixedDefects)))}
                        unit='percentage'
                        wrapperStyle={{ border: '2px solid black' }} 
                        customAreaRenderer={customRender}
                        onChange={() => {return;}}>
                        <img style={{objectFit: 'contain'}} height={350} width={'100%'} src={selectedDrawing.imageBase64}/>
                      </AreaSelector>)}
                  </Box>
                  <Box sx={{padding: '5px'}}>
                    <ImageList sx={{ width: 180, height: 340 }} cols={1}>
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
                                onClick={() => {setSelectedDrawing(item);}}
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
                <FormControlLabel control={<Checkbox defaultChecked />} onChange={()=> setIncludeFixedDefects(!includeFixedDefects)} checked={includeFixedDefects} label="Include fixed defects" />
                <List component="nav">
                  <PaginatedList heightPerItem={100} items={defects.filter(x=> x.status >= Number(!includeFixedDefects))} renderItem={(item) =>
                    <ListItemButton alignItems="flex-start" onClick={() => setSelectedDefectAndDrawing(item)}>
                      <ListItemAvatar>
                        <Avatar alt="Tomas Vainoris" src={  item.author?.userIconBase64 ? item.author.userIconBase64 :  "/static/images/avatar/1.jpg"}>
                          { item.author?.name.toUpperCase()[0] + '' +  item.author?.surname.toUpperCase()[0]}
                        </Avatar>
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
                    </ListItemButton>}/>
                </List>
              </CardContent>
            </Card>
          </Grid>
        </Stack>
      </Grid>
    </Grid>
  );
}