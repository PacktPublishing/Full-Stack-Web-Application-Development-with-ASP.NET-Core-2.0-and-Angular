import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Note } from "./note.model";
import { Observable } from "rxjs";
import { baseUrl } from "../core/constants";

@Injectable()
export class NotesService {
  constructor(
    private _httpClient: HttpClient,
    @Inject(baseUrl) private _baseUrl: string) {

  }

  public getBySlug(options: { slug: string }): Observable<{ note: Note }> {
    return this._httpClient
      .get<{ note: Note }>(`${this._baseUrl}api/notes/slug/${options.slug}`);
  }

  public save(options: { note: Note }) {
    return this._httpClient
      .post(`${this._baseUrl}api/notes`, options)
  }

  public addTag(options: { noteId: number, tagId: number }) {    
    return this._httpClient
      .post(`${this._baseUrl}api/notes/${options.noteId}/tag/${options.tagId}`, options);
  }

  public removeTag(options: { noteId: number, tagId: number }) {
    return this._httpClient
      .post(`${this._baseUrl}api/notes/${options.noteId}/removeTag`, options);
  }

  public get(): Observable<{ notes: Array<Note> }> {
    return this._httpClient
      .get<{ notes: Array<Note> }>(`${this._baseUrl}api/notes`)
  }

  public getByTitleAndCurrentUser(options: { title: string }): Observable<{ note: Note }> {    
    return this._httpClient
      .get<{ note: Note }>(`${this._baseUrl}api/notes/getByTitleAndCurrentUser?title=${options.title}`);
  }

  public getByCurrentUser(): Observable<{ notes: Array<Note> }> {
    return this._httpClient
      .get<{ notes: Array<Note> }>(`${this._baseUrl}api/notes/currentuser`);
  }

  public getById(options: { id: number }): Observable<{ note: Note }> {    
    return this._httpClient
      .get<{ note: Note }>(`${this._baseUrl}api/notes/getById?id=${options.id}`);
  }

  public remove(options: { note: Note }) {    
    return this._httpClient
      .delete(`${this._baseUrl}api/notes/${options.note.noteId}`);
  }
}
