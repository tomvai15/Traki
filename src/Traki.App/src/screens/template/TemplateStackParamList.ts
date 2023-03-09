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
  id: number,
  name: string,
  explanation: string
}

export type CreateQuestionScreenParams = {
  id: number
}