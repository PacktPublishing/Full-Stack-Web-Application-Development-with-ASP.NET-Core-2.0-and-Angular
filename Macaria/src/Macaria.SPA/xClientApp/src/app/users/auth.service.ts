import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LocalStorageService } from "../shared/local-storage.service";
import { map } from "rxjs/operators";
import { constants } from "../shared/constants";
import { HubClient } from "../shared/hub-client";

@Injectable()
export class AuthService {
  constructor(
    @Inject(constants.BASE_URL)private _baseUrl:string,
    private _httpClient: HttpClient,
    private _hubClient: HubClient,
    private _localStorageService: LocalStorageService

  ) { }

  public logout() {
    this._hubClient.disconnect();
    this._localStorageService.put({ name: constants.ACCESS_TOKEN_KEY, value: null }); }

  public tryToLogin(options: { username: string, password: string }) {

    return this._httpClient
      .post<any>(`${this._baseUrl}api/users/token`, options)
      .pipe(map(response => {
        this._localStorageService.put({ name: constants.ACCESS_TOKEN_KEY, value: response.accessToken });
        return response.accessToken;
      }));
  }
  

}
