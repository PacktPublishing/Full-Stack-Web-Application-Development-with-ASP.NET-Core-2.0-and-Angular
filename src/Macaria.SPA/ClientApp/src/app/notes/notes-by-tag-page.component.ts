import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Tag } from '../tags/tag.model';
import { TagsService } from '../tags/tags.service';

@Component({
  templateUrl: './notes-by-tag-page.component.html',
  styleUrls: ['./notes-by-tag-page.component.css'],
  selector: 'app-notes-by-tag-page'
})
export class NotesByTagPageComponent {
  constructor(private _activatedRoute: ActivatedRoute, private _tagsService: TagsService) {}
  
  public tag$: Observable<Tag> = this._tagsService.getBySlug({ slug: this.slug }).pipe(map(x => x.tag));
  
  public get slug() {
    return this._activatedRoute.snapshot.params['slug'];
  }  
}
