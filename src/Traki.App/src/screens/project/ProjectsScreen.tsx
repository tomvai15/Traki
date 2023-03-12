import  React, { useEffect, useState } from 'react';
import { View, FlatList } from 'react-native';
import projectService from '../../services/project-service';
import { Project } from '../../contracts/projects/Project';
import { List, Button, Portal, Text, Modal } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from './RootStackPatamList';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from '../../store/store';
import { setId } from '../../store/project-slice';

type Props = NativeStackScreenProps<RootStackParamList, 'Projects'>;

export default function ProjectsScreens({ navigation }: Props) {

  const dispatch = useDispatch();
  const currentSelectedProject = useSelector((state: RootState) => state.project.id);

  const [selectedProject, setSelectedProject] = useState<number>(0);
  const [projects, setProjects] = useState<Project[]>([]);
  const [visible, setVisible] = useState(false);

  const showModal = () => setVisible(true);
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
    const getProjectsResposne = await projectService.getProjects().catch(err =>console.log(err));
    if (!getProjectsResposne) {
      return;
    }
    setProjects(getProjectsResposne.projects);
  }

  function updateSelectedProject() {
    dispatch(setId(selectedProject));
    hideModal();
  }

  function openChangeProjectModal(id: number) {
    setSelectedProject(id);
    showModal();
  }

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
      <FlatList
        data={projects}
        renderItem={({item}) => <List.Item
          onPress={() => openChangeProjectModal(item.id)}
          title={item.name}
          description='Item description'
          left={props => <List.Icon {...props} icon='folder' />}
          right={item.id == currentSelectedProject ? 
            (props => <List.Icon {...props} icon='check' />) :
            (() => <></>)}
        />}
        keyExtractor={item => item.id.toString()}
      />
      <Button style={{ width: 200, alignSelf: 'center', marginBottom: 10}} mode="contained" onPress={() => navigation.navigate('CreateProject')}>
        Naujas projektas
      </Button>
    </View>
  );
}