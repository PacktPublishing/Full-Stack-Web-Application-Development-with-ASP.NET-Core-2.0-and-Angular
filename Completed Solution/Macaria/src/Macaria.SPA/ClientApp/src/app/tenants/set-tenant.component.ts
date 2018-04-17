import {
  Component,
  Input,
  OnInit,
  EventEmitter,
  Output,
  AfterViewInit,
  AfterContentInit,
  Renderer,
  ElementRef,
} from "@angular/core";

import { Subject } from "rxjs/Subject";
import { RedirectService } from "./redirect.service";
import { TenantsService } from "./tenants.service";
import { takeUntil } from 'rxjs/operators';



import { FormGroup, FormControl, Validators } from "@angular/forms";
import { LocalStorageService } from "../shared/local-storage.service";
import { constants } from "../shared/constants";

@Component({
    templateUrl: "./set-tenant.component.html",
    styleUrls: ["./set-tenant.component.css"],
    selector: "app-set-tenant"
})
export class SetTenantComponent { 
  constructor(
    private _localStorageService: LocalStorageService,
    private _redirectService: RedirectService,
    private _tenantsService: TenantsService
  ) {

  }

  public form = new FormGroup({
    tenantId: new FormControl("", [Validators.required])
  });

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public tryToSubmit($event: { detail: { tenantId: string } }) {    
    this._tenantsService
      .verify({ tenantId: $event.detail.tenantId })
      .pipe(takeUntil(this.onDestroy))
      .subscribe(() => {
        this._localStorageService.put({ name: constants.TENANT_KEY, value: $event.detail.tenantId });
        this._redirectService.redirectPreTenant();
      });    
  }
}
