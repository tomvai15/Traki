import  React, { useEffect, useState } from 'react';
import { View, Image } from 'react-native';
import { Button, Card, Text, TextInput } from 'react-native-paper';
import { Product } from '../../../contracts/product/Product';
import { DrawingWithImage } from '../../products/types/DrawingWithImage';
import drawingService from '../../../services/drawing-service';
import { Drawing } from '../../../contracts/drawing/Drawing';
import { pictureService } from '../../../services';
import AutoImage from '../../../components/AutoImage';

type Props = {
  product: Product,
  navigation: any
}

export function ProductCard ({product, navigation}: Props) {
  const [drawing, setDrawing] = useState<DrawingWithImage>();

  useEffect(() => {
    fetchDrawings();
  }, []);

  async function fetchDrawings() {
    const response = await drawingService.getDrawings(Number(product.id));
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

  return (
    <Card elevation={5} style={{ display: 'flex', flexDirection: 'column', marginBottom: 10}}>
      <Card.Content style={{ flexGrow: 1 }}>
        <View style={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}}>
          <View style={{display: 'flex', flexDirection: 'column', justifyContent: 'space-between'}}>
            <View>
              <Text style={{fontSize: 20}}>
                {product.name}
              </Text>
              <View>
                <Text style={{fontSize: 15}}>4 Defects</Text>
              </View>
              <View>
                <Text style={{fontSize: 15}}>3 Protocols</Text>
              </View>
            </View>
            <View>
              <Button style={{width: 100}} onPress={() => {
                //navigation.navigate('Project Products', {screen: 'Products'}); 
                navigation.navigate('Projects', { screen: 'Product', params: {productId: product.id}}) 
              }} mode='contained'>Details</Button>
            </View>
          </View>
          <View>
            {drawing?.imageBase64 &&
            <Image
              style={{height: 150, width: 150}}
              source={{ uri: drawing.imageBase64}}
            />}
          </View>
        </View>
      </Card.Content>
    </Card>
  );
}