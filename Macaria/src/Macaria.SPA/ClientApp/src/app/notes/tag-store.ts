import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { Note } from ".";

@Injectable()
export class NoteStore {
  constructor() {
    this.notes$ = new BehaviorSubject([]);
  }

  public notes$: BehaviorSubject<Array<Note>>;
}
