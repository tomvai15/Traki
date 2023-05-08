import { Product } from '../product/Product';


export type ProductRecomendation = {
  product: Product,
  defectCount: number,
  protocolsCount: number
}