import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from '@angular/router';

@Injectable()
export class RedirectService {
  constructor(
    protected _route: ActivatedRoute,
    protected _router: Router) {
  }

  tenantUrl: string = "/tenants/set";

  lastPath: string;

  defaultPath: string = "/";

  setTenantUrl(value) { this.setTenantUrl = value; }

  setDefaultUrl(value) { this.defaultPath = value; }

  public redirectToSetTenant() {
    this._router.navigate([this.tenantUrl]);
  }

  public redirectPreTenant() {    
    if (this.lastPath && this.lastPath != this.tenantUrl) {
      this._router.navigate([this.lastPath]);
      this.lastPath = "";
    } else {
      this._router.navigate([this.defaultPath]);
    }
  }
}
