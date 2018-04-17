import { Component, ElementRef } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { NotesService } from './notes.service';
import { Note } from "./note.model";
import { constants } from '../shared/constants';
import { LocalStorageService } from '../shared/local-storage.service';
import { SpeechRecognitionService } from '../speech-recognition/speech-recognition.service';
import { TagsService } from '../tags/tags.service';
import { FormControl } from '@angular/forms';
import { Tag } from '../tags';
import { pluckOut } from '../shared/pluck-out';
import { Observable } from 'rxjs/Observable';
import { addOrUpdate } from '../shared/add-or-update';
import { takeUntil } from 'rxjs/operators';

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
    private _speechRecognitionService: SpeechRecognitionService,
    private _tagsService: TagsService
  ) {

    //_activatedRoute.params
    //  .takeUntil(this.onDestroy)
    //  .switchMap(params => params["slug"] != null
    //    ? _notesService.getBySlugAndCurrentUser({ slug: params["slug"] })
    //    : _notesService.getByTitleAndCurrentUser({ title: moment().format(constants.DATE_FORMAT) })
    //  )
    //  .map(x => x.note)
    //  .subscribe(note => this.note$.next(note || this.note$.value));
  }

  ngAfterViewInit() {
    this._tagsService.get()
      .pipe(takeUntil(this.onDestroy))
      .subscribe(x => this.tags$.next(x.tags));

    //if (constants.SUPPORTS_SPEECH_RECOGNITION)
    //  this.startDictationBehavior();

    //Observable
    //  .fromEvent(this.textEditor, "keyup")
    //  .takeUntil(this.onDestroy)
    //  .debounce(() => Observable.timer(300))
    //  .switchMap(() => this._notesService.save({
    //    note: {
    //      noteId: this.note$.value.noteId,
    //      title: this.note$.value.title,
    //      body: this.quillEditorFormControl.value
    //    }
    //  }))
    //  .subscribe();
  }

  public get textEditor() { return this._elementRef.nativeElement.querySelector("ce-quill-text-editor"); }

  public tags$: BehaviorSubject<Array<Tag>> = new BehaviorSubject([]);

  public note$: BehaviorSubject<Note> = new BehaviorSubject(new Note());

  public onDestroy: Subject<void> = new Subject();

  public quillEditorFormControl: FormControl = new FormControl('');

  public startDictationBehavior() {
    this._speechRecognitionService.start();

    this._speechRecognitionService.finalTranscript$
      .takeUntil(this.onDestroy)
      .filter(x => x && x.length > 0)
      .map(x => this.quillEditorFormControl.patchValue(`${this.quillEditorFormControl.value}<p>${x}</p>`))      
      .switchMap(() => this._notesService.save({
        note: {
          noteId: this.note$.value.noteId,
          title: this.note$.value.title,
          body: this.quillEditorFormControl.value
        },
      }))
      .subscribe();
  }

  public handleNoteTagClicked($event) {    
    const tag = <Tag>$event.tag;
    
    if (this.note$.value.tags.find((x) => x.tagId == tag.tagId) != null) {
      this._notesService.removeTag({ noteId: this.note$.value.noteId, tagId: tag.tagId })
        .subscribe();

      pluckOut({ items: this.note$.value.tags, value: tag.tagId, key: "tagId" });

      return;
    }

    this._notesService
      .addTag({ noteId: this.note$.value.noteId, tagId: tag.tagId })
      .subscribe();

    addOrUpdate({ items: this.note$.value.tags, item: tag, key:"tagId" });
  }

  ngOnDestroy() {
    this.onDestroy.next();
    this._speechRecognitionService.stop();
  }
}
