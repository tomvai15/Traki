import { useRecoilState } from "recoil";
import authService from "services/auth-service";
import { userState } from "state/user-state";

export const useUserInformation = () => {
  const [userInfo, setUserInfo] = useRecoilState(userState);

  async function fetchUser() {
    try {
      const getUserResponse = await authService.getUserInfo();
      setUserInfo((x) => { 
        return  {...x, ...getUserResponse.user, loggedInDocuSign: getUserResponse.loggedInDocuSign };
      });
      console.log('ok');
    } catch {
      setUserInfo({ id: -1, loggedInDocuSign: false });
      console.log('ne ok');
    }
  }

  async function fetchFullUserInformation() {
    try {
      const getUserResponse = await authService.getUserFullInfo();
      setUserInfo((x) => { return  {...x, user: getUserResponse.user};});
    } catch {
      setUserInfo({ id: -1, loggedInDocuSign: false });
    }
  }

  return { fetchUser, fetchFullUserInformation } as const;
};