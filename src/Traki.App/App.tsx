import React, { useEffect, useState } from 'react';
import { Button, StyleSheet, Text, View } from 'react-native';
import projectService from './src/services/project-service';
import { GetProjectResponse } from './src/contracts/GetProjectsReponse';


export default function App() {
  const [count, setCount] = useState<number>(0);
  const [projects, setProjects] = useState<Array<GetProjectResponse>>([]);

  useEffect(() => {
    fetchProjects();
  }, []);
  
  async function fetchProjects() {
    const projects = await projectService.get();
    setProjects(projects);
  }

  return (
    <View style={styles.container}>
      <Text>Open up App.tsx to start working on your app!</Text>
      <Text>{count}</Text>
      <Button title='e' onPress={()=> {setCount(count+1); fetchProjects();}}/>
      { projects.length > 0 ?
						projects.map((project: GetProjectResponse) => (
              <Text key={project.id}>{project.id}-{project.name}</Text>    
						))
						:
            <Text>No projects</Text>    
					}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
