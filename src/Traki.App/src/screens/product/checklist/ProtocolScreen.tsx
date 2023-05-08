import  React, { useEffect, useState } from 'react';
import { FlatList, View } from 'react-native';
import { Title } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from '../ProductStackParamList';
import protocolService from '../../../services/protocol-service';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { Section } from '../../../contracts/protocol/Section';
import sectionService from '../../../services/section-service';
import ImageView from 'react-native-image-viewing';
import { ScreenView } from '../../../components/ScreenView';
import { ProtocolSection } from '../../../features/protocol/components';

type Props = NativeStackScreenProps<ProductStackParamList, 'Protocol'>;

export default function ProtocolScreen({ route, navigation }: Props) {
  const {protocolId} = route.params;
  
  const [protocol, setProtocol] = useState<Protocol>(initialProtocol);
  const [sections, setSections] = useState<Section[]>([]);

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', async () => {
      await fetchProtocol();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProtocol() {
    const response = await protocolService.getProtocol(protocolId);
    setProtocol(response.protocol);
    const getSectionsResponse = await sectionService.getSections(protocolId);
    setSections(getSectionsResponse.sections);
  }

  const [visible, setIsVisible] = useState(false);
  const [selectedImage, setSelectedImage] = useState<string>('');

  function openSelectedImgae(image: string) {
    setSelectedImage(image);
    setIsVisible(true);
  }

  return (
    <ScreenView>
      { selectedImage &&
        <ImageView
          images={[{uri: selectedImage}]}
          imageIndex={0}
          visible={visible}
          onRequestClose={() => setIsVisible(false)}
        />}
      <View>
        <Title>{protocol.name}</Title>
      </View>
      <FlatList data={sections} 
        showsVerticalScrollIndicator={false}
        keyExtractor={item => item.id.toString()}
        renderItem={ ({item}) =>   
          <ProtocolSection setSelectedImage={openSelectedImgae} protocolId={protocolId} sectionId={item.id}></ProtocolSection>  
        }>
      </FlatList>
    </ScreenView>
  );
}

const initialProtocol: Protocol = {
  id: 1,
  name: '',
  sections: [],
  isTemplate: false,
  modifiedDate: ''
};