import { Component, ElementRef } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import { ActivatedRoute, Router, Event } from '@angular/router';
import { NotesService } from './notes.service';
import { Note } from './note.model';
import { LocalStorageService } from '../core/local-storage.service';
import { Observable } from 'rxjs';
import { takeUntil, catchError, tap, map, startWith } from 'rxjs/operators';
import { Tag } from '../tags/tag.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LanguageService } from '../core/language.service';
import { MatInput } from '@angular/material';
import { Store } from '../core/store';

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
    private _languageService: LanguageService,
    private _localStorageService: LocalStorageService,
    private _notesService: NotesService,
    private _router: Router,
    private _store: Store
  ) {
    this.editorPlaceholder = this._languageService.currentTranslations[this.editorPlaceholder];

    this.selectedItems = this._store.note$.value.tags.map(x => x.name);
    this.items = this._store.tags$.value;
  }

  selectedItems: any[];
  items: any[];

  canDeactivate() {
    !this.form.dirty;
  }

  ngAfterViewInit() {
    this.form.patchValue({
      title: this.note$.value.title,
      body: this.note$.value.body
    });
  }

  public notes$: BehaviorSubject<Note> = new BehaviorSubject(<Note>{});

  public get tags$(): Observable<Array<Tag>> {
    return this._store.tags$;
  }

  public get note$(): BehaviorSubject<Note> {
    return this._store.note$;
  }

  public onDestroy: Subject<void> = new Subject();

  public quillEditorFormControl: FormControl = new FormControl('');

  public handleSaveClick() {
    let note = new Note();

    note.noteId = this._store.note$.value.noteId;
    note.title = this.form.value.title;
    note.body = this.form.value.body;
    note.tags = this.form.value.tags.map(x => this._store.tags$.value.find(t => t.name == x));

    this._notesService
      .save({
        note
      })
      .pipe(takeUntil(this.onDestroy), tap(() => this._router.navigateByUrl('/notes')))
      .subscribe();
  }

  public editorPlaceholder: string = 'Compose a note...';

  public form = new FormGroup({
    title: new FormControl(this._store.note$.value.title, [Validators.required]),
    body: new FormControl(this._store.note$.value.body, [Validators.required]),
    tags: new FormControl()
  });

  public get slug(): string {
    return this._activatedRoute.snapshot.params['slug'];
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }

  filterItems(itemName: string) {
    return this.items.filter(item => item.name.toLowerCase().indexOf(itemName.toLowerCase()) === 0);
  }

  handleChipClicked($event) {
    const tag = this._store.tags$.value.find(x => x.name == $event.item);
    this._router.navigate(['tags', tag.slug]);
  }
}
