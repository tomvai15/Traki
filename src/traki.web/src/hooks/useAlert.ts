import { useRecoilState } from "recoil";
import { AlertState, alertInitialState, alertState } from "state/alert-state";

export const useAlert = () => {
  const [alert, setAlert] = useRecoilState(alertState);

  const displayNotification = (notification: AlertState) => {
    setAlert({...alertInitialState, ...notification, open: true});
  };

  const clearNotification = () => {
    setAlert({...alert, open: false});
  };

  const displaySuccess = (message: string) => {
    setAlert({...alertInitialState, type: 'success', message: message, open: true});
  };

  const displayError= (message: string) => {
    setAlert({...alertInitialState, type: 'error', message: message, open: true});
  };

  return { displayNotification, displaySuccess, displayError, clearNotification } as const;
};