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
import { TranslateService } from '@ngx-translate/core';
import { TagsService } from '../tags/tags.service';

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
    private _tagService: TagsService,
    private _translateService: TranslateService
  ) { }

  public ngOnInit() {
    if (this.slug)
      this._notesService
        .getBySlug({ slug: this.slug })
        .pipe(tap(x => {
          this.note = x.note;
          this.selectedItems = this.note.tags.map(x => x.name);
          this.form.patchValue({
            title: this.note.title,
            body: this.note.body
          });
        })).subscribe();

    this._tagService.get().pipe(map(x => {
      this.items = x.tags;
      this.items$.next(x.tags);
    })).subscribe();
  }

  note: Note = new Note();

  selectedItems: any[] = this.note.tags.map(x => x.name);

  items: any[];

  items$: Subject<any> = new Subject();

  canDeactivate() {
    !this.form.dirty;
  }
  
  public onDestroy: Subject<void> = new Subject();

  public quillEditorFormControl: FormControl = new FormControl('');

  public handleSaveClick() {
    let note = new Note();
    let tags = this.form.value.tags || [];

    note.noteId = this.note.noteId;
    note.title = this.form.value.title;
    note.body = this.form.value.body;
    note.tags = tags.map(x => this.items.find(t => t.name == x));

    this._notesService
      .save({ note })
      .pipe(takeUntil(this.onDestroy), tap(() => this._router.navigateByUrl('/notes')))
      .subscribe();
  }

  public editorPlaceholder: string = this._translateService.instant('Compose a note...');

  public form = new FormGroup({
    title: new FormControl(this.note.title, [Validators.required]),
    body: new FormControl(this.note.body, [Validators.required]),
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
    const tag = this.items.find(x => x.name == $event.item);
    this._router.navigate(['tags', tag.slug]);
  }
}
