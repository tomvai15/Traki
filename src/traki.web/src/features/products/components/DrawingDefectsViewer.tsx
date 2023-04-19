import React, { useEffect, useState } from 'react';
import Box from '@mui/material/Box';
import { Card, CardContent, IconButton, ImageList, ImageListItem, ImageListItemBar } from '@mui/material';
import { useLocation } from 'react-router-dom';
import { AreaSelector, IArea, IAreaRendererProps } from '@bmunozg/react-image-area';
import InfoIcon from '@mui/icons-material/Info';
import { Drawing } from '../../../contracts/drawing/Drawing';
import { Defect } from '../../../contracts/drawing/defect/Defect';
import { drawingService, pictureService } from 'services';
import { DrawingWithImage } from '../types/DrawingWithImage';

export interface SimpleDialogProps {
  open: boolean;
  selectedValue: string;
  addProtocol: (id: number) => void;
  onclose: () => void;
}

type Props = {
  productId: number
}

export function DrawingDefectsViewer({productId}: Props) {
  const {state} = useLocation();

  const [drawings, setDrawings] = useState<DrawingWithImage[]>([]);
  const [defects, setDefects] = useState<Defect[]>([]);

  const [selectedDefect, setSelectedDefect] = useState<Defect>();
  const [selectedDrawing, setSelectedDrawing] = useState<DrawingWithImage>();

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
        <div key={areaProps.areaNumber} 
          style={{width: '100%', height: '100%', borderColor: 'grey', borderWidth: 2, borderStyle: 'dashed'}}>
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

  return (
    <Box sx={{display: 'flex', flexDirection: 'row'}}>
      <Box sx={{padding: '5px', width: '100%'}}>
        {selectedDrawing && <AreaSelector
          areas={mapDefectToArea(selectedDrawing.drawing.defects)}
          unit='percentage'
          wrapperStyle={{ border: '2px solid black' }} 
          customAreaRenderer={customRender}
          onChange={() => {return;}}>
          <img style={{objectFit: 'contain'}} height={350} width={'100%'} src={selectedDrawing.imageBase64}/>
        </AreaSelector>}
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
  );
}