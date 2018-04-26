import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./tag-page.component.html",
  styleUrls: ["./tag-page.component.css"],
  selector: "app-tag-page"
})
export class TagPageComponent { 
  constructor() {

  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
