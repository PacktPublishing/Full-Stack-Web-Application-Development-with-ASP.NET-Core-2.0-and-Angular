import { Component, ElementRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable, Subject, of } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { IDeactivatable } from '../core/deactivatable';
import { LanguageService } from '../core/language.service';
import { LocalStorageService } from '../core/local-storage.service';
import { AreYouSureOverlayComponent } from '../shared/are-you-sure-overlay.component';
import { Tag } from '../tags/tag.model';
import { TagsService } from '../tags/tags.service';
import { Note } from './note.model';
import { NotesService } from './notes.service';

@Component({
  selector: 'app-edit-note-page',
  templateUrl: './edit-note-page.component.html',
  styleUrls: ['./edit-note-page.component.css']
})
export class EditNotePageComponent implements IDeactivatable {
  constructor(
    private _activatedRoute: ActivatedRoute,
    private _dialog: MatDialog,
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
        .subscribe(x => {
          this.note = x.note;
          this.selectedItems = this.note.tags.map(x => x.name);
          this.form.patchValue({
            title: this.note.title,
            body: this.note.body,
            tags: this.selectedItems
          });
        });

    this._tagService.get()
      .pipe(takeUntil(this.onDestroy))
      .subscribe(x => this.items$.next(x.tags));
  }

  note: Note = new Note();

  selectedItems: any[] = this.note.tags.map(x => x.name);
  
  items$: BehaviorSubject<Tag[]> = new BehaviorSubject([]);

  public canDeactivate(): Observable<boolean> {
    if (this.form.dirty)
      return this._dialog.open(AreYouSureOverlayComponent, {
        width: '250px'
      }).afterClosed();
    
    return of(true);
  }
  
  public onDestroy: Subject<void> = new Subject();
  
  public handleSaveClick() {
    let note = new Note();
    let tags = this.form.value.tags || [];

    note.noteId = this.note.noteId;
    note.title = this.form.value.title;
    note.body = this.form.value.body;
    note.tags = tags.map(x => this.items$.value.find(t => t.name == x));

    this._notesService
      .save({ note })
      .pipe(takeUntil(this.onDestroy))
      .subscribe(() => {
        this.form.reset();
        this._router.navigateByUrl('/notes');
      });
  }

  public editorPlaceholder: string = this._translateService.instant('Compose a note...');

  public form: FormGroup = new FormGroup({
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
    return this.items$.value.filter(item => item.name.toLowerCase().indexOf(itemName.toLowerCase()) === 0);
  }

  handleChipClicked($event) {
    const tag = this.items$.value.find(x => x.name == $event.item);
    this._router.navigate(['tags', tag.slug]);
  }
}
