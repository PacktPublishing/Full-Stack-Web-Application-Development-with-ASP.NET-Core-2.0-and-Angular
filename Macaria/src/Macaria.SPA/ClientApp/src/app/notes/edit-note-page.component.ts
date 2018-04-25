import { Component, ElementRef } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { NotesService } from './notes.service';
import { Note } from "./note.model";
import { constants } from '../shared/constants';
import { LocalStorageService } from '../shared/local-storage.service';
import { TagsService } from '../tags/tags.service';
import { pluckOut } from '../shared/pluck-out';
import { Observable } from 'rxjs';
import { addOrUpdate } from '../shared/add-or-update';
import { takeUntil, catchError, tap, map } from 'rxjs/operators';
import { Tag } from '../tags/tag.model';

import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";
import { TagStore } from '../tags/tag-store';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from '../shared/language.service';

var moment: any;

@Component({
  selector: 'app-edit-note-page',
  templateUrl: './edit-note-page.component.html',
  styleUrls: ['./edit-note-page.component.css']
})
export class EditNotePageComponent {
  constructor(
    private _activatedRoute: ActivatedRoute,
    private _elementRef: ElementRef,
    private _notesService: NotesService,
    private _languageService: LanguageService,
    private _localStorageService: LocalStorageService,
    private _tagsService: TagsService,
    private _tagStore: TagStore,
    private _router: Router
  ) {    
    this.editorPlaceholder = this._languageService.currentTranslations[this.editorPlaceholder];
  }

  ngOnInit() {
    if (this.slug)
      this._notesService.getBySlug({ slug: this.slug })
        .pipe(
          map(x => this.note$.next(x.note))
        )
        .subscribe();
  }
  ngAfterViewInit() {
    this._tagsService.get()
      .pipe(takeUntil(this.onDestroy))
      .subscribe(x => this._tagStore.tags$.next(x.tags));
  }

  public notes$: BehaviorSubject<Note> = new BehaviorSubject(<Note>{});

  public get tags$():Observable<Array<Tag>> {
    return this._tagStore.tags$;
  }

  public note$: BehaviorSubject<Note> = new BehaviorSubject(new Note());

  public onDestroy: Subject<void> = new Subject();

  public quillEditorFormControl: FormControl = new FormControl('');
  
  public handleSave() {
    
    this._notesService.save({
      note: this.form.value,
    })
      .pipe(
        takeUntil(this.onDestroy),
        tap(() => this._router.navigateByUrl("/notes"))
      )
      .subscribe();
  }

  public editorPlaceholder: string = "Compose a note...";

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

  public get slug():string {
    return this._activatedRoute.snapshot.params["slug"];  
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
