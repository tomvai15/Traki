import  React, { useEffect, useState } from 'react';
import { View } from 'react-native';
import { TextInput, Button, Text  } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { EditQuestionScreenParams, TemplateStackParamList } from './TemplateStackParamList';
import { UpdateQuestionRequest } from '../../contracts/question/UpdateQuestionRequest';
import questionService from '../../services/question-service';

type Props = NativeStackScreenProps<TemplateStackParamList, 'EditQuestion'>;

export default function EditQuestionScreen({ route }: Props) {

  const [initialData, setInitialData] = useState<EditQuestionScreenParams>(route.params);
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [responseMessage, setResponseMessage] = useState('');

  /* eslint-disable */
  useEffect(() => {
    setTitle(initialData.title);
    setDescription(initialData.description);
  }, []);
   /* eslint-disable */

  async function updateQuestion() {
    const updateQuestionRequest: UpdateQuestionRequest = {
      title: title,
      description: description
    };
    try {
      await questionService.updateQuestion(initialData.templateId, initialData.questionId, updateQuestionRequest);
      setResponseMessage('Informacija atnaujinta');
      initialData.title = title;
      initialData.description = description;

      setInitialData({templateId: initialData.templateId, questionId: initialData.questionId, title: title, description: description});
    } catch (err) {
      setResponseMessage('Nepavyko atnaujinti');
    }
  }

  function canUpdate() {
    return !(initialData.description != description || initialData.title != title);
  }

  return (
    <View style={{ flex: 1}}>
      <TextInput
        label="Klausimas"
        value={title}
        onChangeText={text => setTitle(text)}
      />
      <TextInput
        label="Aprašymas"
        value={description}
        onChangeText={value => setDescription(value)}
      />
      <Button disabled={canUpdate()} 
        style={{ width: 200, alignSelf: 'center', marginTop: 10}} 
        mode="contained" 
        onPress={async () => await updateQuestion()}>
        Atnaujinti informaciją
      </Button>
      <Text>{responseMessage}</Text>
    </View>
  );
}