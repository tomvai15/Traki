import  React, { useEffect, useState } from 'react';
import { ScrollView, View } from 'react-native';
import { Avatar, Button, Card, Dialog, Divider, List, Portal, Searchbar, Text, Title } from 'react-native-paper';
import { image } from './test';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from './ProductStackParamList';
import checklistService from '../../services/checklist-service';
import { Checklist } from '../../contracts/checklist/Checklist';
import { Template } from '../../contracts/template/Template';
import templateService from '../../services/template-service';
import { FlatList } from 'react-native-gesture-handler';

type Props = NativeStackScreenProps<ProductStackParamList, 'Product'>;

const LeftContent = props => <List.Icon style={{margin:10}} icon="chevron-right" />
const Wrench = props => <Avatar.Icon size={50} style={{backgroundColor:'red'}}  icon="wrench" />

export default function ProductScreen({route, navigation}: Props) {

  const {productId} = route.params;

  const [visible, setVisible] = React.useState(false);
  const [checklists, setChecklists] = useState<Checklist[]>([]);
  const [templates, setTemplates] = useState<Template[]>([]);
  const [templateToAdd, setTemplateToAdd] = useState<number>(-1);
  const [searchQuery, setSearchQuery] = React.useState('');

  const showDialog = async () => {
    setVisible(true); 
    await fetchTemplates();
  };

  const hideDialog = () => {
    setTemplateToAdd(-1);
    setVisible(false)
  };

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchProduct();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProduct() {
    const getChecklistsResposne = await checklistService.getChecklists(productId).catch(err =>console.log(err));
    if (!getChecklistsResposne) {
      return;
    }
    setChecklists(getChecklistsResposne.checklists);
  }

  async function fetchTemplates() {
    const getTemplatesResponse = await templateService.getTemplates().catch(err =>console.log(err));
    if (!getTemplatesResponse) {
      return;
    }
    setTemplates(getTemplatesResponse.templates);
  }

  async function createChecklist() {
    try {
      await checklistService.createChecklist(productId, {templateId: templateToAdd});
      await fetchProduct();
      hideDialog();
    } catch (err) {
      console.log(err);
    }
  }

  return (
    <ScrollView>
      <Portal>
        <Dialog visible={visible} onDismiss={hideDialog}>
          <Dialog.Title>Alert</Dialog.Title>
          <Dialog.Content>
            <Text variant="bodyLarge">This is simple dialog</Text>
            <Searchbar
              placeholder="Search"
              onChangeText={setSearchQuery}
              value={searchQuery}
            />
            <Card mode='outlined' style={{ height: 100, marginTop: 10 }}>
              <FlatList data={templates.filter(x=> x.name.toLowerCase().includes(searchQuery.toLowerCase()))} 
                    keyExtractor={item => item.id.toString()}
                    renderItem={({item}) => 
                      <View style={{margin:5, backgroundColor: item.id==templateToAdd ? '#e4ae3f' : 'white' }}>
                        <Text onPress={()=> setTemplateToAdd(templateToAdd == item.id ? -1 : item.id)} variant="titleMedium">{item.name}</Text>
                        <Divider bold={true}></Divider>
                      </View>}></FlatList>     
            </Card>
          
          </Dialog.Content>
          <Dialog.Actions style={{justifyContent: 'space-evenly'}}>
            <Button style={{ width:80 }} mode='outlined' onPress={hideDialog}>Uždaryti</Button>
            <Button disabled={templateToAdd==-1} 
              style={{ width:80 }} 
              mode='contained' 
              onPress={createChecklist}>
                Pridėti
            </Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>

      <Card mode='outlined'>
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

      <Card mode='outlined' style={{marginTop:10}}>
        <Card.Title title="Pridėti defektą" subtitle="" left={Wrench} />
      </Card>
      <View style={{padding:15, justifyContent: 'space-between', flexDirection: 'row'}}>
        <Title>Inspekcijos</Title>
        <Button onPress={showDialog} icon={'plus'} mode='contained'>Pridėti</Button>
      </View>
      <List.Section style={{marginTop: -10}}>
        {checklists.map((checklist) => 
          (<Card mode='outlined' onPress={() => navigation.navigate('Checklist', {productId: productId, checklistId: checklist.id})}
            key={checklist.id} 
            style={{marginTop:10, padding: 5}}>
            <Card.Title title={checklist.name} subtitle={checklist.standard} right={LeftContent} />
          </Card>))}
      </List.Section>
    </ScrollView>
  );
}
