import  React, { useEffect, useState } from 'react';
import { FlatList, ScrollView, StyleSheet, View, Image, TouchableHighlight} from 'react-native';
import { Button, Card, Paragraph, Text, Title, TextInput, Divider, Avatar, SegmentedButtons, ActivityIndicator, Checkbox, List, IconButton } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from '../ProductStackParamList';
import { ChecklistQuestion } from '../../../contracts/checklistQuestion/ChecklistQuestion';
import checklistQuestionService from '../../../services/checklistQuestion-service';
import { UpdateChecklistQuestionsRequest } from '../../../contracts/checklistQuestion/UpdateChecklistQuestionsRequest';
import protocolService from '../../../services/protocol-service';
import { Protocol } from '../../../contracts/protocol/Protocol';
import { Section } from '../../../contracts/protocol/Section';
import sectionService from '../../../services/section-service';
import { Item } from '../../../contracts/protocol/items/Item';
import { TextInput as ItemTextInput } from '../../../contracts/protocol/items/TextInput';
import { Checklist } from '../../../contracts/protocol/Checklist';
import { Question } from '../../../contracts/protocol/items/Question';
import { AnswerType } from '../../../contracts/protocol/items/AnswerType';
import { UpdateSectionAnswersRequest } from '../../../contracts/protocol/section/UpdateSectionAnswersRequest';
import * as ImagePicker from 'expo-image-picker';
import pictureService from '../../../services/picture-service';
import ImageView from "react-native-image-viewing";

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
    const response = await protocolService.getProtocol(protocolId);
    setProtocol(response.protocol);
    const getSectionsResponse = await sectionService.getSections(protocolId);
    setSections(getSectionsResponse.sections);
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
    <View  style={[styles.container,{flexDirection: 'column'}]}>
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
      <View style={{flex: 1, justifyContent: 'center'}}>
        <ActivityIndicator animating={isLoading}/>
      </View> :
      <View style={{flex: 1}}>
        <FlatList data={sections} 
        keyExtractor={item => item.id.toString()}
        renderItem={ ({item}) =>   
          <ProtocolSection setSelectedImage={openSelectedImgae} protocolId={protocolId} sectionId={item.id}></ProtocolSection>  
        }>
        </FlatList>
      </View>}
    </View>
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

    if (copiedSection.checklist?.items){
      fetchItemImages(copiedSection.checklist?.items);
    }
  }

  async function fetchItemImages(items: Item[]) {

    console.log('start');
    const fetchedItemImagesPromises: Promise<ItemImage>[] = items.map( async (item) => {
      if (item.itemImage == null) {
        return {
          id: item.id,
          isLocal: false,
          localImageUri: '',
          imageName: '',
          imageBase64: ''
        } as ItemImage
      }
      const imageBase64 = await pictureService.getPicture('item', item.itemImage);
      return {
        id: item.id,
        isLocal: false,
        localImageUri: '',
        imageName: item.itemImage,
        imageBase64: imageBase64
      } as ItemImage;
    })

    const fetchedItemImages = await Promise.all(fetchedItemImagesPromises);

    setInitialItemImagesJson(JSON.stringify(fetchedItemImages));
    setItemImages(fetchedItemImages);
  }

  function updateItem(updatedItem: Item) {
    if (!section.checklist) {
      return;
    }
    const updatedItems = [...section.checklist.items];

    updatedItems.forEach((item, index) => {
      updatedItems[index] = item.id == updatedItem.id ? updatedItem : item;
    });

    const updatedChecklist: Checklist = {...section.checklist, items: updatedItems};
    setSection({...section, checklist: updatedChecklist});
  }

  async function updateSection() {
    const request: UpdateSectionAnswersRequest = {
      section: section
    };
    await sectionService.updateSectionAnswers(protocolId, sectionId, request);
    setInitialSectionJson(JSON.stringify(section));

    await updateItemImages()
  }

  async function updateItemImages() {
    let formData = new FormData();

    console.log(itemImages);
    itemImages.forEach((item) => {
      if (item.isLocal) {
        console.log('??');
        let filename = item.localImageUri.split('/').pop() ?? '';
        let match = /\.(\w+)$/.exec(filename);
        let type = match ? `image/${match[1]}` : `image`;
        formData.append('photo', JSON.parse(JSON.stringify({ uri: item.localImageUri, name: item.imageName, type })));
      }
    });

    await pictureService.uploadPicturesFormData('item', formData);

    if (section.checklist?.items) {
      fetchItemImages(section.checklist?.items);
    }
  }

  function updateItemImage(itemImage: ItemImage) {
    const copiedImages: ItemImage[] = [...itemImages.filter(x=> x.id!=itemImage.id)];
    setItemImages([...copiedImages, itemImage]);
  }

  function canUpdate(): boolean {
    return initialSectionJson != JSON.stringify(section) || initialItemImagesJson != JSON.stringify(itemImages) ;
  }

  return (
    <View>
      <Card mode='contained' style={{borderWidth: 1}}>
        <Card.Title  title={section.name} right={() => 
          <Button disabled={!canUpdate()} onPress={updateSection} style={{margin: 5, padding: 0 ,width: 120, alignSelf: 'flex-end'}} mode='contained'>
            Atnaujinti
          </Button>}>  
        </Card.Title>
      </Card>
      <FlatList data={section.checklist?.items} 
        keyExtractor={item => item.id.toString()}
        renderItem={ ({item}) =>   
          <ProtocolSectionItem setSelectedImage={setSelectedImage} item={item} updateItemImage={updateItemImage} updateItem={updateItem} itemImage={itemImages.find(x=> x.id==item.id)}></ProtocolSectionItem>  
        }>
        </FlatList>
    </View>
  );
}

