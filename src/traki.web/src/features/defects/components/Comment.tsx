import React from 'react';
import { Box, TextField } from '@mui/material';
import ImageWithViewer from 'components/ImageWithViewer';
import { CommentWithImage } from '../types';

type CommentProps = {
  defectComment: CommentWithImage
}

export function Comment ({defectComment}: CommentProps) {
  return (
    <Box sx={{display: 'flex', marginTop: '10px', width: '100%'}}>
      <TextField value={defectComment.defectComment.text} 
        sx={{padding: 0,  width: '100%', marginRight: '10px'}}
        multiline={true}
        focused={false}
        InputProps={{
          readOnly: true,
          "aria-selected": 'false',
          style: {
            padding: 0
          }
        }}>
      </TextField>
      <ImageWithViewer source={defectComment.imageBase64} height={100}></ImageWithViewer>
    </Box>
  );
}