import { Role } from "./Roles";

export type User = {
  id: number
  name: string,
  surname: string,
  email: string,
  role: Role,
  status: string
}