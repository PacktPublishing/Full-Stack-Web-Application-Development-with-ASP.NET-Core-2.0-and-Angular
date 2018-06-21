import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { baseUrl } from '../core/constants';
import { Note } from './note.model';

@Injectable()
export class NotesService {
  constructor(private _httpClient: HttpClient, @Inject(baseUrl) private _baseUrl: string) {}

  public getBySlug(options: { slug: string }): Observable<{ note: Note }> {
    return this._httpClient.get<{ note: Note }>(`${this._baseUrl}api/notes/slug/${options.slug}`);
  }

  public save(options: { note: Note }) {
    return this._httpClient.post(`${this._baseUrl}api/notes`, options);
  }

  public get(): Observable<{ notes: Array<Note> }> {
    return this._httpClient.get<{ notes: Array<Note> }>(`${this._baseUrl}api/notes`);
  }

  public getDeleted(): Observable<{ notes: Array<Note> }> {
    return this._httpClient.get<{ notes: Array<Note> }>(`${this._baseUrl}api/notes/deleted`);
  }
  
  public remove(options: { note: Note }) {
    return this._httpClient.delete(`${this._baseUrl}api/notes/${options.note.noteId}`);
  }

  public undelete(options: { noteId: number }) {
    return this._httpClient.post(`${this._baseUrl}api/notes/undelete`, options);
  }
}
