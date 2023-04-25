import { useRecoilState } from "recoil";
import { AlertState, alertInitialState, alertState } from "state/alert-state";
import { pageState } from "state/page-state";

export const useNotFoundCatcher = () => {
  const [page, setPageState] = useRecoilState(pageState);

  const catchNotFound: (func: () => Promise<void>) => Promise<void> = async (func: () => Promise<void>) => {
    try {
      await func();
    } catch (err) {
      setPageState({...page, notFound: true});
      console.log(err);
    }
  };

  return { catchNotFound, } as const;
};