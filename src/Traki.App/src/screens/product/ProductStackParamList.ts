export type ProductStackParamList = {
  Products: undefined;
  Product: ProductScreenParams;
  Protocol: ProtocolScreenParams;
};

export type ProductScreenParams = {
  productId: number
}

export type ProtocolScreenParams = {
  productId: number,
  protocolId: number
}

