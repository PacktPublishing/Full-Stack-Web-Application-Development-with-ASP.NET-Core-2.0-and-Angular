import { Injectable } from "@angular/core";
import { LocalStorageService } from "./local-storage.service";
import { constants } from "./constants";
import { TranslateService } from "@ngx-translate/core";

@Injectable()
export class LanguageService {
  constructor(
    private _localStorageService: LocalStorageService
    private _translateService: TranslateService
  ) { }

  public get current() {
    return this._localStorageService.get({ name: constants.CULTURE_KEY }) || this._getBrowserLanguage() || this.default;
  }
  
  private _getBrowserLanguage() {
    if (navigator.language == 'en-CA' || navigator.language == 'fr-CA') {      
      return navigator.language.split("-")[0];
    }
    return null;
  }

  public get default() { return 'en'; }
  
  public setCurrent(value: string) {
    this._localStorageService.put({ name: constants.CULTURE_KEY, value });
    this._translateService.use(value);
  }
  
}
