import { Routes, RouterModule, RouteReuseStrategy, DetachedRouteHandle, ActivatedRouteSnapshot } from '@angular/router';
import { HomeComponent } from './notes/home.component';
import { TenantGuard } from './tenants/tenant.guard';
import { AuthGuard } from './users/auth.guard';
import { SetTenantComponent } from './tenants/set-tenant.component';
import { LoginComponent } from './users/login.component';

import { MasterPageComponent } from './master-page.component';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { NgModule } from '@angular/core';
import { TagsPageComponent } from './tags/tags-page.component';
import { NotesPageComponent } from './notes/notes-page.component';
import { SettingsPageComponent } from './settings/settings-page.component';
import { HubClientGuard } from './shared/hub-client-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path: 'home',
    component: MasterPageComponent,
    canActivate: [
      TenantGuard,
      AuthGuard,
      HubClientGuard
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
    path: 'notes',
    component: MasterPageComponent,
    canActivate: [
      TenantGuard,
      AuthGuard,
      HubClientGuard
    ],
    children: [
      {
        path: '',
        component: NotesPageComponent
      }
    ]
  },
  {
    path: 'settings',
    component: MasterPageComponent,
    canActivate: [
      TenantGuard,
      AuthGuard,
      HubClientGuard
    ],
    children: [
      {
        path: '',
        component: SettingsPageComponent
      }
    ]
  },
  {
    path: 'tags',
    component: MasterPageComponent,
    canActivate: [
      TenantGuard,
      AuthGuard,
      HubClientGuard
    ],
    children: [
      {
        path: '',
        component: TagsPageComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
