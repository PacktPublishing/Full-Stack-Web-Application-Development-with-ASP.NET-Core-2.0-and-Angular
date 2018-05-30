import { Component } from '@angular/core';
import { Subject, pipe } from 'rxjs';
import { TagsService } from './tags.service';
import { Tag } from './tag.model';
import { Observable } from 'rxjs';
import { map, tap, filter } from 'rxjs/operators';
import { ColDef } from 'ag-grid';
import { takeUntil } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';
import { HubClient } from '../core/hub-client';
import { TranslateService } from '@ngx-translate/core';
import { DeleteCellComponent } from '../shared/delete-cell.component';
import { Store } from '../core/store';
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
    private _store: Store,
    private _tagsService: TagsService,
    private _translateService: TranslateService
  ) {}

  public localeText: any = {
    "of": this._translateService.instant("of"),
    noRowsToShow: this._translateService.instant("No Rows To Show"),
    "Page": this._translateService.instant("Page"),
    "to": this._translateService.instant("to")
  };

  ngOnInit() {
    this._tagsService
      .get()
      .pipe(takeUntil(this.onDestroy), map(x => this._store.tags$.next(x.tags)))
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

  public frameworkComponents = {
    deleteRenderer: DeleteCellComponent
  };

  public onGridReady(params) {
    params.api.sizeColumnsToFit();
  }

  public get tags$(): Observable<Array<Tag>> {
    return this._store.tags$;
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleDelete($event) {
    this._tagsService
      .remove({ tagId: $event.data.tagId })
      .pipe(takeUntil(this.onDestroy), tap(() => {
        const tags = [...this._store.tags$.value];
        const deletedTag = tags.findIndex(x => x.tagId == $event.data.tagId);
        tags.splice(deletedTag, 1);
        this._store.tags$.next(tags);
      })
      )
      .subscribe();
  }

  public handleCreateClick() {
    this._addTagOverlay.create()
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }
}
