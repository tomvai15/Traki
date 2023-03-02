import  React, { useEffect, useState } from 'react';
import { View, FlatList } from 'react-native';
import projectService from '../../services/project-service';
import { Project } from '../../contracts/projects/Project';
import { List, Button } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from './ProductStackParamList';

type Props = NativeStackScreenProps<ProductStackParamList, 'Products'>;

export default function ProductsScreen({ navigation }: Props) {

  const [projects, setProjects] = useState<Project[]>([]);

  useEffect(() => {

    const focusHandler = navigation.addListener('focus', () => {
      void fetchProjects();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProjects() {
    const getProjectsResposne = await projectService.getProjects().catch(err =>console.log(err));
    if (!getProjectsResposne) {
      return;
    }
    setProjects(getProjectsResposne.projects);
    console.log(getProjectsResposne.projects);
  }

  return (
    <View style={{ flex: 1}}>
      <FlatList
        data={projects}
        renderItem={({item}) => <List.Item
          title={item.name}
          description='Item description'
          left={props => <List.Icon {...props} icon='folder' />}
        />}
        keyExtractor={item => item.name}
      />
    </View>
  );
}