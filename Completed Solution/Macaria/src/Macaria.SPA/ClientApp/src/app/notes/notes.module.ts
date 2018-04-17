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
import { SpeechRecognitionModule } from '../speech-recognition/speech-recognition.module';

const declarations = [
  HomeComponent
];

const providers = [
  NotesService
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
    SharedModule,
    SpeechRecognitionModule,
    TagsModule
  ],
  providers,
})
export class NotesModule { }
