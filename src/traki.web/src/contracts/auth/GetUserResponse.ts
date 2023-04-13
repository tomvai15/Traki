import { UserInfo } from "./UserInfo";

export type GetUserResponse = {
  user: UserInfo,
  loggedInDocuSign: boolean
}