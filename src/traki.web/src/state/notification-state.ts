import { DefectNotification } from "contracts/drawing/defect/DefectNotification";
import { RecoilState, atom } from "recoil";
import { recoilPersist } from "recoil-persist";

const { persistAtom } = recoilPersist();

export const notificationsState: RecoilState<DefectNotification[]> = atom({
  key: 'notificationState',
  default: [],
  effects_UNSTABLE: [persistAtom],
});