import { RecoilState, atom } from "recoil";
import { DefectNotification } from "../contracts/drawing/defect/DefectNotification";
import ReactNativeRecoilPersist from "react-native-recoil-persist";


export const notificationsState: RecoilState<DefectNotification[]> = atom({
  key: 'notificationState',
  default: [],
  effects_UNSTABLE: [ReactNativeRecoilPersist.persistAtom]
});