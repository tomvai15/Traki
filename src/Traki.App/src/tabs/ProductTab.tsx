import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import ProductsScreen from '../screens/product/ProductsScreen';
import { ProductStackParamList } from '../screens/product/ProductStackParamList';

const ProductStack = createNativeStackNavigator<ProductStackParamList>();

export  default function ProductTab() {
  return (
  <ProductStack.Navigator>
    <ProductStack.Screen name="Products" component={ProductsScreen} />
  </ProductStack.Navigator>
  );
}
