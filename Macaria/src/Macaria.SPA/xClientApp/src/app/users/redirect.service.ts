import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from '@angular/router';
import { RedirectService } from "../tenants/redirect.service";

@Injectable()
export class LoginRedirectService extends RedirectService {
  constructor(
    route: ActivatedRoute,
    router: Router) {
    super(route, router)
  }

  loginUrl: string = "/login";

  lastPath: string;

  defaultPath: string = "/";

  setLoginUrl(value) { this.loginUrl = value; }

  setDefaultUrl(value) { this.defaultPath = value; }

  public redirectToLogin() {
    this._router.navigate([this.loginUrl]);
  }
  
  public redirectPreLogin() {
    if (this.lastPath && this.lastPath != this.loginUrl) {
      this._router.navigate([this.lastPath]);
      this.lastPath = "";
    } else {
      this._router.navigate([this.defaultPath]);
    }
  }
}
