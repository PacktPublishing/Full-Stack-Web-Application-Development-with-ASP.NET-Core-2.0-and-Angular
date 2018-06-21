import {
  Routes,
  RouterModule
} from '@angular/router';
import { LoginComponent } from './users/login.component';
import { MasterPageComponent } from './master-page.component';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { NgModule } from '@angular/core';
import { TagsPageComponent } from './tags/tags-page.component';
import { NotesPageComponent } from './notes/notes-page.component';
import { SettingsPageComponent } from './settings/settings-page.component';
import { HubClientGuard } from './core/hub-client-guard';
import { EditNotePageComponent } from './notes/edit-note-page.component';
import { AuthGuard } from './core/auth.guard';
import { NotesByTagPageComponent } from './notes/notes-by-tag-page.component';
import { CanDeactivateComponentGuard } from './core/can-deactivate-component.guard';
import { DeletedNotesPageComponent } from './notes/deleted-notes-page.component';

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
    canActivate: [AuthGuard, HubClientGuard],
    children: [
      {
        path: '',
        component: EditNotePageComponent,
        canDeactivate:[CanDeactivateComponentGuard]
      },
      {
        path: 'notes',
        component: NotesPageComponent
      },
      {
        path: 'tags/:slug',
        component: NotesByTagPageComponent
      },
      {
        path: 'notes/deleted',
        component: DeletedNotesPageComponent
      },
      {
        path: 'notes/:slug',
        component: EditNotePageComponent
      },
      {
        path: 'settings',
        component: SettingsPageComponent
      },
      {
        path: 'tags',
        component: TagsPageComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
