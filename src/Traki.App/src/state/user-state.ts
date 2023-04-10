import { RecoilState, atom } from "recoil";
import ReactNativeRecoilPersist from "react-native-recoil-persist";

interface UserInfo {
	id: number,
  token?: string,
  loggedInDocuSign: boolean
}

const initialState: UserInfo = {
  id: 1,
  loggedInDocuSign: false
};

export const userState: RecoilState<UserInfo> = atom({
  key: 'textState',
  default: initialState,
  effects_UNSTABLE: [ReactNativeRecoilPersist.persistAtom],
});