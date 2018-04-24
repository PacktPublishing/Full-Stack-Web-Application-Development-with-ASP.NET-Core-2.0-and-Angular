import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from "@angular/router";
import { Observable } from "rxjs";


import { TenantsService } from "./tenants.service";
import { LocalStorageService } from "../shared/local-storage.service";
import { RedirectService } from "./redirect.service";
import { Injectable } from "@angular/core";
import { constants } from "../shared/constants";

@Injectable()
export class TenantGuard implements CanActivate {
  constructor(
    private _localStorageService: LocalStorageService,
    private _redirectService: RedirectService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const tenantId = this._localStorageService.get({ name: constants.TENANT_KEY });

    if (tenantId)
      return true;

    this._redirectService.lastPath = state.url;
    this._redirectService.redirectToSetTenant();

    return false;
  }
}
