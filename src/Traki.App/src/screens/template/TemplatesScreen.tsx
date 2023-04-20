import  React, { useEffect, useState } from 'react';
import { View, FlatList } from 'react-native';
import { Button, List, Searchbar } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { TemplateStackParamList } from './TemplateStackParamList';
import protocolService from '../../services/protocol-service';
import { Protocol } from '../../contracts/protocol/Protocol';

type Props = NativeStackScreenProps<TemplateStackParamList, 'Templates'>;

export default function TemplatesScreen({ navigation }: Props) {

  const [protocols, setProtocols] = useState<Protocol[]>([]);

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchProtocols();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProtocols() {
    const response = await protocolService.getTemplateProtocols();
    setProtocols(response.protocols);
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
        data={protocols.filter(p => p.name.toLowerCase().includes(searchQuery.toLowerCase()))}
        renderItem={({item}) => <List.Item onPress={() => navigation.navigate('Template', { protocolId: item.id })}
          title={item.name}
          description={'Modified in '}
          left={props => <List.Icon {...props} icon='folder' />}
        />}
        keyExtractor={item => item.id.toString()}
      />
    </View>
  );
}