import React, { useEffect, useState } from 'react';
import { Activity } from './Activity';
import { Author } from '../../../contracts/drawing/defect/Author';
import { CommentWithImage } from '../types/CommentWithImage';
import { StatusChange } from '../../../contracts/drawing/defect/StatusChange';
import { DefectActivity } from '../types/DefectActivity';
import { View } from 'react-native';
import { Text } from 'react-native-paper';

const noneAuthor: Author = {
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
      <View>
        <Text>
          No Activity
        </Text>
      </View>);
  }

  return (
    <View style={{display: 'flex', flexDirection: 'column'}}>
      {activities.map((item, index) => <Activity key={index} defectActivity={item}/>)}
    </View>
  );
}

