import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { constants } from "../shared/constants";

declare var webkitSpeechRecognition: any;

@Injectable()
export class SpeechRecognitionService {
  constructor() {
    this._onStart = this._onStart.bind(this);
    this._onResult = this._onResult.bind(this);
    this._onEnd = this._onEnd.bind(this);

    if (constants.SUPPORTS_SPEECH_RECOGNITION) {
      this._recognition = new webkitSpeechRecognition();
      this._recognition.continuous = true;
      this._recognition.interimResults = true;

      this._recognition.onstart = this._onStart;
      this._recognition.onresult = this._onResult;
      this._recognition.onend = this._onEnd;
    }
  }

  public finalTranscript$: BehaviorSubject<string> = new BehaviorSubject('');

  private _onStart() {
    this._recognizing = true;
  }

  private _onEnd() {
    this._recognizing = false;
  }

  private _onResult(event) {
    var interim_transcript = '';
    var final_transcript = '';

    for (var i = event.resultIndex; i < event.results.length; ++i) {
      if (event.results[i].isFinal) {
        final_transcript += event.results[i][0].transcript;
      } else {
        interim_transcript += event.results[i][0].transcript;
      }
    }

    this.finalTranscript$.next(final_transcript);
  }

  private _recognition: any;

  private _recognizing: boolean;

  public start() {
    if (this._recognizing) return;

    this._recognition.lang = 'English';
    this._recognition.start();
  }

  public stop() {
    if (!this._recognizing) return;

    this._recognition.stop();
  }
}
