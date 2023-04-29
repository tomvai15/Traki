import { RecoilState, atom } from "recoil";
import ReactNativeRecoilPersist from "react-native-recoil-persist";
import { UserInfo } from "../contracts/auth/UserInfo";

interface UserState {
	id: number,
  name?: string,
  email?: string,
  token?: string,
  refreshToken?: string,
  user?: UserInfo,
  loggedInDocuSign: boolean
}

const initialState: UserState = {
  id: 1,
  loggedInDocuSign: false,
  refreshToken: 'dsaasdasd'
};


export const userState: RecoilState<UserState> = atom({
  key: 'userState',
  default: initialState,
  effects_UNSTABLE: [ReactNativeRecoilPersist.persistAtom]
});