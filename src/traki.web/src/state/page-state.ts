import { AlertColor } from "@mui/material";
import { DefectNotification } from "contracts/drawing/defect/DefectNotification";
import { RecoilState, atom } from "recoil";

export interface PageState {
  notFound: boolean
}

export const pageInitialState: PageState = {
  notFound: false
};

export const pageState: RecoilState<PageState> = atom({
  key: 'pageState',
  default: pageInitialState,
});