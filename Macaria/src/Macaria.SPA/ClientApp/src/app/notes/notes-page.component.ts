import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { NotesService } from "./notes.service";
import { Note } from "./note.model";
import { Observable } from "rxjs";
import { map, takeUntil, tap } from "rxjs/operators";
import { ColDef } from "ag-grid";
import { DeleteCellComponent } from "../ag-grid-components/delete-cell.component";
import { NoteStore } from "./tag-store";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";

@Component({
  templateUrl: "./notes-page.component.html",
  styleUrls: ["./notes-page.component.css"],
  selector: "app-notes-page"
})
export class NotesPageComponent { 
  constructor(
    private _notesService: NotesService,
    private _noteStore: NoteStore,
    private _router: Router,
    private _translateService: TranslateService
  ) { }

  public onDestroy: Subject<void> = new Subject<void>();

  public localeText: any = {};

  ngOnInit() {
    this._notesService.getByCurrentUser()
      .pipe(
        map(x => this._noteStore.notes$.next(x.notes))
      )
      .subscribe();

    this._translateService.get(["Title", "Page", "of", "to"])
      .pipe(
        
        tap((translations) => {
        this.localeText = translations;
          this.columnDefs = [
            { headerName: translations["Title"], field: "title", onCellClicked: ($event) => this.handleTitleClick($event) },
            { cellRenderer: "deleteRenderer", onCellClicked: ($event) => this.handleDelete($event), width: 20 }
          ];
        })
      ) 
      .subscribe();
  }

  public handleDelete($event) {
    const notes = this._noteStore.notes$.value;
    const deletedNoteIndex = notes.findIndex(x => x.noteId == $event.data.noteId);

    notes.splice(deletedNoteIndex, 1);

    this._notesService.remove({ note: <Note>$event.data })
      .pipe(
        takeUntil(this.onDestroy),
        tap(x => {
          this._noteStore.notes$.next([...notes]);
        })
      )
      .subscribe();
  }

  public handleTitleClick($event) {
    this._router.navigateByUrl(`/notes/${$event.data.slug}`);
  }

  public frameworkComponents = {
    deleteRenderer: DeleteCellComponent
  };

  public columnDefs: Array<ColDef> = [];

  public onGridReady($event) { $event.api.sizeColumnsToFit(); }

  public get notes$(): Observable<Array<Note>> {
    return this._noteStore.notes$;
  }

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
