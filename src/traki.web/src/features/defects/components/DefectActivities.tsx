import React, { useEffect, useState } from 'react';
import { Box, Paper, Typography } from '@mui/material';
import { CommentWithImage, DefectActivity } from '../types';
import { StatusChange } from 'contracts/drawing/defect/StatusChange';
import { Author } from 'contracts/drawing/defect/Author';
import { Activity } from './Activity';

const noneAuthor: Author = {
  id: 0,
  name: '',
  surname: ''
};

type DefectActivitiesProps = {
  defectComments: CommentWithImage[],
  statusChanges: StatusChange[]
}

export function DefectActivities ({defectComments, statusChanges}: DefectActivitiesProps) {
  const [activities, setActivities] = useState<DefectActivity[]>([]);

  useEffect(() => {
    createActivities();
  }, [defectComments, statusChanges]);

  function createActivities() {
    const statusActivities = statusChanges.map((statusChange): DefectActivity => {
      return {
        author: statusChange.author ?? noneAuthor,
        date: statusChange.date,
        statusChange: statusChange
      };
    });

    const commentActivities = defectComments.map((comment): DefectActivity => {
      return {
        author: comment.defectComment.author ?? noneAuthor,
        date: comment.defectComment.date,
        defectComment: comment
      };
    });

    const newActivities = [...statusActivities, ...commentActivities];
    const sortedActivities = newActivities.sort((a,b) => Date.parse(b.date) - Date.parse(a.date));

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

  return (
    <Box sx={{display: 'flex', flexDirection: 'column', gap: '10px'}}>
      {activities.map((item, index) => <Activity key={index} defectActivity={item}/>)}
    </Box>
  );
}
