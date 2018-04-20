import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { Tag } from "./tag.model";

@Injectable()
export class TagStore {
  constructor() {
    this.tags$ = new BehaviorSubject([]);
  }

  public tags$: BehaviorSubject<Array<Tag>>;
}
