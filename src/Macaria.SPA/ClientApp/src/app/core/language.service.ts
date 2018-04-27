import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { cultureKey } from './constants';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class LanguageService {
  constructor(
    private _localStorageService: LocalStorageService,
    private _translateService: TranslateService
  ) {}

  public get current() {
    return (
      this._localStorageService.get({ name: cultureKey }) ||
      this._getBrowserLanguage() ||
      this.default
    );
  }

  private _getBrowserLanguage() {
    if (['en', 'fr'].indexOf(this._translateService.getBrowserLang()) > -1)
      return this._translateService.getBrowserLang();
    return null;
  }

  public get default() {
    return 'en';
  }

  public setCurrent(value: string) {
    this._localStorageService.put({ name: cultureKey, value });
    this._translateService.use(value);
  }

  public currentTranslations: any = {};
}
