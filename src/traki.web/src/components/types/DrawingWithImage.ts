import { Drawing } from "../../contracts/drawing/Drawing";

export type DrawingWithImage = {
  drawing: Drawing,
  imageBase64: string
}