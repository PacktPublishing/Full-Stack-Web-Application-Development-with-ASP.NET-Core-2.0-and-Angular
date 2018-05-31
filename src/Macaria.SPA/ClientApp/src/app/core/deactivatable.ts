import { Observable } from "rxjs";

export interface IDeactivatable {
  canDeactivate: () => Observable<boolean> | Promise<boolean> | boolean;
}
