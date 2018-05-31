import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { OverlayRefWrapper } from '../core/overlay-ref-wrapper';
import { Tag } from './tag.model';
import { TagsService } from './tags.service';

@Component({
  templateUrl: './add-tag-overlay.component.html',
  styleUrls: ['./add-tag-overlay.component.css'],
  selector: 'app-add-tag-overlay'
})
export class AddTagOverlayComponent {
  constructor(
    private _overlay: OverlayRefWrapper,    
    private _tagService: TagsService
  ) {}

  public handleCancel() {
    this._overlay.close();
  }

  public handleSave(tag: Tag) {
    this._tagService
      .save({ tag })
      .pipe(
        takeUntil(this.onDestroy),
        map((result: { tagId: number }) => tag.tagId = result.tagId)
      )
      .subscribe(() => this._overlay.close(tag));
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
