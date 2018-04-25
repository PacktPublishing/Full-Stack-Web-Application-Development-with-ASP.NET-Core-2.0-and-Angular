import { Routes, RouterModule, RouteReuseStrategy, DetachedRouteHandle, ActivatedRouteSnapshot } from '@angular/router';
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
import { EditNotePageComponent } from './notes/edit-note-page.component';
import { LanguageGuard } from './shared/language-guard';

export const routes: Routes = [
  {
    path: 'tenants/set',
    component: AnonymousMasterPageComponent,
    children: [
      {
        path: '',
        component: SetTenantComponent
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
        component: LoginComponent
      }
    ]
  },
  {
    path: '',
    component: MasterPageComponent,
    canActivate: [
      TenantGuard,
      AuthGuard,
      HubClientGuard      
    ],
    children: [
      {
        path: '',
        component: EditNotePageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'notes',
        component: NotesPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'notes/:slug',
        component: EditNotePageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'settings',
        component: SettingsPageComponent,
        canActivate: [LanguageGuard]
      },
      {
        path: 'tags',
        component: TagsPageComponent,
        canActivate: [LanguageGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
