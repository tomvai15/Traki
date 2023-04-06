import { Avatar, Box, TextField } from '@mui/material';
import React from 'react';
import { CommentWithImage } from '../types/CommentWithImage';
import ImageWithViewer from '../ImageWithViewer';

type CommentProps = {
  defectComment: CommentWithImage
}

export function Comment ({defectComment}: CommentProps) {
  return (
    <Box sx={{display: 'flex', marginTop: '10px'}}>
      <Avatar alt="J B" src="/static/images/avatar/1.jpg" />
      <TextField value={defectComment.defectComment.text} 
        sx={{marginLeft: '10px', width: '90%'}} 
        multiline={true}
        InputProps={{
          readOnly: true,
        }}
        variant="filled">
      </TextField>
      <ImageWithViewer source={defectComment.imageBase64} height={100}></ImageWithViewer>
    </Box>
  );
}