import React from 'react';
import { BrowserRouter, Routes, Route, Navigate, Outlet } from 'react-router-dom';
import { Projects } from './projects/Projects';
import { ProtectedRoute } from '../components/ProtectedRoute';
import { CompanyPage } from './company/Company';
import { ProtocolReport } from './projects/products/ProtocolReport';
import { HomePage } from './Home';
import { CreateProduct } from './projects/products/CreateProduct';
import MainLayout from 'layout/MainLayout';
import { ProductPage, DefectsPage, FillProtocolPage } from './projects/products';
import { TemplateProtocols, ProtocolPage } from './protocols';
import { EditSectionPage, CreateSectionPage } from './protocols/sections';
import { CreateProject, EditProject } from './projects';
import { UsersPage } from './admin/UsersPage';
import { UserPage } from './admin/UserPage';
import { CreateUserPage } from './admin/CreateUserPage';
import { EditProduct } from './projects/products/EditProduct';
import { MyInformation } from './MyInformation';
import { Protected } from 'components/Protected';
import { CheckOAuth, RegisterPage, SignIn, SignValidation } from './Authentication';

export function Main() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='' element={<ProtectedRoute><MainLayout/></ProtectedRoute>}>
          <Route index element={<Navigate to='/login'/>}/>
          <Route path='admin/users' element={<Outlet/>}>
            <Route index element={<Protected roles={['Administrator']}><UsersPage/></Protected>}/>
            <Route path=':userId' element={<Protected roles={['Administrator']}><UserPage/></Protected>}/>
            <Route path='create' element={<Protected roles={['Administrator']}><CreateUserPage/></Protected>}/>
          </Route>
          <Route path='home' element={<Protected roles={['ProjectManager', 'ProductManager']}><HomePage/></Protected>}/>
          <Route path='my-information' element={<Protected roles={['Administrator','ProjectManager', 'ProductManager']}><MyInformation/></Protected>}/>
          <Route path='company' element={<Protected roles={['Administrator']}><CompanyPage/></Protected>}/>
          <Route path='checkoauth' element={<CheckOAuth/>}/>
          <Route path='signvalidation' element={<SignValidation/>}/>
          <Route path='projects' element={<Outlet/>}>
            <Route index element={<Protected roles={['ProjectManager', 'ProductManager']}><Projects/></Protected>}/>
            <Route path='create' element={<Protected roles={['ProjectManager']}><CreateProject/></Protected>}/>
            <Route path=':projectId' element={<Outlet/>}>
              <Route path='edit' element={<Protected roles={['ProjectManager', 'ProductManager']}><EditProject/></Protected>}/>
              <Route path='products' element={<Outlet/>}>
                <Route path='create' element={<Protected roles={['ProjectManager', 'ProductManager']}><CreateProduct/></Protected>}/>
                <Route path=':productId' element={<Outlet/>}>
                  <Route index element={<Protected roles={['ProjectManager', 'ProductManager']}><ProductPage/></Protected>}/>
                  <Route path='defects' element={<Protected roles={['ProjectManager', 'ProductManager']}><DefectsPage/></Protected>}/>
                  <Route path='edit' element={<Protected roles={['ProjectManager', 'ProductManager']}><EditProduct/></Protected>}/>
                  <Route path='protocols/:protocolId' element={<Outlet/>}>
                    <Route index element={<Protected roles={['ProjectManager', 'ProductManager']}><FillProtocolPage/></Protected>}/>
                    <Route path='report' element={<Protected roles={['ProjectManager', 'ProductManager']}><ProtocolReport/></Protected>}/>
                  </Route>  
                </Route>  
              </Route>  
            </Route>
          </Route>
          <Route path='report' element={<Protected roles={['ProjectManager', 'ProductManager']}><ProtocolReport/></Protected>}/>
          <Route path='templates' element={<Outlet/>}>
            <Route path='protocols' element={<Outlet/>}>
              <Route index element={<Protected roles={['ProjectManager', 'ProductManager']}><TemplateProtocols/></Protected>}/>
              <Route path=':protocolId' element={<Outlet/>}>
                <Route index element={<Protected roles={['ProjectManager', 'ProductManager']}><ProtocolPage/></Protected>}/>
                <Route path='sections/:sectionId' element={<Protected roles={['ProjectManager', 'ProductManager']}><EditSectionPage/></Protected>}/>
                <Route path='sections/create' element={<Protected roles={['ProjectManager', 'ProductManager']}><CreateSectionPage/></Protected>}/>
              </Route>
            </Route>
          </Route>
        </Route>
        <Route path='/login' element={<SignIn/>}/>
        <Route path='/auth/register' element={<RegisterPage/>}/>
        <Route path='*' element={<Navigate to={'/login'}/>}/>
      </Routes>
    </BrowserRouter>
  );
}
