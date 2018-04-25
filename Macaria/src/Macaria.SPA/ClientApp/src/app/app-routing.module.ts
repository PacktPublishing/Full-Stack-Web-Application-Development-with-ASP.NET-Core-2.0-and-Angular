import { Routes, RouterModule, RouteReuseStrategy, DetachedRouteHandle, ActivatedRouteSnapshot } from '@angular/router';
import { AuthGuard } from './users/auth.guard';
import { LoginComponent } from './users/login.component';
import { MasterPageComponent } from './master-page.component';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { NgModule } from '@angular/core';
import { TagsPageComponent } from './tags/tags-page.component';
import { NotesPageComponent } from './notes/notes-page.component';
import { SettingsPageComponent } from './settings/settings-page.component';
import { HubClientGuard } from './core/hub-client-guard';
import { EditNotePageComponent } from './notes/edit-note-page.component';
import { LanguageGuard } from './core/language-guard';
import { TagPageComponent } from './notes/tag-page.component';

export const routes: Routes = [
  {
    path: 'login',
    component: AnonymousMasterPageComponent,
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
        path: 'notes/tag/:tagId',
        component: TagPageComponent,
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
