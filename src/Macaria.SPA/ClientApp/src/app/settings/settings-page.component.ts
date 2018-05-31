import { Component } from '@angular/core';
import { LanguageService } from '../core/language.service';

@Component({
  templateUrl: './settings-page.component.html',
  styleUrls: ['./settings-page.component.css'],
  selector: 'app-settings-page'
})
export class SettingsPageComponent {
  constructor(private _languageService: LanguageService) {}

  public get currentLanguage() {
    return this._languageService.current;
  }

  public set currentLanguage(value: string) {
    this._languageService.setCurrent(value);
  }
}
