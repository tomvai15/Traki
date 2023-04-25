import React from 'react';
import { Avatar, Box, Paper, Typography } from '@mui/material';
import { DefectActivity } from '../types';
import { StatusChangeItem } from './StatusChangeItem';
import { Comment } from './Comment';
import { formatDate } from 'utils/dateHelpers';
import { useTheme } from '@mui/material';

type Props = {
  defectActivity: DefectActivity
};

export function Activity ({defectActivity}: Props) {
  const theme = useTheme();

  function formatName() {
    return `${defectActivity.author?.name} ${defectActivity.author?.surname}`;
  }

  function initials() {
    return `${defectActivity.author?.name.toUpperCase()[0]}${defectActivity.author?.surname.toUpperCase()[0]}`;
  }

  function renderActivity() {
    if (defectActivity.defectComment) {
      return (<Comment defectComment={defectActivity.defectComment}/>);
    } else if (defectActivity.statusChange) {
      return (<StatusChangeItem statusChange={defectActivity.statusChange}/>);
    }
    return (<></>);
  }
  return (
    <Paper sx={{padding: '10px', backgroundColor: theme.palette.grey[100] }}>
      <Box sx={{display: 'flex', marginTop: '10px', gap: '10px',  width: '100%'}}>
        <Avatar src={ defectActivity.author.userIconBase64 != undefined ? defectActivity.author.userIconBase64  : "/static/images/avatar/1.jpg" }>
          {initials()}
        </Avatar>
        <Box sx={{ width: '100%'}}> 
          <Box sx={{display: 'flex', gap: '10px'}}>
            <Typography>{formatName()}</Typography>
            <br/>
            <Typography variant='caption'>{formatDate(new Date(defectActivity.date))}</Typography>
          </Box>
          <Box sx={{display: 'flex', flexDirection: 'row', alignItems: 'center', width: '100%'}}>
            {renderActivity()}
          </Box>
        </Box>
      </Box>
    </Paper>
  );
}