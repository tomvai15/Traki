import { RecoilState, atom } from "recoil";
import { recoilPersist } from "recoil-persist";


interface UserInfo {
	id: number,
  loggedInDocuSign: boolean
}

const initialState: UserInfo = {
  id: 1,
  loggedInDocuSign: false
};

const { persistAtom } = recoilPersist();

export const userState: RecoilState<UserInfo> = atom({
  key: 'textState',
  default: null,
  effects_UNSTABLE: [persistAtom],
});