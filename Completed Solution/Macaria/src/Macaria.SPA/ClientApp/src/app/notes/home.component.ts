import { Component, ElementRef } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { NotesService } from './notes.service';
import { Note } from "./note.model";
import { constants } from '../shared/constants';
import { LocalStorageService } from '../shared/local-storage.service';
import { TagsService } from '../tags/tags.service';
import { pluckOut } from '../shared/pluck-out';
import { Observable } from 'rxjs/Observable';
import { addOrUpdate } from '../shared/add-or-update';
import { takeUntil, catchError, tap } from 'rxjs/operators';
import { Tag } from '../tags/tag.model';
import { MatSnackBar } from '@angular/material';
import { NotificationComponent } from '../shared/notification.component';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { Notifications } from '../shared/notifications';

import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";

var moment: any;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: [
    './home.component.css'
  ]
})
export class HomeComponent {
  constructor(
    private _activatedRoute: ActivatedRoute,
    private _elementRef: ElementRef,
    private _notesService: NotesService,
    private _localStorageService: LocalStorageService,
    private _notifications: Notifications,
    private _snackBar: MatSnackBar,
    private _tagsService: TagsService,
    private _router: Router
  ) { }

  ngAfterViewInit() {
    this._tagsService.get()
      .pipe(takeUntil(this.onDestroy))
      .subscribe(x => this.tags$.next(x.tags));
  }

  public showErrorMessage(options: { message: string }) {
    this._notifications.message$.next(options.message);

    this._snackBar.openFromComponent(NotificationComponent, {
      duration:3000
    });
  }

  public get textEditor() { return this._elementRef.nativeElement.querySelector("ce-quill-text-editor"); }

  public tags$: BehaviorSubject<Array<Tag>> = new BehaviorSubject([]);

  public note$: BehaviorSubject<Note> = new BehaviorSubject(new Note());

  public onDestroy: Subject<void> = new Subject();

  public quillEditorFormControl: FormControl = new FormControl('');
  
  public handleSave() {
    
    this._notesService.save({
      note: this.form.value,
    })
      .pipe(
        takeUntil(this.onDestroy),
        tap(() => this._router.navigateByUrl("/notes")),
        catchError((error:Error) => {
          this.showErrorMessage({ message:  error.message });
          return ErrorObservable.create(error);
        })
      )
      .subscribe();
  }

  public handleNoteTagClicked($event) {    
    const tag = <Tag>$event.tag;
    
    if (this.note$.value.tags.find((x) => x.tagId == tag.tagId) != null) {
      this._notesService.removeTag({ noteId: this.note$.value.noteId, tagId: tag.tagId })
        .pipe(
          takeUntil(this.onDestroy)
        )
        .subscribe();

      pluckOut({ items: this.note$.value.tags, value: tag.tagId, key: "tagId" });

      return;
    }

    this._notesService
      .addTag({ noteId: this.note$.value.noteId, tagId: tag.tagId })
      .pipe(
        takeUntil(this.onDestroy)
      )
      .subscribe();

    addOrUpdate({ items: this.note$.value.tags, item: tag, key:"tagId" });
  }

  public note: Note = <Note>{};
  
  public form = new FormGroup({
    title: new FormControl(this.note.title, [Validators.required]),
    body: new FormControl(this.note.body, [Validators.required]),
  });

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
