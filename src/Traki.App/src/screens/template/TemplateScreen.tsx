import  React, { useEffect, useState } from 'react';
import { StyleSheet, View, FlatList, Text } from 'react-native';
import { Button, List } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { TemplateStackParamList } from './TemplateStackParamList';
import templateService from '../../services/template-service';
import { useTheme } from 'react-native-paper';
import { Question } from '../../contracts/question/Question';
import questionService from '../../services/question-service';
import { Template } from '../../contracts/template/Template';

type Props = NativeStackScreenProps<TemplateStackParamList, 'Template'>;

const staticQuestions: Question[] = [
  {
    id: 1,
    title: 'Ar siųlės šaknis pilnai privirinta?',
    description: 'Virinimas yra procesas, kai naudojant šilumą ir slėgį sujungiama du ar daugiau metalinių dalių. Proceso metu paprastai kaitinami metalo kraštai ir jie sujungiami. Virinant būtina laikytis saugos taisyklių ir turėti reikiamą apmokymą, kad būtų išvengta sužeidimų ir pasiektas sėkmingas suvirinimas.'
  },
  {
    id: 2,
    title: 'Josuke',
    description: '5'
  },
  {
    id: 3,
    title: 'Josuke2',
    description: '4'
  }
];

export default function TemplateScreen({ route, navigation }: Props) {

  const { id } = route.params;
  const [questions, setQuestions] = useState<Question[]>();
  const [template, setTemplate] = useState<Template>();

  const { colors } = useTheme();

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchQuestions();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchQuestions() {
    const getQuestionsResponse = await questionService.getQuestions(id).catch(err =>console.log(err));
    const getTemplateResponse = await templateService.getTemplate(id).catch(err =>console.log(err));
    if (!getQuestionsResponse || !getTemplateResponse) {
      return;
    }
    setQuestions(getQuestionsResponse.questions);
    setTemplate(getTemplateResponse.template);
  }

  return (
    <View style={{ flex: 1}}>
      <Text>{template?.name}</Text>
      <FlatList
        data={questions}
        renderItem={({item}) => <List.Accordion
          title={item.title}
          left={props => <List.Icon {...props} icon="text" />}>
          <List.Section style={{marginLeft: 0, paddingLeft: 0 }}>
            <Text style={{ paddingLeft: 20, fontWeight: 'bold' }}>Paaiškinimas {item.id}</Text>
            <Text style={{ paddingLeft: 20 }}>{item.description}</Text>
            <View  style={{ flexDirection: 'row', justifyContent:'space-between', paddingHorizontal:20 }} >
              <Button mode="contained" buttonColor={colors.error} style={{ width: 150, paddingRight: 0, marginRight: 0}}
                onPress={() => navigation.navigate('EditQuestion', { id: 1, title: item.title, description: item.description })}>
                  Šalinti
              </Button>
              <Button mode="contained" style={{ width: 150, paddingRight: 0, marginRight: 0}}
                onPress={() => navigation.navigate('EditQuestion', { templateId: id, questionId: item.id, title: item.title, description: item.description })}>
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
