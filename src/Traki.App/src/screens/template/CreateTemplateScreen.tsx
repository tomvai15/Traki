import  React, { useEffect, useState } from 'react';
import { View } from 'react-native';
import { TextInput, Button, Text  } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { TemplateStackParamList } from './TemplateStackParamList';

type Props = NativeStackScreenProps<TemplateStackParamList, 'CreateTemplate'>;

export default function CreateTemplateScreen({ navigation }: Props) {

  const [name, setName] = useState('');
  const [explanation, setExplanation] = useState('');
  const [responseMessage, setResponseMessage] = useState('');

  function notEmptyOrNull( params: string[]): boolean {
    return params.some(x=> x == null || x == '');
  }

  function canCreate() {
    return notEmptyOrNull([name, explanation]);
  }

  return (
    <View style={{ flex: 1}}>
      <TextInput
        label="Pavadinimas"
        value={name}
        onChangeText={text => setName(text)}
      />
      <TextInput
        label="Standartas"
        value={explanation}
        onChangeText={value => setExplanation(value)}
      />
      <Button disabled={canCreate()} 
              style={{ width: 200, alignSelf: 'center', marginTop: 10}} 
              mode="contained" 
              onPress={() => console.log('Template Created')}>
        Sukurti šabloną
      </Button>
      <Text>{responseMessage}</Text>
    </View>
  );
}