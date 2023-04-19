import { useRecoilState } from "recoil";
import authService from "services/auth-service";
import { userState } from "state/user-state";

export const useUserInformation = () => {
  const [userInfo, setUserInfo] = useRecoilState(userState);

  async function fetchUser() {
    try {
      const getUserResponse = await authService.getUserInfo();
      setUserInfo({...getUserResponse.user, loggedInDocuSign: getUserResponse.loggedInDocuSign });
      console.log('ok');
    } catch {
      setUserInfo({ id: -1, loggedInDocuSign: false });
      console.log('ne ok');
    }
  }

  return { fetchUser } as const;
};