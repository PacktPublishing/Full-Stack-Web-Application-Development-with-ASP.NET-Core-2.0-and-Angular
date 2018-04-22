import { Injectable } from "@angular/core";
import { LocalStorageService } from "./local-storage.service";

@Injectable()
export class LanguageService {
  constructor(private _localStorageService: LocalStorageService) {

  }

  public get current() {
    return 'en';
  }

  public get default() {
    return 'en';
  }
  
  public setCurrent(value:string) {

  }
  
}
