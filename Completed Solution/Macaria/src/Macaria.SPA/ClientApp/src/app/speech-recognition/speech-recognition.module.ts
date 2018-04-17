import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { SpeechRecognitionService } from './speech-recognition.service';

const providers = [
  SpeechRecognitionService
];

@NgModule({
  imports: [
    CommonModule
  ],
  providers,
})
export class SpeechRecognitionModule { }
