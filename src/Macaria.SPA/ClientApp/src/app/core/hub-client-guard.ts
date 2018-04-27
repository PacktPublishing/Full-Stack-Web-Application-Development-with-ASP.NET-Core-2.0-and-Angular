import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { HubClient } from './hub-client';
import { Observable } from 'rxjs';

@Injectable()
export class HubClientGuard implements CanActivate {
  constructor(private _hubClient: HubClient) {}

  public canActivate(): Promise<boolean> {
    return new Promise(resolve =>
      this._hubClient.connect().then(() => {
        resolve(true);
      })
    );
  }
}
