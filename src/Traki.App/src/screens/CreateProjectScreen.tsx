import { NativeStackScreenProps } from '@react-navigation/native-stack';
import  React, { useEffect, useState } from 'react';
import { View } from 'react-native';
import { List, TextInput, Button, Text  } from 'react-native-paper';
import { RootStackParamList } from './RootStackPatamList';
import projectService from '../services/project-service';
import { CreateProjectRequest } from '../contracts/projects/CreateProjectRequest';

type Props = NativeStackScreenProps<RootStackParamList, 'CreateProject'>;

export default function CreateProjectScreen({navigation}: Props) {
  const [name, setName] = React.useState('');
  const [responseMessage, setResponseMessage] = React.useState('');

  useEffect(() => {
  }, []);

  async function createProject() {
    const createProjectRequest: CreateProjectRequest = {
      project:{
        name: name
      }
    }
    const wasCreated = await projectService.createProject(createProjectRequest);

    if (wasCreated) {
      setResponseMessage('Projektas sukurtas')
    } else {
      setResponseMessage('Projekto nepavyko sukurti')
    }
  }

  return (
    <View style={{ flex: 1}}>
     <TextInput
        label="Name"
        value={name}
        onChangeText={text => setName(text)}
      />
      <Button mode="contained" disabled={name==''} onPress={() => createProject()}>
        Sukurti
      </Button>
      <Text>{responseMessage}</Text>
    </View>
  );
}