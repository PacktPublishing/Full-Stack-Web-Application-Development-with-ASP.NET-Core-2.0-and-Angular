import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { accessTokenKey, baseUrl } from '../core/constants';
import { HubClient } from '../core/hub-client';
import { LocalStorageService } from '../core/local-storage.service';
import { Logger } from './logger.service';

@Injectable()
export class AuthService {
  constructor(
    @Inject(baseUrl) private _baseUrl: string,
    private _httpClient: HttpClient,
    private _hubClient: HubClient,
    private _localStorageService: LocalStorageService,
    private _loggerService: Logger
  ) {}

  public logout() {
    this._hubClient.disconnect();
    this._localStorageService.put({ name: accessTokenKey, value: null });
  }

  public tryToLogin(options: { username: string; password: string }) {
    this._loggerService.trace('AuthService', 'tryToLogin');

    return this._httpClient.post<any>(`${this._baseUrl}api/users/token`, options).pipe(
      map(response => {
        this._localStorageService.put({ name: accessTokenKey, value: response.accessToken });
        return response.accessToken;
      })
    );
  }
}
