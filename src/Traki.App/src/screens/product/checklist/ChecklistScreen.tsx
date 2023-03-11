import  React, { useEffect, useState } from 'react';
import { View } from 'react-native';
import { Text } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from '../ProductStackParamList';
import { Checklist } from '../../../contracts/checklist/Checklist';
import checklistService from '../../../services/checklist-service';

type Props = NativeStackScreenProps<ProductStackParamList, 'Checklist'>;

export default function ChecklistScreen({ route, navigation }: Props) {

  const {productId, checklistId} = route.params;

  const [checklist, setChecklist] = useState<Checklist>();

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchData();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchData() {
  
    const getChecklistResposne = await checklistService.getChecklist(productId, checklistId).catch(err =>console.log(err));
    if (!getChecklistResposne) {
      return;
    }
    setChecklist(getChecklistResposne.checklist);
  }

  return (
    <View style={{ flex: 1}}>
      <Text>{checklist?.name}</Text>
    </View>
  );
}