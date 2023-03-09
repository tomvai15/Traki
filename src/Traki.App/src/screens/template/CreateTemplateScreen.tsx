import  React, { useEffect, useState } from 'react';
import { View } from 'react-native';
import { TextInput, Button, Text  } from 'react-native-paper';
import projectService from '../../services/project-service';
import { CreateProjectRequest } from '../../contracts/projects/CreateProjectRequest';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { CreateQuestionScreenParams, TemplateStackParamList } from './TemplateStackParamList';

type Props = NativeStackScreenProps<TemplateStackParamList, 'CreateTemplate'>;

export default function CreateTemplateScreen({ navigation }: Props) {

  const [name, setName] = useState('');
  const [explanation, setExplanation] = useState('');
  const [responseMessage, setResponseMessage] = useState('');

  async function createProject() {
    const createProjectRequest: CreateProjectRequest = {
      project:{
        name: name
      }
    };
    const wasCreated = await projectService.createProject(createProjectRequest);

    if (wasCreated) {
      setResponseMessage('Projektas sukurtas');
    } else {
      setResponseMessage('Projekto nepavyko sukurti');
    }
  }

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