import { Injectable } from "@angular/core";
import { CanDeactivate } from "@angular/router";
import { Observable } from "rxjs";
import { IDeactivatable } from "./deactivatable";

@Injectable()
export class CanDeactivateComponentGuard implements CanDeactivate<IDeactivatable> {  
  canDeactivate(
    component: IDeactivatable
  ): Observable<boolean> | Promise<boolean> | boolean {
    return component.canDeactivate ? component.canDeactivate() : true;
  }
}
