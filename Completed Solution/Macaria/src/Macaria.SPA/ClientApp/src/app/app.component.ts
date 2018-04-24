import { Component, ViewChild } from '@angular/core';
import { TranslateService } from "@ngx-translate/core";
import { LanguageService } from './shared/language.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(
    private _languageService: LanguageService,
    private _translateService: TranslateService) {

    _translateService.setDefaultLang(_languageService.default);
    
    _translateService.use(_languageService.current);
  }
}
