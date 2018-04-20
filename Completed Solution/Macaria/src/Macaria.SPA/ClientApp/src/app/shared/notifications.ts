import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class Notifications {

  public message$: BehaviorSubject<string> = new BehaviorSubject("");
}
