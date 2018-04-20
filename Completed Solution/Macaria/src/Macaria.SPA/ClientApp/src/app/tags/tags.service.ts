import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { constants } from "../shared/constants";
import { Tag } from "./tag.model";
import { Observable } from "rxjs/Observable";

@Injectable()
export class TagsService {
  constructor(
    private _httpClient: HttpClient,
    @Inject(constants.BASE_URL) private _baseUrl: string) {

  }

  public save(options) {
    return this._httpClient
      .post<{ tag: Tag }>(`${this._baseUrl}api/tags`, options)
  }

  public get(): Observable<{ tags: Array<Tag> }> {
    return this._httpClient
      .get<{ tags: Array<Tag> }>(`${this._baseUrl}api/tags`)
  }

  public getById(options) {

  }

  public remove(options) {
    return this._httpClient
      .delete<{ tags: Array<Tag> }>(`${this._baseUrl}api/tags/${options.tagId}`)
  }
}
