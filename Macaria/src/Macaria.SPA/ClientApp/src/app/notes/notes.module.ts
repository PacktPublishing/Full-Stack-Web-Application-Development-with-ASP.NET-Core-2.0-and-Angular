import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { TagsModule } from '../tags/tags.module';
import { NotesService } from './notes.service';
import { CoreModule } from '../core/core.module';
import { NotesPageComponent } from './notes-page.component';

import { EditNotePageComponent } from './edit-note-page.component';
import { NoteStore } from './note-store';
import { TagPageComponent } from './tag-page.component';
import { SharedModule } from '../shared/shared.module';

const declarations = [
  EditNotePageComponent,
  NotesPageComponent,
  TagPageComponent
];

const providers = [
  NotesService,
  NoteStore
];

const entryComponents = [

]

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,
    
    CoreModule,
    SharedModule,
    TagsModule
  ],
  entryComponents,
  providers,
})
export class NotesModule { }
