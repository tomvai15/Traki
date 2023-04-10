import React from 'react';
import { Avatar, Box, Chip, Paper, TextField, Typography } from '@mui/material';
import { CommentWithImage } from '../types/CommentWithImage';
import ImageWithViewer from '../ImageWithViewer';
import { StatusChange } from 'contracts/drawing/defect/StatusChange';
import { DefectStatus } from 'contracts/drawing/defect/DefectStatus';

type StatusChangeItemProps = {
  statusChange: StatusChange
}

function formatDate(date: Date): string {
  return date.toLocaleString();
}

export function StatusChangeItem ({statusChange}: StatusChangeItemProps) {
  return (
    <Box sx={{display: 'flex', marginTop: '10px', gap: '10px'}}>
      <Avatar alt="J B" src="/static/images/avatar/1.jpg" />
      <Box>
        <Box sx={{display: 'flex', gap: '10px'}}>
          <Typography>Jotaro Kujo</Typography>
          <br/>
          <Typography variant='caption'>{formatDate(new Date(statusChange.date))}</Typography>
        </Box>
        <Typography>Updated status:</Typography>
        <Chip label={DefectStatus[statusChange.from]}></Chip> to <Chip color='primary' label={DefectStatus[statusChange.to]}></Chip>
      </Box>
    </Box>
  );
}