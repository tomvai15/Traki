import  React, { useEffect, useState } from 'react';
import { StyleSheet, View, FlatList, Text } from 'react-native';
import { Button, List, Searchbar } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { TemplateStackParamList } from './TemplateStackParamList';
import productService from '../../services/product-service';
import { Template } from '../../contracts/template/Template';
import { GetTemplateResponse } from '../../contracts/template/GetTemplateResponse';
import templateService from '../../services/template-service';
import { Question } from '../../contracts/template/Question';
import { useTheme } from 'react-native-paper';

type Props = NativeStackScreenProps<TemplateStackParamList, 'Template'>;

const staticQuestions: Question[] = [
  {
    id: 1,
    name: "Ar siųlės šaknis pilnai privirinta?",
    explanation: "Virinimas yra procesas, kai naudojant šilumą ir slėgį sujungiama du ar daugiau metalinių dalių. Proceso metu paprastai kaitinami metalo kraštai ir jie sujungiami. Virinant būtina laikytis saugos taisyklių ir turėti reikiamą apmokymą, kad būtų išvengta sužeidimų ir pasiektas sėkmingas suvirinimas."
  },
  {
    id: 2,
    name: "Josuke",
    explanation: "5"
  },
  {
    id: 3,
    name: "Josuke2",
    explanation: "4"
  }
];

export default function TemplateScreen({ route, navigation }: Props) {

  const { id } = route.params;
  const [questions, setQuestions] = useState<Question[]>();

  const { colors } = useTheme();

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchProjects();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchProjects() {
    setQuestions(staticQuestions);
  }

  return (
    <View style={{ flex: 1}}>
      <FlatList
        data={questions}
        renderItem={({item}) => <List.Accordion
            title={item.name}
            left={props => <List.Icon {...props} icon="text" />}>
            <List.Section style={{marginLeft: 0, paddingLeft: 0 }}>
              <Text style={{ paddingLeft: 20, fontWeight: 'bold' }}>Paaiškinimas</Text>
              <Text style={{ paddingLeft: 20 }}>{item.explanation}</Text>
              <View  style={{ flexDirection: 'row', justifyContent:'space-between', paddingHorizontal:20 }} >
                <Button mode="contained" buttonColor={colors.error} style={{ width: 150, paddingRight: 0, marginRight: 0}}
                onPress={() => navigation.navigate('EditQuestion', { id: 1, name: item.name, explanation: item.explanation })}>
                  Šalinti
                </Button>
                <Button mode="contained" style={{ width: 150, paddingRight: 0, marginRight: 0}}
                  onPress={() => navigation.navigate('EditQuestion', { id: 1, name: item.name, explanation: item.explanation })}>
                  Redaguoti
                </Button>
              </View>
            </List.Section>
          </List.Accordion>}
        keyExtractor={item => item.id.toString()}
      />

      <Button mode="contained" onPress={() => navigation.navigate('CreateQuestion', { id: 1 }) }>Naujas klausimas</Button>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1
  },
});
