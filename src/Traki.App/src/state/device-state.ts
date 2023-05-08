import { RecoilState, atom } from 'recoil';
import ReactNativeRecoilPersist from 'react-native-recoil-persist';

interface DeviceInfo {
  token: string,
}

const initialState: DeviceInfo = {
  token: ''
};
/* eslint-disable */
export const deviceState: RecoilState<DeviceInfo> = atom({
  key: 'deviceState',
  default: initialState,
  effects_UNSTABLE: [ReactNativeRecoilPersist.persistAtom],
});
 /* eslint-disable */