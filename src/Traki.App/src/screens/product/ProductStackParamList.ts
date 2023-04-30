export type ProductStackParamList = {
  ProjectsScreen: ProjectsScreenParams;
  Products: undefined;
  Product: ProductScreenParams;
  Protocol: ProtocolScreenParams;
  AddDefectScreen: AddDefectScreenParams;
  DefectsScreen: DefectsScreenParams;
  DefectScreen: DefectScreenParams;
};

export type ProjectsScreenParams = {
}

export type ProductScreenParams = {
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
