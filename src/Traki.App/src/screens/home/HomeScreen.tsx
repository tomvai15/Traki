import  React, { useEffect, useState } from 'react';
import { FlatList, View } from 'react-native';
import { Avatar, Button, Card, Text, TextInput } from 'react-native-paper';
import { userState } from '../../state/user-state';
import { useRecoilState } from 'recoil';
import { DefectRecomendation } from '../../contracts/recommendation/DefectRecomendation';
import { Product } from '../../contracts/product/Product';
import recommendationService from '../../services/recommendation-service';
import { ScrollView } from 'react-native-gesture-handler';
import { ProductCard } from '../../features/recommendation/components/ProductCard';
import { DefectCard } from '../../features/recommendation/components/DefectCard';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { HomeStackParamList } from './HomeStackParamList';

const Wrench = () => <Avatar.Icon size={50} style={{backgroundColor:'red'}}  icon="alert" />;

type Props = NativeStackScreenProps<HomeStackParamList, 'HomeScreen'>;

export default function HomeScreen({navigation}: Props) {
  
  const [defects, setDefects] = useState<DefectRecomendation[]>([]);
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetchRecommendations();
  }, []);

  async function fetchRecommendations() {
    const response = await recommendationService.getRecommendations();
    setProducts(response.recommendation.products);
    setDefects(response.recommendation.defects);
  }

  return (
    <ScrollView>
      <Card style={{marginBottom: 10}}>
        <Card.Title  title='Products' left={Wrench}/>
      </Card>
      {products.map((item, index) => <ProductCard navigation={navigation} key={index} product={item} />)}
      <Card style={{marginBottom: 10}}>
        <Card.Title title='Defects' left={Wrench}/>
      </Card>
      {defects.map((item, index) => <DefectCard navigation={navigation} key={index} defect={item} />)}
    </ScrollView>
  );
}