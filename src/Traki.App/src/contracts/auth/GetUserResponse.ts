import { User } from './User';

export type GetUserResponse = {
  user: User,
  loggedInDocuSign: boolean
}