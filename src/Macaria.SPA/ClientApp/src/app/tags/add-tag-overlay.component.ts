import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { OverlayRefWrapper } from '../core/overlay-ref-wrapper';
import { TagsService } from './tags.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Tag } from './tag.model';
import { map, tap } from 'rxjs/operators';
import { Store } from '../core/store';

@Component({
  templateUrl: './add-tag-overlay.component.html',
  styleUrls: ['./add-tag-overlay.component.css'],
  selector: 'app-add-tag-overlay'
})
export class AddTagOverlayComponent {
  constructor(
    private _overlay: OverlayRefWrapper,
    private _store: Store,
    private _tagService: TagsService) { }

  public handleCancel() {
    this._overlay.close();
  }

  public handleSave(tag: Tag) {
    this._tagService
      .save({ tag })
      .pipe(map((result: any) => {
        alert(JSON.stringify(result));
        const tags = this._store.tags$.value;
        tag.tagId = result.tagId;
        this._store.tags$.next([...tags,tag])
      }), tap(() => this._overlay.close()))
      .subscribe();
  }

  public tag: Tag = <Tag>{};

  public form = new FormGroup({
    name: new FormControl(this.tag.name, [Validators.required])
  });

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
