import { Avatar, Box, Card, CardContent, Divider, Tab, Tabs, TextField, Typography } from '@mui/material';
import { Comment } from './Comment';
import React, { useEffect, useState } from 'react';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import PhotoCamera from '@mui/icons-material/PhotoCamera';
import { Defect } from '../../contracts/drawing/defect/Defect';
import { DefectComment } from '../../contracts/drawing/defect/DefectComment';
import pictureService from '../../services/picture-service';
import defectService from '../../services/defect-service';
import { CommentWithImage } from '../types/CommentWithImage';


type DefectWithImage = {
  defect: Defect,
  imageBase64: string
}

type DefectDetailsProps = {
  selectedDefect?: Defect,
}

function a11yProps(index: number) {
  return {
    id: `simple-tab-${index}`,
    'aria-controls': `simple-tabpanel-${index}`,
  };
}

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ p: 3 }}>
          <Typography>{children}</Typography>
        </Box>
      )}
    </div>
  );
}

export function DefectDetails ({selectedDefect}: DefectDetailsProps) {

  const [defect, setDefect] = useState<DefectWithImage>();
  const [comments, setComments] = useState<CommentWithImage[]>([]);
  const [tabIndex, setTabIndex] = React.useState(0);

  useEffect(() => {
    fetchDefect();
  }, [selectedDefect]);

  async function fetchDefect() {
    if (selectedDefect == null) {
      return;
    }
    const response = await defectService.getDefect(selectedDefect.drawingId, selectedDefect.id);
    let imageBase64 = '';
    if (response.defect.imageName != '') {
      imageBase64 = await pictureService.getPicture('item', response.defect.imageName);
    } 
    const defectWithImage: DefectWithImage = {
      defect: response.defect,
      imageBase64: imageBase64
    };
    setDefect(defectWithImage);
    if (response.defect.defectComments) {
      await fetchComments(response.defect.defectComments);
    }
  }

  async function fetchComments(defectComments: DefectComment[]) {
    console.log('??');
    const commentsWithImage: CommentWithImage [] = [];

    for (let i = 0; i < defectComments.length; i++) {
      let imageBase64 = '';
      if (defectComments[i].imageName != '') {
        imageBase64 = await pictureService.getPicture('item', defectComments[i].imageName);
      }
      const newCommentWithImage: CommentWithImage = {
        defectComment: defectComments[i], 
        imageBase64: imageBase64
      };

      commentsWithImage.push(newCommentWithImage);
    }
    console.log(commentsWithImage);
    setComments(commentsWithImage);
  }

  return (
    <Card>
      <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
        <Tabs value={tabIndex} onChange={(e, newValue) => setTabIndex(newValue)} aria-label="basic tabs example">
          <Tab label="Defect information" {...a11yProps(0)} />
          <Tab label="New defect" {...a11yProps(1)} />
        </Tabs>
      </Box>
      <TabPanel value={tabIndex} index={0}>
        { selectedDefect == null ? 
          <CardContent sx={{height: 200}}>
          </CardContent> :
          <Box>
            <CardContent>
              <Typography variant='h6'>
                {selectedDefect.title}
              </Typography>
              <Typography>
                {selectedDefect.description}
              </Typography>
            </CardContent>
            <Divider></Divider>
            <CardContent>
              <Typography>Comments</Typography>
              {comments.length == 0 ? 
                <Typography>No comments</Typography> :
                comments.map( (item, index) => <Comment defectComment={item} key={index}/>)}
            </CardContent>
            <Divider></Divider>
            <CardContent>
              <Typography>Add new comment</Typography>
              <Box sx={{display: 'flex', width: '100%'}}>
                <Avatar alt="J B" src="/static/images/avatar/1.jpg" />
                <TextField multiline={true}></TextField>
                <IconButton color="secondary" aria-label="upload picture" component="label">
                  <input hidden accept="image/*" type="file" />
                  <PhotoCamera />
                </IconButton>
              </Box>
            </CardContent>
          </Box>}
      </TabPanel>
      <TabPanel value={tabIndex} index={1}>
        <CardContent>
          <Typography>Add new defect</Typography>
          <Box sx={{display: 'flex', width: '100%'}}>
            <Avatar alt="J B" src="/static/images/avatar/1.jpg" />
            <TextField multiline={true}></TextField>
            <IconButton color="secondary" aria-label="upload picture" component="label">
              <input hidden accept="image/*" type="file" />
              <PhotoCamera />
            </IconButton>
          </Box>
        </CardContent>
      </TabPanel>  
    </Card>
  );
}