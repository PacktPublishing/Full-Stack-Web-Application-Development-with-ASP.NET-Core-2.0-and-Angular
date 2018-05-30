import { Component } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import { NotesService } from './notes.service';
import { Note } from './note.model';
import { Observable } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ColDef } from 'ag-grid';

@Component({
  templateUrl: './notes-page.component.html',
  styleUrls: ['./notes-page.component.css'],
  selector: 'app-notes-page'
})
export class NotesPageComponent {
  constructor(
    private _notesService: NotesService,
    private _router: Router,
    private _translateService: TranslateService
  ) {}

  public notes$: BehaviorSubject<Note[]> = new BehaviorSubject([]);

  public onDestroy: Subject<void> = new Subject<void>();
  
  ngOnInit() {
    this._notesService
      .get()
      .pipe(map(x => this.notes$.next(x.notes)))
      .subscribe();
  }

  public handleDelete($event) {
    const notes = this.notes$.value;
    const deletedNoteIndex = notes.findIndex(x => x.noteId == $event.data.noteId);

    notes.splice(deletedNoteIndex, 1);

    this._notesService
      .remove({ note: <Note>$event.data })
      .pipe(
        takeUntil(this.onDestroy),
        tap(x => {
          this.notes$.next([...notes]);
        })
      )
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
      cellRenderer: 'deleteRenderer',
      onCellClicked: $event => this.handleDelete($event),
      width: 20
    }
  ];
  
  ngOnDestroy() {
    this.onDestroy.next();
  }
}
