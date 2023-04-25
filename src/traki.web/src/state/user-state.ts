import { UserInfo } from "contracts/auth/UserInfo";
import { RecoilState, atom } from "recoil";
import { recoilPersist } from "recoil-persist";


interface UserState {
	id: number,
  name?: string,
  email?: string,
  user?: UserInfo,
  loggedInDocuSign: boolean
}

const initialState: UserState = {
  id: 1,
  loggedInDocuSign: false
};

const { persistAtom } = recoilPersist();

export const userState: RecoilState<UserState> = atom({
  key: 'textState',
  default: initialState,
  effects_UNSTABLE: [persistAtom],
});