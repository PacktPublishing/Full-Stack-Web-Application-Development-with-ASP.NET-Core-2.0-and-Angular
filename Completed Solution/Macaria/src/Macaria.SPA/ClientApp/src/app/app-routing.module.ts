import { Routes, RouterModule, RouteReuseStrategy, DetachedRouteHandle, ActivatedRouteSnapshot } from '@angular/router';
import { HomeComponent } from './notes/home.component';
import { TenantGuard } from './tenants/tenant.guard';
import { AuthGuard } from './users/auth.guard';
import { SetTenantComponent } from './tenants/set-tenant.component';
import { LoginComponent } from './users/login.component';
import { TagManagementComponent } from './tags';
import { MasterPageComponent } from './master-page.component';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { NgModule } from '@angular/core';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path: 'home',
    component: MasterPageComponent,
    canActivate: [
      TenantGuard,
      AuthGuard
    ],
    children: [
      {
        path: '',
        component: HomeComponent
      }
    ]
  },
  {
    path: 'tenants/set',
    component: AnonymousMasterPageComponent,
    children: [
      {
        path:'',
        component:SetTenantComponent
      }
    ]
  },
  {
    path: 'login',
    component: AnonymousMasterPageComponent,
    canActivate: [
      TenantGuard
    ],
    children: [
      {
        path: '',
        component:LoginComponent
      }
    ]
  },
  {
    path: 'tags',
    component: MasterPageComponent,
    canActivate: [
      TenantGuard,
      AuthGuard
    ],
    children: [
      {
        path: '',
        component:TagManagementComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
