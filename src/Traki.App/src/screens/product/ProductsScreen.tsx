import  React, { useEffect, useState } from 'react';
import { View, FlatList } from 'react-native';
import { List, Searchbar } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from './ProductStackParamList';
import { Product } from '../../contracts/product/Product';
import productService from '../../services/product-service';

type Props = NativeStackScreenProps<ProductStackParamList, 'Products'>;

export default function ProductsScreen({route, navigation }: Props) {

  const { projectId } = route.params;

  const [products, setProducts] = useState<Product[]>([]);
  const [searchQuery, setSearchQuery] = React.useState('');

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchProjects();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProjects() {
    const getProductsResposne = await productService.getProducts(projectId).catch(() => {return;});
    if (!getProductsResposne) {
      return;
    }
    setProducts(getProductsResposne.products);
  }

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
          onPress={() => navigation.navigate('Product', {projectId: projectId, productId: item.id })}
          title={item.name}
          left={props => <List.Icon {...props} icon='folder' />}
        />}
        keyExtractor={item => item.id.toString()}
      />
    </View>
  );
}