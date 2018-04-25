import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { Note } from "./note.model";

@Injectable()
export class NoteStore {
  constructor() {
    this.notes$ = new BehaviorSubject([]);
    this.note$ = new BehaviorSubject(<Note>{});
  }

  public notes$: BehaviorSubject<Array<Note>>;
  public note$: BehaviorSubject<Note>;
}
