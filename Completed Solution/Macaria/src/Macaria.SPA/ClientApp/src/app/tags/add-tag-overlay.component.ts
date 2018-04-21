import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { OverlayRefWrapper } from "../shared/overlay-ref-wrapper";
import { TagsService } from "./tags.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Tag } from "./tag.model";
import { tap } from "rxjs/operators";
import { TagStore } from "./tag-store";

@Component({
  templateUrl: "./add-tag-overlay.component.html",
  styleUrls: ["./add-tag-overlay.component.css"],
  selector: "app-add-tag-overlay"
})
export class AddTagOverlayComponent { 
  constructor(
    private _overlay: OverlayRefWrapper,
    private _tagService: TagsService,
    private _tagStore: TagStore
  ) { }

  public handleCancel() {
    this._overlay.close();
  }

  public handleSave(tag:Tag) {
    this._tagService.save({ tag })
      .pipe(        
        tap(() => this._overlay.close())
      )
      .subscribe();
  }

  public tag: Tag = <Tag>{};

  public form = new FormGroup({
    name: new FormControl(this.tag.name, [Validators.required])
  });

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() { this.onDestroy.next(); }
}
