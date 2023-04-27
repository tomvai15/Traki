import React, { useEffect, useState } from "react";
import { View, Image, Text } from 'react-native';
import { DefectRecomendation } from "../../../contracts/recommendation/DefectRecomendation";
import { DefectWithImage } from "../../defect/types/DefectWithImage";
import { defectService, pictureService } from "../../../services";
import { Defect } from "../../../contracts/drawing/defect/Defect";
import { Button, Card } from "react-native-paper";
import AutoImage from "../../../components/AutoImage";


type Props = {
  defect: DefectRecomendation,
  navigation: any
}

export function DefectCard ({defect, navigation}: Props) {
  const [defectWithImage, setDefectWithImage] = useState<DefectWithImage>();

  useEffect(() => {
    fetchDefect(defect.defect);
  }, []);


  async function fetchDefect(defect: Defect) {
    const response = await defectService.getDefect(defect.drawingId, defect.id);
    let imageBase64 = '';
    if (response.defect.imageName != '') {
      imageBase64 = await pictureService.getPicture('item', response.defect.imageName);
    } 
    const defectWithImage: DefectWithImage = {
      defect: response.defect,
      imageBase64: imageBase64
    };

    setDefectWithImage(defectWithImage);
  }

  return (
    <Card elevation={5} style={{marginBottom: 10}}>
      <Card.Content>
      <View style={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between'}}>
        <View style={{display: 'flex', flexDirection: 'column', justifyContent: 'space-between'}}>
          <View>
            <Text>
              {defect.defect.title}
            </Text>
            <View>
              <Text>4 Defects</Text>
            </View>
            <View>
              <Text>3 Protocols</Text>
            </View>
          </View>
          <View>
            <Button mode='contained' onPress={() => {
              navigation.navigate('Project Products', {screen: 'Products'}); 
              navigation.navigate('Project Products', { screen: 'DefectScreen', params: { productId: defect.productId, drawingId: defect.defect.drawingId, defectId: defect.defect.id }}) 
              navigation.navigate('Product', {productId: defect.productId});
            }}>
              Details
            </Button>
          </View>
        </View>
        <View>
          {defectWithImage?.imageBase64 &&
            <Image
              style={{height: 150, width: 150}}
              source={{ uri: defectWithImage?.imageBase64}}
            />}
        </View>
      </View>
      </Card.Content>
      <Card.Content>
      </Card.Content>
    </Card>
  );
}