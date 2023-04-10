import React, { useEffect, useState } from 'react';
import { Box, Paper, Typography } from '@mui/material';
import { CommentWithImage } from '../types/CommentWithImage';
import { StatusChange } from 'contracts/drawing/defect/StatusChange';
import { Comment } from './Comment';
import { StatusChangeItem } from './StatusChangeItem';
import { useTheme } from '@mui/material';

type DefectActivity = {
  date: string,
  defectComment?: CommentWithImage,
  statusChange?: StatusChange
}

type DefectActivitiesProps = {
  defectComments: CommentWithImage[],
  statusChanges: StatusChange[]
}

export function DefectActivities ({defectComments, statusChanges}: DefectActivitiesProps) {
  const theme = useTheme();

  const [activities, setActivities] = useState<DefectActivity[]>([]);

  useEffect(() => {
    createActivities();
  }, [defectComments, statusChanges]);

  function createActivities() {
    const statusActivities = statusChanges.map((statusChange): DefectActivity => {
      return {
        date: statusChange.date,
        statusChange: statusChange
      };
    });

    const commentActivities = defectComments.map((comment): DefectActivity => {
      return {
        date: comment.defectComment.date,
        defectComment: comment
      };
    });

    const newActivities = [...statusActivities, ...commentActivities];
    const sortedActivities = newActivities.sort((a,b) => Date.parse(a.date) - Date.parse(b.date));

    setActivities(sortedActivities);
  }

  if (activities.length == 0) {
    return (
      <Box>
        <Typography color="grey" variant='subtitle2'>
        No comments
        </Typography>
      </Box>);
  }

  function renderActivity(defectActivity: DefectActivity) {
    if (defectActivity.defectComment) {
      return (<Comment defectComment={defectActivity.defectComment}/>);
    } else if (defectActivity.statusChange) {
      return (<StatusChangeItem statusChange={defectActivity.statusChange}/>);
    }
    return (<></>);
  }

  return (
    <Box sx={{display: 'flex', flexDirection: 'column', gap: '10px'}}>
      {activities.map((item, index) => 
        <Paper key={index} sx={{padding: '10px', backgroundColor: theme.palette.secondary.light }}>
          {renderActivity(item)}
        </Paper>)}
    </Box>
  );
}
