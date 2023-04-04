import { Avatar, Box, Card, CardContent, CardMedia, Divider, Tab, Tabs, TextField, Typography } from '@mui/material';
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
import { v4 as uuid } from 'uuid';

type DefectWithImage = {
  defect: Defect,
  imageBase64: string
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

type DefectDetailsProps = {
  selectedDefect?: Defect,
  onSelectInformation: () => void,
  onSelectNew: () => void,
  createDefect: (title: string, description: string, imageName?: string, image?: FormData) => void
  tabIndex: number,
  setTabIndex: (value: number) => void
}

export function DefectDetails ({selectedDefect, onSelectInformation, onSelectNew, createDefect, tabIndex, setTabIndex}: DefectDetailsProps) {

  const [title, setTitle] = useState<string>('');
  const [description, setDescription] = useState<string>('');
  const [defect, setDefect] = useState<DefectWithImage>();
  const [comments, setComments] = useState<CommentWithImage[]>([]);

  const [previewImage, setPreviewImage] = useState<string>('');
  const [file, setFile] = useState<File>();
  
  const selectFile = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { files } = event.target;
    const selectedFiles = files as FileList;
    setFile(selectedFiles?.[0]);
    setPreviewImage(URL.createObjectURL(selectedFiles?.[0]));
  };

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

  function onSubmit() {
    if (file) {
      const pictureName = `${uuid()}${file.type.replace('image/','.')}`;

      const formData = new FormData();
      formData.append(pictureName, file, pictureName);
  
      createDefect(title, description, pictureName, formData);
    } else {
      createDefect(title, description, undefined, undefined);
    }
  }

  return (
    <Card>
      <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
        <Tabs value={tabIndex} onChange={(e, newValue) => setTabIndex(newValue)} aria-label="basic tabs example">
          <Tab label="Defect information" {...a11yProps(0)} onClick={() => onSelectInformation()}/>
          <Tab label="New defect" {...a11yProps(1)} onClick={() => onSelectNew()}/>
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
                  <input hidden accept="image/*" type="file" onChange={selectFile} />
                  <PhotoCamera />
                </IconButton>
              </Box>
            </CardContent>
          </Box>}
      </TabPanel>
      <TabPanel value={tabIndex} index={1}>
        <CardContent>
          <Typography>Add new defect</Typography>
          <TextField value={title} onChange={(e) => setTitle(e.target.value)} label='Title' multiline={false} sx={{width: '90%', marginBottom: '10px'}} ></TextField>
          <Box sx={{display: 'flex', width: '100%'}}>
            <TextField value={description} onChange={(e) => setDescription(e.target.value)} label='Description' multiline={true} sx={{width: '90%'}}></TextField>
            <IconButton color="secondary" aria-label="upload picture" component="label">
              <input hidden accept="image/*" type="file" onChange={selectFile} />
              <PhotoCamera />
            </IconButton>
          </Box>
        </CardContent>
        <CardContent>
          <Box sx={{display: 'flex', flexDirection: 'row',  justifyContent: 'space-between'}}>
            <Button onClick={onSubmit} sx={{height: 40}} variant='contained'>Submit</Button>
            { previewImage && <img
              height={200}
              width='auto'
              src={previewImage}
              alt="random"
            />}
          </Box>
        </CardContent>
      </TabPanel>  
    </Card>
  );
}