import  React, { useEffect, useState } from 'react';
import { View, FlatList } from 'react-native';
import { Button, List, Searchbar, Text } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { TemplateStackParamList } from './TemplateStackParamList';
import productService from '../../services/product-service';
import { Template } from '../../contracts/template/Template';
import { GetTemplateResponse } from '../../contracts/template/GetTemplateResponse';
import templateService from '../../services/template-service';
import { useDispatch } from 'react-redux';
import { setMessage } from '../../store/message-slice';
import { useSelector } from 'react-redux';
import { RootState } from '../../store/store';

type Props = NativeStackScreenProps<TemplateStackParamList, 'Templates'>;

export default function TemplatesScreen({ navigation }: Props) {

  const dispatch = useDispatch();

  const { message } = useSelector((state: RootState) => state.message);

  const [templates, setTemplates] = useState<Template[]>([]);


  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchProjects();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProjects() {
    const getTemplatesResponse = await templateService.getTemplates().catch(err =>console.log(err));
    if (!getTemplatesResponse) {
      return;
    }
    setTemplates(getTemplatesResponse.templates);
  }

  const [searchQuery, setSearchQuery] = React.useState('');

  const onChangeSearch = (query: string) => setSearchQuery(query);

  return (
    <View style={{ flex: 1}}>
       <Searchbar
          placeholder="Search"
          onChangeText={onChangeSearch}
          value={searchQuery}
        />
      <FlatList
        data={templates.filter(p => p.name.toLowerCase().includes(searchQuery.toLowerCase()))}
        renderItem={({item}) => <List.Item onPress={() => navigation.navigate('Template', { id: 1 })}
          title={item.name}
          description={item.standard}
          left={props => <List.Icon {...props} icon='folder' />}
        />}
        keyExtractor={item => item.id.toString()}
      />
      <Text>{message}</Text>
      <Button mode='contained' onPress={() => navigation.navigate('CreateTemplate')}>Naujas šablonas</Button>
      <Button mode='contained' onPress={() => dispatch(setMessage('bybys'))}>TEst</Button>
      <Button mode='contained' onPress={() => dispatch(setMessage('Kiausai'))}>TEst</Button>
    </View>
  );
}