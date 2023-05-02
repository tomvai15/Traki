import { Product } from "contracts/product/Product"

export type ProductRecomendation = {
  product: Product,
  defectCount: number,
  protocolsCount: number
}