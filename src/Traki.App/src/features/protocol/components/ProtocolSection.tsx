import  React, { useEffect, useState } from 'react';
import { FlatList, View, Text, ActivityIndicator } from 'react-native';
import { Button, Title, Divider } from 'react-native-paper';
import { Section } from '../../../contracts/protocol/Section';
import sectionService from '../../../services/section-service';
import { Item } from '../../../contracts/protocol/items/Item';
import { Checklist } from '../../../contracts/protocol/Checklist';
import { UpdateSectionAnswersRequest } from '../../../contracts/protocol/section/UpdateSectionAnswersRequest';
import pictureService from '../../../services/picture-service';
import { ItemImage } from '../../../features/protocol/types/ItemImage';
import { initialSection } from '../data/initialSection';
import { ProtocolSectionItem } from './ProtocolSectionItem';
import { Table } from '../../../contracts/protocol/section/Table';
import { ProtocolTable } from './ProtocolTable';
import { validate, validationRules } from '../../../utils/textValidation';
import { TableRow } from '../../../contracts/protocol/section/TableRow';

type Props = {
  protocolId: number,
  sectionId: number,
  setSelectedImage: (image: string) => void
}

export function ProtocolSection({ protocolId, sectionId, setSelectedImage }: Props) {

  const [isLoading, setLoading] = React.useState(false);

  const [section, setSection] = useState<Section>(initialSection);
  const [initialSectionJson, setInitialSectionJson] = useState<string>('');
  const [itemImages, setItemImages] = useState<ItemImage[]>([]);
  const [initialItemImagesJson, setInitialItemImagesJson] = useState<string>('');

  const [table, setTable] = useState<Table>();
  const [sectionType, setSectionType] = useState<string>('checklist');

  const [initialTableJson, setInitialTableJson] = useState<string>('');

  useEffect(() => {
    setLoading(true);
    fetchSection().then(() => setLoading(false));
  }, []);

  async function fetchSection() {
    const getSectionResponse = await sectionService.getSection(Number(protocolId), Number(sectionId));
    console.log(getSectionResponse);
    orderAndSetSection(getSectionResponse.section);
    orderAndSetTable(getSectionResponse.section.table);
  }

  function orderAndSetSection(sectionToSort: Section) {
    if (!sectionToSort.checklist)
    {
      setSection(sectionToSort);
      setInitialItemImagesJson(JSON.stringify([]));
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

  function orderAndSetTable(table?: Table) {
    if (!table) {
      setInitialTableJson(JSON.stringify(undefined));
      return;
    }

    setSectionType('table');
    setTable(table);
    setInitialTableJson(JSON.stringify(table));
  }

  async function fetchItemImages(items: Item[]) {

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
      section: {...section, table: table}, 
    };
    await sectionService.updateSectionAnswers(protocolId, sectionId, request);
    setInitialSectionJson(JSON.stringify(section));
    setInitialTableJson(JSON.stringify(table));

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
    if (sectionType == 'table') {
      console.log(JSON.stringify(itemImages));
    }
    return (initialSectionJson != JSON.stringify(section) || 
           initialItemImagesJson != JSON.stringify(itemImages) ||
           initialTableJson != JSON.stringify(table)) &&
           ((section.checklist == undefined ? true : isValidChecklist()) && isValidTable());
  }

  /*
  function canUpdate(): boolean {
    return  ((section.checklist == undefined ? true : isValidChecklist()) && isValidTable()) &&  
      (initialSectionJson != JSON.stringify(section) ||
      initialTableJson != JSON.stringify(table));
  }*/


  function isValidChecklist(): boolean {
    
    const a = !section.checklist?.items.map(x => { 
      return ( 
        (x.textInput == undefined ? true : !validate(x.textInput.value, [validationRules.noSpecialSymbols]).invalid) &&
        (x.question == undefined ? true : !validate(x.question.comment, [validationRules.noSpecialSymbols]).invalid)
      );
    }).some((value) => value == false);
    console.log(a);
    return a;
  }

  function isValidTable(): boolean {
    if (!table) {
      return true;
    }
    return !table?.tableRows.map(x => { 
      console.log(isValidTableRow(x));
      return isValidTableRow(x);
    }).some((value) => value == false);
  }

  function isValidTableRow(tableRow: TableRow): boolean {
    return !tableRow.rowColumns.map(x => { 
      return (x.value == undefined ? true : !validate(x.value, [validationRules.noSpecialSymbols]).invalid);
    }).some((value) => value == false);
  }


  return (
    isLoading ? 
    <View style={{margin: 50}}>
      <ActivityIndicator animating={isLoading}/>
    </View> :
    <View style={{marginBottom: 30}}>
      <View style={{display: 'flex', flexDirection: 'row', alignItems: 'flex-start', justifyContent: 'space-between'}}>
        <Title style={{fontSize: 15, width: '70%'}}>{section.name}</Title>
        <Button disabled={!canUpdate()} onPress={updateSection} mode='contained'>
            Save
          </Button>
      </View>      
      { table == undefined ?  
        <FlatList data={section.checklist?.items} 
          keyExtractor={item => item.id.toString()}
          renderItem={ ({item}) =>   
            <ProtocolSectionItem setSelectedImage={setSelectedImage} item={item} updateItemImage={updateItemImage} updateItem={updateItem} itemImage={itemImages.find(x=> x.id==item.id)}></ProtocolSectionItem>  
          }>
        </FlatList> :
        <ProtocolTable buttonVisible={true} table={table} updateTable={setTable} />}
      <Divider bold></Divider>
    </View>
  );
}