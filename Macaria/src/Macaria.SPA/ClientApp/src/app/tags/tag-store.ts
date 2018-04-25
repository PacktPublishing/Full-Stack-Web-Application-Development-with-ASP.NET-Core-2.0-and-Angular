import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { Tag } from "./tag.model";
import { HubClient } from "../core/hub-client";
import { filter, map } from "rxjs/operators";
import { PathLocationStrategy } from "@angular/common";

@Injectable()
export class TagStore {
  constructor(private _hubClient: HubClient) {

    this.tags$ = new BehaviorSubject([]);

    this.savedTags$
      .pipe(
        map((x:any) => this.handleTagSaved(x.payload))
      )
      .subscribe();

    this.removedTags$
      .pipe(
        map((x: any) => this.handleTagRemoved(x.payload))
      )
      .subscribe();
  }

  public handleTagSaved(payload: { tag: Tag }) {    
    this.tags$.next([
      ...this.tags$.value,
      payload.tag
    ]);
  }

  public handleTagRemoved(payload: { tagId: number }) {
    const tags = this.tags$.value;
    const deletedTagIndex = tags.findIndex(x => x.tagId == payload.tagId);    
    tags.splice(deletedTagIndex, 1);
    this.tags$.next([...tags]);
  }

  public tags$: BehaviorSubject<Array<Tag>>;

  public get savedTags$() {
    return this._hubClient.messages$
      .pipe(
        filter(x => x.type == "[Tag] Saved")
      );
  }

  public get removedTags$() {
    return this._hubClient.messages$
      .pipe(
        filter(x => x.type == "[Tag] Removed")
      );
  }
}
