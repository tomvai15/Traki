export type ProductStackParamList = {
  Products: undefined;
  Product: ProductScreenParams;
  Protocol: ProtocolScreenParams;
  DefectsScreen: DefectsScreenParams;
};

export type ProductScreenParams = {
  productId: number
}

export type ProtocolScreenParams = {
  productId: number,
  protocolId: number
}

export type DefectsScreenParams = {
  productId: number,
}

