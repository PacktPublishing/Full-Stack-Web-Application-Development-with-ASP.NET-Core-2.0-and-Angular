import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { HttpClientModule } from '@angular/common/http';
import { TagsModule } from '../tags/tags.module';
import { NotesService } from './notes.service';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../material/material.module';
import { NotesPageComponent } from './notes-page.component';
import { AgGridComponentsModule } from '../ag-grid-components/ag-grid-components.module';
import { NoteStore } from './tag-store';

const declarations = [
  HomeComponent,
  NotesPageComponent
];

const providers = [
  NotesService,
  NoteStore
];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,

    AgGridComponentsModule,
    MaterialModule,
    SharedModule,
    TagsModule
  ],
  providers,
})
export class NotesModule { }
