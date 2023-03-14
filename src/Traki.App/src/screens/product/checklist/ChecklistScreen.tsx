import  React, { useEffect, useState } from 'react';
import { FlatList, ScrollView, StyleSheet, View } from 'react-native';
import { Button, Card, Paragraph, Text, Title, TextInput, Divider, Avatar, SegmentedButtons, ActivityIndicator } from 'react-native-paper';
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
  const [isLoading, setLoading] = React.useState(false);
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
    setLoading(true);
    const getChecklistResposne = await checklistService.getChecklist(productId, checklistId).catch(err =>console.log(err));
    const getChecklistQuestionsResposne = await checklistQuestionService.getChecklistQuestions(checklistId).catch(err =>console.log(err));
    if (!getChecklistResposne || !getChecklistQuestionsResposne) {
      return;
    }
    setLoading(false);
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
    <View  style={[styles.container,{flexDirection: 'column'}]}>
      <View>
        <Button style={{margin: 5, padding: 0 ,width: 120, alignSelf: 'flex-end'}} 
          disabled={areEquivalent()} 
          onPress={() => submitChanges()} 
          mode='contained'>
            Atnaujinti
        </Button>
      </View>
      { isLoading ?
      <View style={{flex: 1, justifyContent: 'center'}}>
        <ActivityIndicator animating={isLoading}/>
      </View> :
      <View style={{flex: 1}}>
        <FlatList data={checklistQuestions} 
        keyExtractor={item => item.id.toString()}
        renderItem={ ({item}) => 
        <Card
            mode='outlined'
            key={item.id} 
            style={{ borderWidth:0, marginHorizontal:5, marginVertical:10 }}>
            <Card.Content>
              <Title>{item.title}</Title>
              <Paragraph>{item.description}</Paragraph>
              <Divider style={{ height: 2 }} />
              <Paragraph style={{fontSize: 15}}>Reikalavimai išpildyti?</Paragraph>
              <SegmentedButtons
                value={item.evaluation.toString()}
                onValueChange={(value) => 
                  setChecklistQuestions(checklistQuestions.map(x=> x.id == item.id ? Object.assign(x, {evaluation: Number(value)}) : x ))}
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
              <TextInput value={item.comment} 
                onChangeText={(value)=> 
                  setChecklistQuestions(checklistQuestions.map(x=> x.id == item.id ? Object.assign(x, {comment: value}) : x ))}
                multiline={true}></TextInput>
          </Card.Content>
        </Card>
        
        }>
        </FlatList>
      </View>}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1
  },
});