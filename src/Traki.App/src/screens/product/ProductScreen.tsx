import  React, { useEffect, useState } from 'react';
import { ScrollView } from 'react-native';
import { Avatar, Button, Card, List, Text } from 'react-native-paper';
import { image } from './test';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from './ProductStackParamList';
import checklistService from '../../services/checklist-service';
import { Checklist } from '../../contracts/checklist/Checklist';
import { Item } from 'react-native-paper/lib/typescript/components/Drawer/Drawer';

type Props = NativeStackScreenProps<ProductStackParamList, 'Product'>;

const LeftContent = props => <Text variant="titleLarge">10/14</Text>
const Wrench = props => <Avatar.Icon size={50} style={{backgroundColor:'red'}}  icon="wrench" />

export default function ProductScreen({route, navigation}: Props) {

  const {productId} = route.params;

  const [checklists, setChecklists] = useState<Checklist[]>([]);

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchProjects();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProjects() {
    const getChecklistsResposne = await checklistService.getChecklists(productId).catch(err =>console.log(err));
    if (!getChecklistsResposne) {
      return;
    }
    setChecklists(getChecklistsResposne.checklists);
  }

  return (
    <ScrollView>
      <Card>
        <Card.Content >
          <Text variant="titleLarge">Šiluminis mazgas</Text>
          <Text variant="bodyMedium">Busena: gaminamas</Text>
        </Card.Content>
        <Card.Cover style={{height:300}} source={{ uri: image }} />
        <Card.Actions>
          <Button>Redaguoti</Button>
          <Button>Keisti Būseną</Button>
        </Card.Actions>
      </Card>

      <Card style={{marginTop:10}}>
        <Card.Title title="Pridėti defektą" subtitle="" left={Wrench} />
      </Card>

      <List.Section title='Inspekcijos'>
        {checklists.map((checklist) => 
          (<Card onPress={() => navigation.navigate('Checklist', {productId: productId, checklistId: checklist.id})}
            key={checklist.id} 
            style={{marginTop:10, padding: 5}}>
            <Card.Title title={checklist.name} subtitle={checklist.standard} right={LeftContent} />
          </Card>))}
      </List.Section>
    </ScrollView>
  );
}
