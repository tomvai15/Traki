export type ProductStackParamList = {
  ProjectsScreen: undefined;
  Products: ProductsScreenParams;
  Product: ProductScreenParams;
  Protocol: ProtocolScreenParams;
  AddDefectScreen: AddDefectScreenParams;
  DefectsScreen: DefectsScreenParams;
  DefectScreen: DefectScreenParams;
};

export type ProductsScreenParams = {
  projectId: number,
}

export type ProductScreenParams = {
  projectId: number,
  productId: number
}

export type ProtocolScreenParams = {
  productId: number,
  protocolId: number
}

export type AddDefectScreenParams = {
  productId: number,
}

export type DefectsScreenParams = {
  productId: number,
}

export type DefectScreenParams = {
  productId: number,
  drawingId: number,
  defectId: number
}
