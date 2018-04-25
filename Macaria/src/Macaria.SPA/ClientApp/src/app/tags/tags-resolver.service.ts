import { Resolve } from "@angular/router";
import { Tag } from "./tag.model";
import { Observable } from "rxjs";
import { RouterStateSnapshot } from "@angular/router";
import { ActivatedRouteSnapshot } from "@angular/router";
import { TagsService } from "./tags.service";

export class TagsResolver implements Resolve<Array<Tag>> {
  constructor(private _tagsService: TagsService) {

  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Tag[]> {
    throw new Error("Method not implemented.");
  }
}
