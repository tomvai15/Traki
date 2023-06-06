import  React from 'react';
import { View, Image, TouchableHighlight} from 'react-native';
import { Card, Text, Title, Divider, IconButton } from 'react-native-paper';
import { Item } from '../../../contracts/protocol/items/Item';
import * as ImagePicker from 'expo-image-picker';
import { ItemImage } from '../types/ItemImage';
import { ProtocolSectionItemQuestion } from './ProtocolSectionItemQuestion';
import { ProtocolSectionItemTextInput } from './ProtocolSectionItemTextInput';
import { ProtocolSectionItemMultipleChoice } from './ProtocolSectionItemMultipleChoice';

type ProtocolSectionItemProps = {
  item: Item,
  itemImage: ItemImage| undefined
  updateItem: (item: Item) => void
  updateItemImage: (itemImage: ItemImage) => void,
  setSelectedImage: (image: string) => void
};

export function ProtocolSectionItem({ item, itemImage, updateItem, updateItemImage, setSelectedImage }: ProtocolSectionItemProps) {

  const pickImage = async () => {
    const result = await ImagePicker.launchCameraAsync({
      mediaTypes: ImagePicker.MediaTypeOptions.All,
      allowsEditing: true,
      aspect: [4, 3],
      quality: 0.2,
    });

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
    };
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
      return <Text>No image</Text>;
    }
    else {
      return <Text>No image</Text>;
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