import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { baseUrl } from '../core/constants';
import { Tag } from './tag.model';
import { Observable } from 'rxjs';
import { shareReplay, tap } from 'rxjs/operators';
import { LoggerService } from '../core/logger.service';

@Injectable()
export class TagsService {
  constructor(
    @Inject(baseUrl) private _baseUrl: string,
    private _httpClient: HttpClient,
    private _loggerService: LoggerService    
  ) { }

  public save(options) {
    return this._httpClient.post<{ tagId: number }>(`${this._baseUrl}api/tags`, options);
  }

  public get(): Observable<{ tags: Array<Tag> }> {
    return this._httpClient.get<{ tags: Array<Tag> }>(`${this._baseUrl}api/tags`);
  }

  public getBySlug(options: { slug: string }): Observable<{ tag: Tag }> {
    return this._httpClient.get<{ tag: Tag }>(`${this._baseUrl}api/tags/slug/${options.slug}`);
  }

  public remove(options) {
    return this._httpClient.delete<{ tags: Array<Tag> }>(
      `${this._baseUrl}api/tags/${options.tagId}`
    );
  }  
}
