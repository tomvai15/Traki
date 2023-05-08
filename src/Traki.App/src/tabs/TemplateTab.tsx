import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { TemplateStackParamList } from '../screens/template/TemplateStackParamList';
import TemplatesScreen from '../screens/template/TemplatesScreen';
import TemplateScreen from '../screens/template/TemplateScreen';
import EditQuestionScreen from '../screens/template/EditQuestionScreen';

/* eslint-disable */
const ProductStack = createNativeStackNavigator<TemplateStackParamList>();

export  default function TemplateTab() {
  return (
    <ProductStack.Navigator screenOptions={{ animation:'slide_from_right', header: ()=> <></>}}>
      <ProductStack.Screen name="Templates" options={{title: 'Šablonai',}} component={TemplatesScreen} />
      <ProductStack.Screen name="Template" options={{title: 'Šablonai',}} component={TemplateScreen} />
      <ProductStack.Screen name="EditQuestion" options={{title: 'Šablonai',}} component={EditQuestionScreen} />
    </ProductStack.Navigator>
  );
}
/* eslint-disable */
