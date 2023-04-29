import  React, { useEffect, useState } from 'react';
import { FlatList, View } from 'react-native';
import { Title, ActivityIndicator } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from '../ProductStackParamList';
import { ChecklistQuestion } from '../../../contracts/checklistQuestion/ChecklistQuestion';
import checklistQuestionService from '../../../services/checklistQuestion-service';
import { UpdateChecklistQuestionsRequest } from '../../../contracts/checklistQuestion/UpdateChecklistQuestionsRequest';
import protocolService from '../../../services/protocol-service';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { Section } from '../../../contracts/protocol/Section';
import sectionService from '../../../services/section-service';
import { Checklist } from '../../../contracts/protocol/Checklist';
import ImageView from "react-native-image-viewing";
import { ScreenView } from '../../../components/ScreenView';
import { ProtocolSection } from '../../../features/protocol/components';

type Props = NativeStackScreenProps<ProductStackParamList, 'Protocol'>;

export default function ProtocolScreen({ route, navigation }: Props) {

  const [initialChecklist, setInitialChecklist] = useState<ChecklistQuestion[]>([]);
  const [isLoading, setLoading] = React.useState(false);
  const {productId, protocolId} = route.params;

  const [checklist, setChecklist] = useState<Checklist>();
  const [checklistQuestions, setChecklistQuestions] = useState<ChecklistQuestion[]>([]);
  
  const [protocol, setProtocol] = useState<Protocol>(initialProtocol);
  const [sections, setSections] = useState<Section[]>([]);

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      fetchProtocol();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProtocol() {
    setLoading(true);
    const response = await protocolService.getProtocol(protocolId);
    setProtocol(response.protocol);
    const getSectionsResponse = await sectionService.getSections(protocolId);
    setSections(getSectionsResponse.sections);
    setLoading(false);
  }

  function submitChanges() {
    const updateRequest: UpdateChecklistQuestionsRequest ={
      checklistQuestions: checklistQuestions
    }
    try {
      checklistQuestionService.updateChecklists(productId, updateRequest);
      setInitialChecklist(checklistQuestions.map(x=>Object.assign({},x)));
    } catch (err) {
      console.log(err);
    }
  }

  function areEquivalent(): boolean {
    return JSON.stringify(initialChecklist)==JSON.stringify(checklistQuestions);
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