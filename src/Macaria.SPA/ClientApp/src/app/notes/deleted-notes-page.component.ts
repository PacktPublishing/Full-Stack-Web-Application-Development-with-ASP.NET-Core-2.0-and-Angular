import { Component } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { NotesService } from "./notes.service";
import { Note } from "./note.model";
import { map, takeUntil } from "rxjs/operators";
import { ColDef } from "ag-grid";
import { TranslateService } from "@ngx-translate/core";

@Component({
  templateUrl: "./deleted-notes-page.component.html",
  styleUrls: ["./deleted-notes-page.component.css"],
  selector: "app-deleted-notes-page"
})
export class DeletedNotesPageComponent { 
  constructor(
    private _activatedRoute: ActivatedRoute,
    private _notesService: NotesService,
    private _router: Router,
    private _translateService: TranslateService
  ) {

  }

  ngOnInit() {
    this._notesService.getDeleted()
      .pipe(map(x => this.notes$.next(x.notes), takeUntil(this.onDestroy)))
      .subscribe();
  }

  public columnDefs: Array<ColDef> = [
    {
      headerName: this._translateService.instant('Title'),
      field: 'title'
    },
    {
      headerName: this._translateService.instant('Last Modified On'),
      field: 'lastModifiedOn',
      width: 100
    },
    {
      cellRenderer: 'unDeleteRenderer',
      onCellClicked: $event => this.handleUnDelete($event),
      width: 20
    }
  ];

  public handleUnDelete($event) {
    const notes = this.notes$.value;
    const index = notes.findIndex(x => x.noteId == $event.data.noteId);
    notes.splice(index, 1);
    this.notes$.next([...notes]);

    this._notesService
      .undelete({ noteId: +$event.data.noteId })
      .pipe(takeUntil(this.onDestroy))
      .subscribe(() => this._router.navigateByUrl("/notes"));
  }
  public notes$: BehaviorSubject<Note[]> = new BehaviorSubject([]);

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
