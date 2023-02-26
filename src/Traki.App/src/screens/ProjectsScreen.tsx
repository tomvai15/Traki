import  React, { useEffect, useState } from 'react';
import { View, FlatList } from 'react-native';
import projectService from '../services/project-service';
import { Project } from '../contracts/projects/Project';
import { List } from 'react-native-paper';

//type Props = NativeStackScreenProps<RootStackParamList, 'Projects'>;

export default function ProjectsScreens() {

  const [projects, setProjects] = useState<Project[]>([]);

  useEffect(() => {
    void fetchProjects();
  }, []);

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
          description="Item description"
          left={props => <List.Icon {...props} icon="folder" />}
        />}
        keyExtractor={item => item.name}
      />
    </View>
  );
}