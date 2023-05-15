import  React, { useEffect, useState } from 'react';
import { View, FlatList } from 'react-native';
import projectService from '../../services/project-service';
import { Project } from '../../contracts/projects/Project';
import { List, Button, Portal, Text, Modal, Searchbar } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from './ProductStackParamList';

type Props = NativeStackScreenProps<ProductStackParamList, 'ProjectsScreen'>;

export default function ProjectsScreen({ navigation }: Props) {

  const currentSelectedProject = 1;

  const [selectedProject, setSelectedProject] = useState<number>(0);
  const [projects, setProjects] = useState<Project[]>([]);
  const [visible, setVisible] = useState(false);

  const hideModal = () => setVisible(false);
  const containerStyle = { backgroundColor: 'white', padding: 20, margin: 20};

  useEffect(() => {
    setSelectedProject(currentSelectedProject);
    const focusHandler = navigation.addListener('focus', () => {
      void fetchProjects();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProjects() {
    const getProjectsResposne = await projectService.getProjects().catch(() => {return;});
    if (!getProjectsResposne) {
      return;
    }
    setProjects(getProjectsResposne.projects);
  }

  function updateSelectedProject() {
    hideModal();
  }

  const [searchQuery, setSearchQuery] = React.useState('');
  const onChangeSearch = (query: string) => setSearchQuery(query);

  return (
    <View style={{ flex: 1}}>
      <Portal>
        <Modal visible={visible} onDismiss={hideModal} contentContainerStyle={containerStyle}>
          <Text style={{ alignSelf: 'center'}}>Pakeisti projektą į {selectedProject}?</Text>
          <Button style={{ width: 100,  alignSelf: 'center'}} mode="contained" onPress={updateSelectedProject}>
            Pakeisti
          </Button>
        </Modal>
      </Portal>
      <Searchbar
        placeholder="Search"
        onChangeText={onChangeSearch}
        value={searchQuery}
      />
      <FlatList
        data={projects.filter(p => p.name.toLowerCase().includes(searchQuery.toLowerCase()))}
        renderItem={({item}) => <List.Item
          onPress={() => navigation.navigate('Products', {projectId: item.id})}
          title={item.name}
          left={props => <List.Icon {...props} icon='folder' />}
        />}
        keyExtractor={item => item.id.toString()}
      />
    </View>
  );
}