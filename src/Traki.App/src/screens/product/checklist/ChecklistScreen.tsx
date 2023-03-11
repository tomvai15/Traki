import  React, { useEffect, useState } from 'react';
import { ScrollView, View } from 'react-native';
import { Button, Card, Paragraph, Text, Title, TextInput, Divider, Avatar } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from '../ProductStackParamList';
import { Checklist } from '../../../contracts/checklist/Checklist';
import checklistService from '../../../services/checklist-service';
import { ChecklistQuestion } from '../../../contracts/checklistQuestion/ChecklistQuestion';
import checklistQuestionService from '../../../services/checklistQuestion-service';

type Props = NativeStackScreenProps<ProductStackParamList, 'Checklist'>;

export default function ChecklistScreen({ route, navigation }: Props) {

  const {productId, checklistId} = route.params;

  const [checklist, setChecklist] = useState<Checklist>();
  const [checklistQuestions, setChecklistQuestions] = useState<ChecklistQuestion[]>([]);

  useEffect(() => {
    const focusHandler = navigation.addListener('focus', () => {
      void fetchData();
    });
    return focusHandler;
  }, [navigation]);

  async function fetchData() { 
    const getChecklistResposne = await checklistService.getChecklist(productId, checklistId).catch(err =>console.log(err));
    const getChecklistQuestionsResposne = await checklistQuestionService.getChecklistQuestions(checklistId).catch(err =>console.log(err));
    if (!getChecklistResposne || !getChecklistQuestionsResposne) {
      return;
    }
    setChecklistQuestions(getChecklistQuestionsResposne.checklistQuestions)
    setChecklist(getChecklistResposne.checklist);
  }

  return (
    <ScrollView>
      {checklistQuestions.map((checklistQuestion) => 
        (<Card
          key={checklistQuestion.id} 
          style={{marginTop:10, padding: 5}}>
          <Card.Content>
            <Title>{checklistQuestion.title}</Title>
            <Paragraph>{checklistQuestion.description}</Paragraph>
          </Card.Content>
        </Card>))}

        <Card
          style={{marginTop:10, padding: 5}}>
          <Card.Content>
            <Title>Ar gaminys yra geras?</Title>
            <Paragraph>Card content</Paragraph>
            <Divider style={{borderColor: 'black'}} />
            <TextInput multiline={true}></TextInput>
          </Card.Content>
        </Card>
    </ScrollView>
  );
}