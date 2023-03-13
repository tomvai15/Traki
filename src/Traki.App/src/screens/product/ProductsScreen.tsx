import  React, { useEffect, useState } from 'react';
import { View, FlatList } from 'react-native';
import { Button, List, Searchbar } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from './ProductStackParamList';
import { Product } from '../../contracts/product/Product';
import productService from '../../services/product-service';

type Props = NativeStackScreenProps<ProductStackParamList, 'Products'>;

export default function ProductsScreen({ navigation }: Props) {

  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {

    const focusHandler = navigation.addListener('focus', () => {
      void fetchProjects();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProjects() {
    const getProductsResposne = await productService.getProducts().catch(err =>console.log(err));
    if (!getProductsResposne) {
      return;
    }
    setProducts(getProductsResposne.products);
  }

  const [searchQuery, setSearchQuery] = React.useState('');

  const onChangeSearch = (query: string) => setSearchQuery(query);

  return (
    <View style={{ flex: 1}}>
       <Searchbar
          placeholder="Search"
          onChangeText={onChangeSearch}
          value={searchQuery}
        />
      <FlatList
        data={products.filter(p => p.name.toLowerCase().includes(searchQuery.toLowerCase()))}
        renderItem={({item}) => <List.Item
          onPress={() => navigation.navigate('Product', { productId: item.id })}
          title={item.name}
          description='Item description'
          left={props => <List.Icon {...props} icon='folder' />}
        />}
        keyExtractor={item => item.id.toString()}
      />

      <Button>Product</Button>
    </View>
  );
}