export type ProductStackParamList = {
  Products: undefined;
  Product: ProductScreenParams;
  Checklist: ChecklistScreenParams;
};

export type ProductScreenParams = {
  productId: number
}

export type ChecklistScreenParams = {
  productId: number,
  checklistId: number
}

