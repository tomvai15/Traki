import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import ProductsScreen from '../screens/product/ProductsScreen';
import ProductScreen from '../screens/product/ProductScreen';
import { ProductStackParamList } from '../screens/product/ProductStackParamList';
import ProtocolScreen from '../screens/product/checklist/ProtocolScreen';

const ProductStack = createNativeStackNavigator<ProductStackParamList>();

export  default function ProductTab() {
  return (
    <ProductStack.Navigator screenOptions={{ header: ()=> <></>}}>
      <ProductStack.Screen name="Products" options={{title: 'Visi produktai',}} component={ProductsScreen} />
      <ProductStack.Screen name="Product" options={{title: '',}} component={ProductScreen} />
      <ProductStack.Screen name="Protocol" options={{animation:'slide_from_right', title: ''}} component={ProtocolScreen} />
    </ProductStack.Navigator>
  );
}
