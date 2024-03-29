import  React, { useEffect, useState } from 'react';
import { ScrollView, TouchableOpacity, View } from 'react-native';
import { Avatar, Button, Card, Dialog, Divider, List, Portal, Searchbar, Text, Title } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from './ProductStackParamList';
import { FlatList } from 'react-native-gesture-handler';
import { Product } from '../../contracts/product/Product';
import productService from '../../services/product-service';
import { Protocol } from '../../contracts/protocol/Protocol';
import protocolService from '../../services/protocol-service';
import { ScreenView } from '../../components/ScreenView';
import { DrawingWithImage } from '../../features/products/types/DrawingWithImage';
import { drawingService, pictureService } from '../../services';
import { Drawing } from '../../contracts/drawing/Drawing';

type Props = NativeStackScreenProps<ProductStackParamList, 'Product'>;

const LeftContent = () => <List.Icon style={{margin:10}} icon="chevron-right" />;
const Wrench = () => <Avatar.Icon size={50} style={{backgroundColor:'red'}}  icon="wrench" />;

export default function ProductScreen({route, navigation}: Props) {

  const { projectId, productId} = route.params;

  const [visible, setVisible] = React.useState(false);
  const [product, setProduct] = useState<Product>();
  const [templateToAdd, setTemplateToAdd] = useState<number>(-1);
  const [searchQuery, setSearchQuery] = React.useState('');

  const [protocols, setProtocols] = useState<Protocol[]>([]);

  const showDialog = async () => {
    setVisible(true); 
    await fetchTemplates();
  };

  const hideDialog = () => {
    setTemplateToAdd(-1);
    setVisible(false);
  };

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchProduct();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProduct() {
    const getProductResposne = await productService.getProduct(projectId, productId).catch(() => {return;});
    if (!getProductResposne) {
      return;
    }
    setProduct(getProductResposne.product);
    await fetchDrawings(productId);
    await fetchProtocols();
  }

  const [drawing, setDrawing] = useState<DrawingWithImage>();

  async function fetchDrawings(productId: number) {
    const response = await drawingService.getDrawings(productId);
    await fetchDrawingPictures(response.drawings[0]);
  }

  async function fetchDrawingPictures(drawing: Drawing) {
    if (!drawing.imageName) {
      return;
    }
    const imageBase64 = await pictureService.getPicture('company', drawing.imageName);
    const newDrawingImage: DrawingWithImage = {drawing: drawing, imageBase64: imageBase64};

    setDrawing(newDrawingImage);
  }

  async function fetchProtocols() {
    const getProductProtocolsResponse = await protocolService.getProtocols(projectId, productId);
    setProtocols(getProductProtocolsResponse.protocols);
  }
  
  const [templateProtocols, setTemplateProtocols] = useState<Protocol[]>([]);

  async function fetchTemplates() {
    const getProtocolsResponse = await protocolService.getTemplateProtocols();
    setTemplateProtocols(getProtocolsResponse.protocols);
  }

  async function addProtocol(protocolId: number) {
    await productService.addProtocol(Number(projectId), Number(productId), protocolId);
    await fetchProduct();
  }

  async function createChecklist() {
    await addProtocol(templateToAdd);
    hideDialog();
  }

  return (
    <ScrollView>
      <ScreenView>
        <Portal>
          <Dialog visible={visible} onDismiss={hideDialog}>
            <Dialog.Title>Import protocol</Dialog.Title>
            <Dialog.Content>
              <Searchbar
                placeholder="Search"
                onChangeText={setSearchQuery}
                value={searchQuery}
              />
              <Card mode='outlined' style={{ height: 100, marginTop: 10 }}>
                <FlatList data={templateProtocols.filter(x=> x.name.toLowerCase().includes(searchQuery.toLowerCase()))} 
                  keyExtractor={item => item.id.toString()}
                  renderItem={({item}) => 
                    <View style={{margin:5, backgroundColor: item.id==templateToAdd ? '#e4ae3f' : 'white' }}>
                      <Text onPress={()=> setTemplateToAdd(templateToAdd == item.id ? -1 : item.id)} variant="titleMedium">
                        {item.name}
                      </Text>
                      <Divider bold={true}></Divider>
                    </View>}></FlatList>     
              </Card>
            
            </Dialog.Content>
            <Dialog.Actions style={{justifyContent: 'space-evenly'}}>
              <Button style={{ width:80 }} mode='outlined' onPress={hideDialog}>
                Close
              </Button>
              <Button disabled={templateToAdd==-1} 
                style={{ width:80 }} 
                mode='contained' 
                onPress={createChecklist}>
                  Import
              </Button>
            </Dialog.Actions>
          </Dialog>
        </Portal>

        <Card mode='outlined'>
          <Card.Content >
            <Text variant="titleLarge">{product?.name}</Text>
          </Card.Content>
          <Card.Cover style={{height:300}} source={{ uri: drawing?.imageBase64 }} />
          <Card.Actions>
            <Button onPress={() => navigation.navigate('DefectsScreen', {productId: productId})}>View defects</Button>
          </Card.Actions>
        </Card>

        <Card mode='outlined' style={{marginTop:10}}>
          <TouchableOpacity onPress={() => navigation.navigate('AddDefectScreen', {productId: productId})}>
            <Card.Title title="Add defect" left={Wrench} />
          </TouchableOpacity >
        </Card>
        <View style={{padding:15, justifyContent: 'space-between', flexDirection: 'row'}}>
          <Title>Protocols</Title>
          <Button onPress={showDialog} icon={'plus'} mode='contained'>Add</Button>
        </View>
        <List.Section style={{marginTop: -10}}>
          {protocols.map((protocol) => 
            (<Card mode='outlined' onPress={() => navigation.navigate('Protocol', {productId: productId, protocolId: protocol.id})}
              key={protocol.id} 
              style={{marginTop:10, padding: 5}}>
              <Card.Title title={protocol.name} right={LeftContent} />
            </Card>))}
        </List.Section>
      </ScreenView>
    </ScrollView>
  );
}
