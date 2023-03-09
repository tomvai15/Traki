import  React, { useEffect, useState } from 'react';
import { View } from 'react-native';
import { TextInput, Button, Text  } from 'react-native-paper';
import projectService from '../../services/project-service';
import { CreateProjectRequest } from '../../contracts/projects/CreateProjectRequest';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { EditQuestionScreenParams, TemplateStackParamList } from './TemplateStackParamList';

type Props = NativeStackScreenProps<TemplateStackParamList, 'EditQuestion'>;

export default function EditQuestionScreen({ route, navigation }: Props) {

  const initialData: EditQuestionScreenParams = route.params;

  const [name, setName] = useState('');
  const [explanation, setExplanation] = useState('');
  const [responseMessage, setResponseMessage] = useState('');

  useEffect(() => {
    setName(initialData.name);
    setExplanation(initialData.explanation);
  }, []);

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

  function canUpdate() {
    return !(initialData.explanation != explanation || initialData.name != name);
  }

  return (
    <View style={{ flex: 1}}>
      <TextInput
        label="Klausimas"
        value={name}
        onChangeText={text => setName(text)}
      />
      <TextInput
        label="Aprašymas"
        value={explanation}
        onChangeText={value => setExplanation(value)}
      />
      <Button disabled={canUpdate()} 
              style={{ width: 200, alignSelf: 'center', marginTop: 10}} 
              mode="contained" 
              onPress={() => console.log('Question updated')}>
        Atnaujinti informaciją
      </Button>
      <Text>{responseMessage}</Text>
    </View>
  );
}