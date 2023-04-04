import { Avatar, Box, TextField } from '@mui/material';
import React from 'react';
import { CommentWithImage } from '../types/CommentWithImage';

type CommentProps = {
  defectComment: CommentWithImage
}

export function Comment ({defectComment}: CommentProps) {
  return (
    <Box sx={{display: 'flex'}}>
      <Avatar alt="J B" src="/static/images/avatar/1.jpg" />
      <TextField value={defectComment.defectComment.text} multiline={true}></TextField>
    </Box>
  );
}