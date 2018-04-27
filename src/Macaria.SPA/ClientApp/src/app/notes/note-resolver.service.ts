import { Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { RouterStateSnapshot } from '@angular/router';
import { ActivatedRouteSnapshot } from '@angular/router';
import { map, catchError, tap } from 'rxjs/operators';
import { Store } from '../core/store';
import { Injectable } from '@angular/core';
import { Note } from './note.model';
import { NotesService } from './notes.service';

@Injectable()
export class NoteResolver implements Resolve<Note> {
  constructor(private _notesService: NotesService, private _store: Store) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Note> {
    const slug = route.params['slug'];

    if (!slug) {
      const note = new Note();
      this._store.note$.next(note);
      return of(note);
    }

    return this._notesService
      .getBySlug({ slug })
      .pipe(tap(x => this._store.note$.next(x.note)), map(x => x.note));
  }
}