type ProtocolSectionItemProps = {
  item: Item,
  itemImage: ItemImage| undefined
  updateItem: (item: Item) => void
  updateItemImage: (itemImage: ItemImage) => void,
  setSelectedImage: (image: string) => void
};

function ProtocolSectionItem({ item, itemImage, updateItem, updateItemImage, setSelectedImage }: ProtocolSectionItemProps) {

  const [image, setImage] = useState<string>('');

  const pickImage = async () => {
    // No permissions request is necessary for launching the image library
    /*
    let result = await ImagePicker.launchImageLibraryAsync({
      mediaTypes: ImagePicker.MediaTypeOptions.All,
      allowsEditing: true,
      aspect: [4, 3],
      quality: 1,
    });*/

    const result = await ImagePicker.launchCameraAsync({
      mediaTypes: ImagePicker.MediaTypeOptions.All,
      allowsEditing: true,
      aspect: [4, 3],
      quality: 1,
    });

    console.log(result);

    if (result.canceled) {
      return;
    }

    const localUri = result.assets[0].uri;

    const itemImageName = itemImage && itemImage.imageName != '' ? itemImage.imageName : `${item.id}.jpeg`;

    const updatedItemImage: ItemImage = {
      id: item.id,
      isLocal: true,
      localImageUri: localUri,
      imageName: itemImageName,
      imageBase64: itemImage ? itemImage.imageBase64 : ''
    }
    updateItemImage(updatedItemImage);

    updateItem({...item, itemImage: itemImageName});
  };

  function checkType() {
    if (item.question) {
      return (
        <ProtocolSectionItemQuestion item={item} updateItem={updateItem}/>
      );
    } else if (item.textInput) {
      return (
        <ProtocolSectionItemTextInput item={item} updateItem={updateItem} />
      );
    } else {
      return (
        <ProtocolSectionItemMultipleChoice item={item} updateItem={updateItem}/>
      );
    }
  }
  
  function displayPhoto() {
    if (itemImage !== undefined) {
      if (itemImage.isLocal) {
        return (
          <TouchableHighlight onPress={() => setSelectedImage(itemImage.localImageUri)}>
            <Image source={{ uri: itemImage.localImageUri }} style={{ width: 100, height: 100 }} />
          </TouchableHighlight>
        );
      } else if (itemImage.imageBase64 != '') {
        return (
          <TouchableHighlight onPress={() => setSelectedImage(itemImage.imageBase64)}>
            <Image source={{ uri: itemImage.imageBase64 }} style={{ width: 100, height: 100 }} />
          </TouchableHighlight>
        );
      }
      return <Text>No image</Text>
    }
    else {
      return <Text>No image</Text>
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
        <View>      
          {displayPhoto()}
        </View>
        <View style={{flex: 2}}>  
        </View>
        <View>  
          <IconButton onPress={() => pickImage()} size={30} icon='camera' />
        </View>
      </View>
    </Card>
  );
}

type ProtocolSectionItemCompProps = {
  item: Item,
  updateItem: (item: Item) => void
};

function ProtocolSectionItemTextInput({ item, updateItem }: ProtocolSectionItemCompProps) {
  function updateTextInput(newValue: string) {
    if (!item.textInput) {
      return;
    }
    const updatedTextInput: ItemTextInput = {...item.textInput, value: newValue};
    const updatedItem: Item = {...item, textInput: updatedTextInput};
    updateItem(updatedItem);
  } 

  return (
    <TextInput value={item.textInput?.value} 
            onChangeText={(value)=>  updateTextInput(value)}
            multiline={true}></TextInput>
  );
}

function ProtocolSectionItemQuestion({ item, updateItem }: ProtocolSectionItemCompProps) {

  function updateQuestionComment(newValue: string) {
    if (!item.question) {
      return;
    }
    const updatedQuestion: Question = {...item.question, comment: newValue};
    const updatedItem: Item = {...item, question: updatedQuestion};
    updateItem(updatedItem);
  }

  function updateQuestionAnswer(newAnswer: AnswerType) {
    if (!item.question) {
      return;
    }
    const updatedQuestion: Question = {...item.question, answer: item.question.answer == newAnswer ? undefined : newAnswer};
    const updatedItem: Item = {...item, question: updatedQuestion};
    updateItem(updatedItem);
  }

  return (
    <View>
      <SegmentedButtons
        value={item.question?.answer == undefined ? '' : item.question?.answer.toString()}
        onValueChange={(value: string) => updateQuestionAnswer(Number(value) as AnswerType)}
        buttons={[
          { value: '0', label: 'Yes' },
          { value: '1', label: 'No'},
          { value: '2', label: 'Other' },
          { value: '3', label: 'Not applicable' },
        ]}
      />
      <Paragraph>Comment</Paragraph>
      <TextInput value={item.question?.comment} 
        onChangeText={(value)=>  updateQuestionComment(value)}
        multiline={true}></TextInput>
    </View>
  );
}

function ProtocolSectionItemMultipleChoice({ item, updateItem }: ProtocolSectionItemCompProps) {

  function updateMultipleChoice(option: string) {
    if (!item.multipleChoice) {
      return;
    }
    const updatedOptions = [...item.multipleChoice.options];
    updatedOptions.forEach((item, index) => {
      updatedOptions[index] = item.name == option ? {...item, selected: !item.selected} : {...item, selected: false};
    });

    const multipleChoice = {...item.multipleChoice, options: updatedOptions};
    const updatedItem: Item = {...item, multipleChoice: multipleChoice};
    updateItem(updatedItem);
  }

  return (
    <View style={{display: 'flex', flexDirection: 'row', justifyContent: 'space-evenly'}}>
      {item.multipleChoice?.options.map((item, index) => 
      <View key={index} >
        <Text>{item.name}</Text>
        <Checkbox 
          status={item.selected ? 'checked' : 'unchecked'} 
          onPress={() => updateMultipleChoice(item.name)}/>
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