import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class LaunchSettings {
  constructor() {
    this.logLevel$ = new BehaviorSubject(0);
    this.supportedLanguages$ = new BehaviorSubject([]);
    this.defaultLanguage$ = new BehaviorSubject('');
  }

  public logLevel$: BehaviorSubject<number>;

  public supportedLanguages$: BehaviorSubject<string[]>;

  public defaultLanguage$: BehaviorSubject<string>;
}
