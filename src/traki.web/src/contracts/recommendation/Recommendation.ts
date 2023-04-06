import { Defect } from "../drawing/defect/Defect"
import { Product } from "../product/Product"

export type Recommendation = {
  products: Product[],
  defects: Defect[]
}