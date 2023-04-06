import { Product } from "../product/Product"
import { DefectRecomendation } from "./DefectRecomendation"

export type Recommendation = {
  products: Product[],
  defects: DefectRecomendation[]
}