
import { DefectRecomendation } from './DefectRecomendation';
import { ProductRecomendation } from './ProductRecomendation';

export type Recommendation = {
  products: ProductRecomendation[],
  defects: DefectRecomendation[]
}