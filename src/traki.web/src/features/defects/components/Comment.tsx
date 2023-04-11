import { Avatar, Box, TextField } from '@mui/material';
import React from 'react';
import ImageWithViewer from 'components/ImageWithViewer';
import { CommentWithImage } from '../types';

type CommentProps = {
  defectComment: CommentWithImage
}

export function Comment ({defectComment}: CommentProps) {
  return (
    <Box sx={{display: 'flex', marginTop: '10px'}}>
      <TextField value={defectComment.defectComment.text} 
        sx={{width: '90%'}} 
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