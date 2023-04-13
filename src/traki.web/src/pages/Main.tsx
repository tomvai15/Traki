import React, { useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate, Outlet } from 'react-router-dom';
import SignIn from './authentication/SignIn';
import { useRecoilState } from 'recoil';
import { userState } from '../state/user-state';
import authService from '../services/auth-service';
import CheckOAuth from './authentication/CheckOAuth';
import { Projects } from './projects/Projects';
import { ProtectedRoute } from '../components/ProtectedRoute';
import { CompanyPage } from './company/Company';
import { EditCheckpoint } from './templates/EditCheckpoint';
import SignValidation from './authentication/SignValidation';
import { ProtocolReport } from './projects/products/ProtocolReport';
import { HomePage } from './Home';
import { CreateProduct } from './projects/products/CreateProduct';
import MainLayout from 'layout/MainLayout';
import { ProductPage, DefectsPage, FillProtocolPage } from './projects/products';
import { TemplateProtocols, ProtocolPage } from './protocols';
import { EditSectionPage, CreateSectionPage } from './protocols/sections';
import { TemplatePage } from './templates/TemplatePage';
import { CreateProject, EditProject } from './projects';
import { UsersPage } from './admin/UsersPage';
import { UserPage } from './admin/UserPage';
import { CreateUserPage } from './admin/CreateUserPage';

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
        <Route path='' element={<ProtectedRoute><MainLayout/></ProtectedRoute>}>
          <Route index element={<Navigate to='/home'/>}/>
          <Route path='admin/users' element={<Outlet/>}>
            <Route index element={<UsersPage/>}/>
            <Route path=':userId' element={<UserPage/>}/>
            <Route path='create' element={<CreateUserPage/>}/>
          </Route>
          <Route path='home' element={<HomePage/>}/>
          <Route path='company' element={<CompanyPage/>}/>
          <Route path='checkoauth' element={<CheckOAuth/>}/>
          <Route path='signvalidation' element={<SignValidation/>}/>
          <Route path='projects' element={<Outlet/>}>
            <Route index element={<Projects/>}/>
            <Route path='create' element={<CreateProject/>}/>
            <Route path=':projectId' element={<Outlet/>}>
              <Route path='edit' element={<EditProject/>}/>
              <Route path='products' element={<Outlet/>}>
                <Route path='create' element={<CreateProduct/>}/>
                <Route path=':productId' element={<Outlet/>}>
                  <Route index element={<ProductPage/>}/>
                  <Route path='defects' element={<DefectsPage/>}/>
                  <Route path='protocols/:protocolId' element={<Outlet/>}>
                    <Route index element={<FillProtocolPage/>}/>
                    <Route path='report' element={<ProtocolReport/>}/>
                  </Route>  
                </Route>  
              </Route>  
            </Route>
          </Route>
          <Route path='report' element={<ProtocolReport/>}/>
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
