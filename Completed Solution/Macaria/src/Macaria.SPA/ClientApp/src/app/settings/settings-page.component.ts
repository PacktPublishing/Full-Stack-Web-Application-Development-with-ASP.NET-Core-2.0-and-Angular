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
  constructor(private _localStorageService: LocalStorageService) { }
  
  public get cultureCode() { return this._localStorageService.get({ name: constants.CULTURE_KEY }); }

  public set cultureCode(value: string) { this._localStorageService.put({ name: constants.CULTURE_KEY, value }); }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
