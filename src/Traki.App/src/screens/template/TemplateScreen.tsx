import  React, { useEffect, useState } from 'react';
import { StyleSheet, View, FlatList, Text, ActivityIndicator, TextInput, TouchableHighlight, Image } from 'react-native';
import { Button, Card, Checkbox, Divider, IconButton, List, Paragraph, SegmentedButtons, Title } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { TemplateStackParamList } from './TemplateStackParamList';
import protocolService from '../../services/protocol-service';
import { sectionService } from '../../services';
import { Checklist } from '../../contracts/protocol/Checklist';
import { Protocol } from '../../contracts/protocol/Protocol';
import { Section } from '../../contracts/protocol/Section';
import { ScreenView } from '../../components/ScreenView';
import ImageView from "react-native-image-viewing";
import { Item } from '../../contracts/protocol/items/Item';

type Props = NativeStackScreenProps<TemplateStackParamList, 'Template'>;

export default function TemplateScreen({ route, navigation }: Props) {
  const [isLoading, setLoading] = React.useState(false);
  const {protocolId} = route.params;
  
  const [protocol, setProtocol] = useState<Protocol>(initialProtocol);
  const [sections, setSections] = useState<Section[]>([]);

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      fetchProtocol();
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
      { isLoading ?
      <View>
        <ActivityIndicator animating={isLoading}/>
      </View> :
      <FlatList data={sections} 
        showsVerticalScrollIndicator={false}
        keyExtractor={item => item.id.toString()}
        renderItem={ ({item}) =>   
          <ProtocolSection setSelectedImage={openSelectedImgae} protocolId={protocolId} sectionId={item.id}></ProtocolSection>  
        }>
      </FlatList>}
    </ScreenView>
  );
}

type ProtocolSectionProps = {
  protocolId: number,
  sectionId: number,
  setSelectedImage: (image: string) => void
}

type ItemImage = {
  id: string,
  isLocal: boolean,
  localImageUri: string,
  imageName: string,
  imageBase64: string
}

function ProtocolSection({ protocolId, sectionId, setSelectedImage }: ProtocolSectionProps) {

  const [section, setSection] = useState<Section>(initialSection);
  const [initialSectionJson, setInitialSectionJson] = useState<string>('');
  const [itemImages, setItemImages] = useState<ItemImage[]>([]);
  const [initialItemImagesJson, setInitialItemImagesJson] = useState<string>('');

  useEffect(() => {
    fetchSection();
  }, []);

  async function fetchSection() {
    const getSectionResponse = await sectionService.getSection(Number(protocolId), Number(sectionId));
    console.log(getSectionResponse);
    orderAndSetSection(getSectionResponse.section);
  }

  function orderAndSetSection(sectionToSort: Section) {
    if (!sectionToSort.checklist)
    {
      setSection(sectionToSort);
      setInitialSectionJson(JSON.stringify(sectionToSort));
      return;
    }
    const sortedItems = [...sectionToSort.checklist.items];
    sortedItems.sort((a, b) => a.priority - b.priority);

    const copiedChecklist: Checklist = {...sectionToSort.checklist, items: sortedItems};
    const copiedSection: Section = {...sectionToSort, checklist: copiedChecklist};
    setSection(copiedSection);
    setInitialSectionJson(JSON.stringify(copiedSection));
  }

  return (
    <View>
      <View style={{display: 'flex', flexDirection: 'row', alignItems: 'flex-start', justifyContent: 'space-between'}}>
        <Title style={{fontSize: 15, width: '70%'}}>{section.name}</Title>
      </View>
      <FlatList data={section.checklist?.items} 
        keyExtractor={item => item.id.toString()}
        renderItem={ ({item}) =>   
          <ProtocolSectionItem setSelectedImage={setSelectedImage} item={item} itemImage={itemImages.find(x=> x.id==item.id)}></ProtocolSectionItem>  
        }>
        </FlatList>
        <Divider bold></Divider>
    </View>
  );
}

type ProtocolSectionItemProps = {
  item: Item,
  itemImage: ItemImage| undefined
  setSelectedImage: (image: string) => void
};

function ProtocolSectionItem({ item, itemImage, setSelectedImage }: ProtocolSectionItemProps) {

  function checkType() {
    if (item.question) {
      return (
        <ProtocolSectionItemQuestion item={item}/>
      );
    } else if (item.textInput) {
      return (
        <ProtocolSectionItemTextInput item={item}/>
      );
    } else {
      return (
        <ProtocolSectionItemMultipleChoice item={item}/>
      );
    }
  }
  
  return (
    <Card
      mode='outlined'
      key={item.id} 
      style={{ borderWidth:0, marginHorizontal:5, marginVertical:10 }}>
      <Card.Content>
        <Title>{item.name}</Title>
        <Divider style={{ height: 2 }} />
        {checkType()}
      </Card.Content>
      <View style={{display: 'flex', padding: 10, justifyContent: 'space-around', flexDirection: 'row'}}>
        <View style={{flex: 2}}>  
        </View>
      </View>
    </Card>
  );
}

type ProtocolSectionItemCompProps = {
  item: Item,
};

function ProtocolSectionItemTextInput({ item }: ProtocolSectionItemCompProps) {
  return (
    <TextInput value={item.textInput?.value} 
            multiline={true}></TextInput>
  );
}

function ProtocolSectionItemQuestion({ item }: ProtocolSectionItemCompProps) {
  return (
    <View>
      <SegmentedButtons
        value={item.question?.answer == undefined ? '' : item.question?.answer.toString()}
        onValueChange={() => { return 'NOOP';}}
        buttons={[
          { value: '0', label: 'Yes' },
          { value: '1', label: 'No'},
          { value: '2', label: 'Other' },
          { value: '3', label: 'Not applicable' },
        ]}
      />
      <Paragraph>Comment</Paragraph>
      <TextInput
        multiline={true}></TextInput>
    </View>
  );
}

function ProtocolSectionItemMultipleChoice({ item }: ProtocolSectionItemCompProps) {

  return (
    <View style={{display: 'flex', flexDirection: 'row', justifyContent: 'space-evenly'}}>
      {item.multipleChoice?.options.map((item, index) => 
      <View key={index} >
        <Text>{item.name}</Text>
        <Checkbox 
          status={item.selected ? 'checked' : 'unchecked'}/>
      </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1
  },
});

const initialProtocol: Protocol = {
  id: 1,
  name: '',
  sections: [],
  isTemplate: false,
  modifiedDate: ''
};

const initialSection: Section = {
  id: 0,
  name: '',
  priority: 1,
  checklist: undefined,
  table: undefined
};