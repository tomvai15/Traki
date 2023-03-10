export type TemplateStackParamList = {
  Templates: undefined;
  Template: TemplateScreenParams;
  EditQuestion: EditQuestionScreenParams;
  CreateQuestion: CreateQuestionScreenParams;
  CreateTemplate: undefined;
};

export type TemplateScreenParams = {
  id: number
}

export type EditQuestionScreenParams = {
  templateId: number,
  questionId: number,
  title: string,
  description: string
}

export type CreateQuestionScreenParams = {
  id: number
}