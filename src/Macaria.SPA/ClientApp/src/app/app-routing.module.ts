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
import { TagsResolver } from './tags/tags-resolver.service';
import { NoteResolver } from './notes/note-resolver.service';
import { NotesByTagPageComponent } from './notes/notes-by-tag-page.component';

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
        resolve: {
          tags: TagsResolver,
          note: NoteResolver
        }
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
        path: 'notes/:slug',
        component: EditNotePageComponent,        
        resolve: {
          tags: TagsResolver,
          note: NoteResolver
        }
      },
      {
        path: 'settings',
        component: SettingsPageComponent
      },
      {
        path: 'tags',
        component: TagsPageComponent,
        resolve: {
          tags: TagsResolver
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
