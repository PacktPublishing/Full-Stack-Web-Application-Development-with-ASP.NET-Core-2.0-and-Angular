import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { constants } from "../shared/constants";
import { Tag } from "./tag.model";
import { Observable } from "rxjs/Observable";
import { shareReplay, tap } from "rxjs/operators";
import { HubClient } from "../shared/hub-client";

@Injectable()
export class TagsService {
  constructor(
    private _httpClient: HttpClient,
    private _hubClient: HubClient
    @Inject(constants.BASE_URL) private _baseUrl: string) {

    this._hubClient.messages$
      .pipe(
        tap(() => this._cache$ = null)
      ).subscribe();
  }

  public save(options) {
    return this._httpClient
      .post<{ tag: Tag }>(`${this._baseUrl}api/tags`, options)
  }

  public get(): Observable<{ tags: Array<Tag> }> {
  
    if (!this._cache$) {
      this._cache$ = this._get()
        .pipe(
          shareReplay(1)
        );
    }

    return this._cache$;
  }

  public getById(options) {
    
  }

  public remove(options) {
    return this._httpClient
      .delete<{ tags: Array<Tag> }>(`${this._baseUrl}api/tags/${options.tagId}`)
  }

  private _get(): Observable<{ tags: Array<Tag> }> {  
    return this._httpClient
      .get<{ tags: Array<Tag> }>(`${this._baseUrl}api/tags`)
  }

  private _cache$: Observable<{ tags: Array<Tag> }>;  
}
