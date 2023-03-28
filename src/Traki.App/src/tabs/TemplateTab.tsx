import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { TemplateStackParamList } from '../screens/template/TemplateStackParamList';
import TemplatesScreen from '../screens/template/TemplatesScreen';
import TemplateScreen from '../screens/template/TemplateScreen';
import EditQuestionScreen from '../screens/template/EditQuestionScreen';
import CreateQuestionScreen from '../screens/template/CreateQuestionScreen';
import CreateTemplateScreen from '../screens/template/CreateTemplateScreen';

const ProductStack = createNativeStackNavigator<TemplateStackParamList>();

export  default function TemplateTab() {
  return (
    <ProductStack.Navigator screenOptions={{ animation:'slide_from_right', header: ()=> <></>}}>
      <ProductStack.Screen name="Templates" options={{title: 'Šablonai',}} component={TemplatesScreen} />
      <ProductStack.Screen name="Template" options={{title: 'Šablonai',}} component={TemplateScreen} />
      <ProductStack.Screen name="EditQuestion" options={{title: 'Šablonai',}} component={EditQuestionScreen} />
      <ProductStack.Screen name="CreateQuestion" options={{title: 'Šablonai',}} component={CreateQuestionScreen} />
      <ProductStack.Screen name="CreateTemplate" options={{title: 'Šablonai',}} component={CreateTemplateScreen} />
    </ProductStack.Navigator>
  );
}
