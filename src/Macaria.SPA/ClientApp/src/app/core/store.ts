import { Injectable } from '@angular/core';
import { HubClient } from './hub-client';
import { Tag } from '../tags/tag.model';
import { BehaviorSubject } from 'rxjs';
import { Note } from '../notes/note.model';
import { filter } from 'rxjs/operators';

@Injectable()
export class Store {
  constructor(private _hubClient: HubClient) {
    this.note$ = new BehaviorSubject(<Note>{});
    this.notes$ = new BehaviorSubject([]);
    this.tags$ = new BehaviorSubject([]);
  }

  public note$: BehaviorSubject<Note>;

  public notes$: BehaviorSubject<Array<Note>>;

  public tags$: BehaviorSubject<Array<Tag>>;

  public handleTagSaved(payload: { tag: Tag }) {
    this.tags$.next([...this.tags$.value, payload.tag]);
  }

  public handleTagRemoved(payload: { tagId: number }) {
    const tags = this.tags$.value;
    const deletedTagIndex = tags.findIndex(x => x.tagId == payload.tagId);
    tags.splice(deletedTagIndex, 1);
    this.tags$.next([...tags]);
  }

  public get savedNotes$() {
    return this._hubClient.messages$.pipe(filter(x => x.type == '[Note] Saved'));
  }

  public get removedNotes$() {
    return this._hubClient.messages$.pipe(filter(x => x.type == '[Note] Removed'));
  }

  public get savedTags$() {
    return this._hubClient.messages$.pipe(filter(x => x.type == '[Tag] Saved'));
  }

  public get removedTags$() {
    return this._hubClient.messages$.pipe(filter(x => x.type == '[Tag] Removed'));
  }
}
