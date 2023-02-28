import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { RootStackParamList } from '../screens/project/RootStackPatamList';
import CreateProjectScreen from '../screens/project/CreateProjectScreen';
import DetailsScreen from '../screens/project/DetailsScreen';
import HomeScreen from '../screens/project/HomeScreen';
import ProjectsScreens from '../screens/project/ProjectsScreen';

const ProjectStack = createNativeStackNavigator<RootStackParamList>();

export  default function ProjectTab() {
  return (
  <ProjectStack.Navigator>
    <ProjectStack.Screen name="Home" component={HomeScreen} />
    <ProjectStack.Screen name="Details" component={DetailsScreen} />
    <ProjectStack.Screen name="Projects" component={ProjectsScreens} />
    <ProjectStack.Screen name="CreateProject" component={CreateProjectScreen} />
  </ProjectStack.Navigator>
  );
}
