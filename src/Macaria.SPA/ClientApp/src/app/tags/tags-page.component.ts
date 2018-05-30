import { Component } from '@angular/core';
import { Subject, pipe, BehaviorSubject } from 'rxjs';
import { TagsService } from './tags.service';
import { Tag } from './tag.model';
import { Observable } from 'rxjs';
import { map, tap, filter } from 'rxjs/operators';
import { ColDef } from 'ag-grid';
import { takeUntil } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';
import { HubClient } from '../core/hub-client';
import { TranslateService } from '@ngx-translate/core';
import { AddTagOverlay } from './add-tag-overlay';

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
      .pipe(takeUntil(this.onDestroy), map(x => this.tags$.next(x.tags)))
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
      .pipe(takeUntil(this.onDestroy), tap(() => {
        const tags = [...this.tags$.value];
        const deletedTag = tags.findIndex(x => x.tagId == $event.data.tagId);
        tags.splice(deletedTag, 1);
        this.tags$.next(tags);
      })
      )
      .subscribe();
  }

  public handleCreateClick() {
    this._addTagOverlay.create()
      .pipe(
        takeUntil(this.onDestroy),
        filter(x => x != null),
        map(x => this.tags$.next([...this.tags$.value, x]))
      )
      .subscribe();
  }
}
