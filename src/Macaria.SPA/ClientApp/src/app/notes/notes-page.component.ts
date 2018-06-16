import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ColDef } from 'ag-grid';
import { BehaviorSubject, Observable, Subject, of } from 'rxjs';
import { filter, map, switchMap, takeUntil } from 'rxjs/operators';
import { HubClient } from '../core/hub-client';
import { ConfirmRefreshOverlayComponent } from '../shared/confirm-refresh-overlay.component';
import { Note } from './note.model';
import { NotesService } from './notes.service';

@Component({
  templateUrl: './notes-page.component.html',
  styleUrls: ['./notes-page.component.css'],
  selector: 'app-notes-page'
})
export class NotesPageComponent {
  private _openConfirmRefreshDialog$(): Observable<any> {
    return this._dialog.open(ConfirmRefreshOverlayComponent, { width: '250px' }).afterClosed();
  }

  private _handleNoteSavedMessage$(messageResult:any): Observable<any> {
    return this._openConfirmRefreshDialog$()
      .pipe(
        map(dialogResult => { return { messageResult, dialogResult } }),
        filter(x => x.dialogResult == true),
        switchMap(x => this._notesService.get()),
        map(x => this.notes$.next(x.notes))
    );
  }

  private _handleNoteRemovedMessage$(messageResult:any): Observable<any> {
    return this._openConfirmRefreshDialog$()
      .pipe(
        map(dialogResult => { return { messageResult, dialogResult } }),
        filter(x => x.dialogResult == true),
        map(x => {
          const removedNoteId = x.messageResult.payload.noteId;
          const index = this.notes$.value.findIndex(y => y.noteId == removedNoteId);
          if (index > -1) {
            const notes = this.notes$.value;
            notes.splice(index, 1);
            this.notes$.next([...notes]);
          }
        })
      );
  }

  constructor(
    private _dialog: MatDialog,
    private _hubClient: HubClient,
    private _notesService: NotesService,
    private _router: Router,
    private _translateService: TranslateService
  ) {}

  public notes$: BehaviorSubject<Note[]> = new BehaviorSubject([]);

  public onDestroy: Subject<void> = new Subject<void>();
  
  ngOnInit() {
    this._notesService
      .get()
      .pipe(takeUntil(this.onDestroy))
      .subscribe(x => this.notes$.next(x.notes));

    this._hubClient.messages$
      .pipe(
        takeUntil(this.onDestroy),
        switchMap(messageResult => {
          if (messageResult.type == "[Note] Removed" && this.hasNote(messageResult.payload.noteId))
            return this._handleNoteRemovedMessage$(messageResult);

          if (messageResult.type == "[Note] Saved")
            return this._handleNoteSavedMessage$(messageResult);

          return of(null);
        })
      )
      .subscribe();
  }

  public hasNote(noteId: number) { return this.notes$.value.findIndex(x => x.noteId == noteId) > -1; }
  
  public handleDelete($event) {
    const notes = this.notes$.value;
    const index = notes.findIndex(x => x.noteId == $event.data.noteId);
    notes.splice(index, 1);
    this.notes$.next([...notes]);

    this._notesService
      .remove({ note: <Note>$event.data })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleTitleClick($event) {
    this._router.navigateByUrl(`/notes/${$event.data.slug}`);
  }

  public columnDefs: Array<ColDef> = [
    {
      headerName: this._translateService.instant('Title'),
      field: 'title',
      onCellClicked: $event => this.handleTitleClick($event)
    },
    {
      headerName: this._translateService.instant('Last Modified On'),
      field: 'lastModifiedOn',
      width:100
    },
    {
      cellRenderer: 'deleteRenderer',
      onCellClicked: $event => this.handleDelete($event),
      width: 20
    }
  ];
  
  ngOnDestroy() {
    this.onDestroy.next();
  }
}
