import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { ColDef } from 'ag-grid';
import { BehaviorSubject, Subject } from 'rxjs';
import { filter, map, takeUntil } from 'rxjs/operators';
import { HubClient } from '../core/hub-client';
import { AddTagOverlay } from './add-tag-overlay';
import { Tag } from './tag.model';
import { TagsService } from './tags.service';

@Component({
  templateUrl: './tags-page.component.html',
  styleUrls: ['./tags-page.component.css'],
  selector: 'app-tags-page'
})
export class TagsPageComponent {
  constructor(
    private _addTagOverlay: AddTagOverlay,
    private _hubClient: HubClient,    
    private _snackBar: MatSnackBar,
    private _tagsService: TagsService,
    private _translateService: TranslateService
  ) {}


  ngOnInit() {
    this._tagsService
      .get()
      .pipe(map(x => this.tags$.next(x.tags)), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleChange($event) {
    this._tagsService
      .save({ tag: $event.data })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public columnDefs: Array<ColDef> = [
    {
      headerName: this._translateService.instant("Name"),
      field: 'name',
      onCellValueChanged: $event => this.handleChange($event),
      editable: true
    },
    {
      cellRenderer: 'deleteRenderer',
      onCellClicked: $event => this.handleDelete($event),
      width: 20
    }
  ];

  public tags$: BehaviorSubject<Tag[]> = new BehaviorSubject([]);

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleDelete($event) {
    this._tagsService
      .remove({ tagId: $event.data.tagId })
      .pipe(takeUntil(this.onDestroy))
      .subscribe(() => {
        const tags = [...this.tags$.value];
        const deletedTag = tags.findIndex(x => x.tagId == $event.data.tagId);
        tags.splice(deletedTag, 1);
        this.tags$.next(tags);
      });
  }

  public handleCreateClick() {
    this._addTagOverlay.create()
      .pipe(
        filter(x => x != null),
        map(x => this.tags$.next([...this.tags$.value, x])),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }
}
