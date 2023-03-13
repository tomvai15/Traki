import  React, { useEffect, useState } from 'react';
import { ScrollView, View } from 'react-native';
import { Button, Card, Paragraph, Text, Title, TextInput, Divider, Avatar, SegmentedButtons } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { ProductStackParamList } from '../ProductStackParamList';
import { Checklist } from '../../../contracts/checklist/Checklist';
import checklistService from '../../../services/checklist-service';
import { ChecklistQuestion } from '../../../contracts/checklistQuestion/ChecklistQuestion';
import checklistQuestionService from '../../../services/checklistQuestion-service';
import { UpdateChecklistQuestionsRequest } from '../../../contracts/checklistQuestion/UpdateChecklistQuestionsRequest';

type Props = NativeStackScreenProps<ProductStackParamList, 'Checklist'>;

export default function ChecklistScreen({ route, navigation }: Props) {

  const [initialChecklist, setInitialChecklist] = useState<ChecklistQuestion[]>([]);
  const [value, setValue] = React.useState('');
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
    setChecklistQuestions(getChecklistQuestionsResposne.checklistQuestions);
    setInitialChecklist(getChecklistQuestionsResposne.checklistQuestions.map(x=>Object.assign({},x)));
    setChecklist(getChecklistResposne.checklist);
  }

  function submitChanges() {
    const updateRequest: UpdateChecklistQuestionsRequest ={
      checklistQuestions: checklistQuestions
    }
    try {
      checklistQuestionService.updateChecklists(productId, updateRequest);
      setInitialChecklist(checklistQuestions.map(x=>Object.assign({},x)));
    } catch (err) {
      console.log(err);
    }
  }

  function areEquivalent(): boolean {
    return JSON.stringify(initialChecklist)==JSON.stringify(checklistQuestions);
  }

  return (
    <ScrollView>
      <Button style={{margin: 5, padding: 0 ,width: 120, alignSelf: 'flex-end'}} disabled={areEquivalent()} onPress={() => submitChanges()} mode='contained'>Atnaujinti</Button>
      {checklistQuestions.map((checklistQuestion) => 
        (<Card
        mode='outlined'
          key={checklistQuestion.id} 
          style={{ borderWidth:0, marginHorizontal:5, marginVertical:10 }}>
          <Card.Content>
            <Title>{checklistQuestion.title}</Title>
            <Paragraph>{checklistQuestion.description}</Paragraph>
            <Divider style={{ height: 2 }} />
            <Paragraph style={{fontSize: 15}}>Reikalavimai išpildyti?</Paragraph>
            <SegmentedButtons
              value={checklistQuestion.evaluation.toString()}
              onValueChange={(value) => 
                setChecklistQuestions(checklistQuestions.map(x=> x.id == checklistQuestion.id ? Object.assign(x, {evaluation: Number(value)}) : x ))}
              buttons={[
                {
                  value: '0', label: 'Ne',
                },
                {
                  value: '1',label: 'Taip',
                },
                { value: '2', label: 'Neaktualu' },
              ]}
            />
            <Paragraph>Įvertinimo komentaras</Paragraph>
            <TextInput value={checklistQuestion.comment} 
              onChangeText={(value)=> 
                setChecklistQuestions(checklistQuestions.map(x=> x.id == checklistQuestion.id ? Object.assign(x, {comment: value}) : x ))}
              multiline={true}></TextInput>
          </Card.Content>
        </Card>))}
    </ScrollView>
  );
}