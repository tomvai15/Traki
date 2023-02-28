import  React, { useState } from 'react';
import { View } from 'react-native';
import { TextInput, Button, Text  } from 'react-native-paper';
import projectService from '../services/project-service';
import { CreateProjectRequest } from '../contracts/projects/CreateProjectRequest';

export default function CreateProjectScreen() {
  const [name, setName] = useState('');
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

  return (
    <View style={{ flex: 1}}>
      <TextInput
        label="Name"
        value={name}
        onChangeText={text => setName(text)}
      />
      <Button style={{ width: 200, alignSelf: 'center', marginTop: 10}} mode="contained" disabled={name==''} onPress={() => void createProject()}>
        Sukurti
      </Button>
      <Text>{responseMessage}</Text>
    </View>
  );
}