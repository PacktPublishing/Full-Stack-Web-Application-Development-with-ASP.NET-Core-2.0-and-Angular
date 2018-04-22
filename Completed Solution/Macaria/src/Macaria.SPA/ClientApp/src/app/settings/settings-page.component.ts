import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { LocalStorageService } from "../shared/local-storage.service";
import { constants } from "../shared/constants";

@Component({
  templateUrl: "./settings-page.component.html",
  styleUrls: ["./settings-page.component.css"],
  selector: "app-settings-page"
})
export class SettingsPageComponent { 
  constructor(private _localStorageService: LocalStorageService) {
    
  }
  
  public get cultureCode() {
    return this._localStorageService.get({ name: constants.CULTURE_KEY }) || this._getDefaultCulture();
  }

  public set cultureCode(value: string) { this._localStorageService.put({ name: constants.CULTURE_KEY, value }); }

  public onDestroy: Subject<void> = new Subject<void>();

  private _getDefaultCulture() {
    if (navigator.language == 'en-CA' || navigator.language == 'fr-CA')
      return navigator.language;

    return 'en-CA';
  }

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
