import React, { useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate, Outlet } from 'react-router-dom';
import { DrawerAndHeader } from '../layout/DrawerAndHeader';
import SignIn from './Authentication/SignIn';
import Dashboard from './Dashboard';
import { useRecoilState } from 'recoil';
import { userState } from '../state/user-state';
import authService from '../services/auth-service';
import CheckOAuth from './Authentication/CheckOAuth';
import { Projects } from './projects/Projects';
import { ProtectedRoute } from '../components/ProtectedRoute';
import { CompanyPage } from './company/Company';
import { TemplatePage } from './templates/TemplatePage';
import { EditCheckpoint } from './templates/EditCheckpoint';
import { SectionPage } from './SectionPage';
import { ProtocolPage } from './protocols/ProtocolPage';
import { TemplateProtocols } from './protocols/TemplateProtocols';
import { EditSectionPage } from './protocols/sections/EditSectionPage';
import { CreateSectionPage } from './protocols/sections/CreateSectionPage';
import { ProductPage } from './projects/products/ProductPage';
import { FillProtocol } from './projects/products/FillProtocol';

export function Main() {

  const [userInfo, setUserInfo] = useRecoilState(userState);

  useEffect(() => {
    fetchUser();
  }, []);

  async function fetchUser() {
    try {
      const getUserResponse = await authService.getUserInfo();
      setUserInfo({ id: getUserResponse.user.id, loggedInDocuSign: getUserResponse.loggedInDocuSign });
      console.log('ok');
    } catch {
      setUserInfo({ id: -1, loggedInDocuSign: false });
      console.log('ne ok');
    }
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route path='' element={<ProtectedRoute><DrawerAndHeader/></ProtectedRoute>}>
          <Route index element={<Navigate to='/projects'/>}/>
          <Route path='company' element={<CompanyPage/>}/>
          <Route path='checkoauth' element={<CheckOAuth/>}/>
          <Route path='projects' element={<Outlet/>}>
            <Route index element={<Projects/>}/>
            <Route path=':projectId' element={<Outlet/>}>
              <Route path='products/:productId' element={<Outlet/>}>
                <Route index element={<ProductPage/>}/>
                <Route path='protocols/:protocolId' element={<FillProtocol/>}/>
              </Route>  
            </Route>
          </Route>
          <Route path='report' element={<SectionPage/>}/>
          <Route path='templates' element={<Outlet/>}>
            <Route path='protocols' element={<Outlet/>}>
              <Route index element={<TemplateProtocols/>}/>
              <Route path=':protocolId' element={<Outlet/>}>
                <Route index element={<ProtocolPage/>}/>
                <Route path='sections/:sectionId' element={<EditSectionPage/>}/>
                <Route path='sections/create' element={<CreateSectionPage/>}/>
              </Route>
            </Route>
            <Route path=':templateId' element={<Outlet/>}>
              <Route index element={<TemplatePage/>}/>
              <Route path='checkpoints/:checkpointId' element={<EditCheckpoint/>}/>
            </Route>
          </Route>
        </Route>
        <Route path='/login' element={<SignIn/>}/>
        <Route path='*' element={<Navigate to='/'/>} />
      </Routes>
    </BrowserRouter>
  );
}
