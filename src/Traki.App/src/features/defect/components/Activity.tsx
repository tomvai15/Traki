import React from 'react';
import { StatusChangeItem } from './StatusChangeItem';
import { View, StyleSheet } from 'react-native';
import { Card, Text } from 'react-native-paper';
import { formatDate } from '../../../utils/dateHelpers';
import { DefectActivity } from '../types/DefectActivity';
import { Comment } from './Comment';
import { CustomAvatar } from '../../../components/CustomAvatar';

type Props = {
  defectActivity: DefectActivity
};

export function Activity ({defectActivity}: Props) {
  function formatName() {
    return `${defectActivity.author?.name} ${defectActivity.author?.surname}`;
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
    <Card mode='outlined' style={{marginTop:10}}>
      <Card.Content>
        <View style={{display: 'flex'}}>
          <View> 
            <View style={styles.nameContainer}>
              <View style={{display: 'flex', flexDirection: 'row', alignItems: 'center'}}>
                <CustomAvatar user={defectActivity.author} size={40}/>
                <Text style={{fontSize: 16, marginLeft: 5}}>{formatName()}</Text>
              </View>
              <Text>{formatDate(new Date(defectActivity.date))}</Text>
            </View>
            <View style={{display: 'flex', flexDirection: 'row', alignItems: 'center', marginTop: 10}}>
              {renderActivity()}
            </View>
          </View>
        </View>
      </Card.Content>
    </Card>
  );
}

const styles = StyleSheet.create({
  itemsCenter: {alignItems: 'center'},
  nameContainer: {
    display: 'flex', flexDirection: 'row', gap: 10, justifyContent: 'space-between', alignItems: 'center'
  },
});
