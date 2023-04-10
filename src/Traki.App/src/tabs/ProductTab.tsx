import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import ProductsScreen from '../screens/product/ProductsScreen';
import ProductScreen from '../screens/product/ProductScreen';
import { ProductStackParamList } from '../screens/product/ProductStackParamList';
import ProtocolScreen from '../screens/product/checklist/ProtocolScreen';
import AddDefectScreen from '../screens/product/defects/AddDefectScreen';
import DefectsScreen from '../screens/product/defects/DefectsScreen';
import DefectScreen from '../screens/product/defects/DefectScreen';

const ProductStack = createNativeStackNavigator<ProductStackParamList>();

export default function ProductTab() {
  return (
    <ProductStack.Navigator screenOptions={{ header: ()=> <></>}}>
      <ProductStack.Screen name="Products" options={{title: 'Products',}} component={ProductsScreen} />
      <ProductStack.Screen name="Product" options={{title: '',}} component={ProductScreen} />
      <ProductStack.Screen name="Protocol" options={{animation:'slide_from_right', title: ''}} component={ProtocolScreen} />
      <ProductStack.Screen name="AddDefectScreen" options={{animation:'slide_from_right', title: ''}} component={AddDefectScreen} />
      <ProductStack.Screen name="DefectsScreen" options={{animation:'slide_from_right', title: ''}} component={DefectsScreen} />
      <ProductStack.Screen name="DefectScreen" options={{animation:'slide_from_right', title: ''}} component={DefectScreen} />
    </ProductStack.Navigator>
  );
}
