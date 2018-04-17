import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SearchComponent } from './search.component';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../material/material.module';
import { NotesModule } from '../notes/notes.module';

const declarations = [
  SearchComponent
];

const providers = [

];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,

    MaterialModule,
    NotesModule,
    SharedModule
  ],
  providers,
})
export class SearchModule { }
