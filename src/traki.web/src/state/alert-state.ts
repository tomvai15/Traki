import { AlertColor } from "@mui/material";
import { RecoilState, atom } from "recoil";
import { recoilPersist } from "recoil-persist";

const { persistAtom } = recoilPersist();

export interface AlertState {
  open?: boolean;
  type?: AlertColor;
  message?: string;
  timeout?: number | null;
}

export const alertInitialState: AlertState = {
  open: true,
  type: "info",
  message: "",
  timeout: 5000
};

export const alertState: RecoilState<AlertState> = atom({
  key: 'alertState',
  default: alertInitialState,
  effects_UNSTABLE: [persistAtom],
});