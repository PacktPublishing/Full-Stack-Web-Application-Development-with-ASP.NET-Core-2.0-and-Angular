import { Component } from "@angular/core";
import { Subject } from "rxjs/Subject";

@Component({
    templateUrl: "./search.component.html",
    styleUrls: ["./search.component.css"],
    selector: "app-search"
})
export class SearchComponent { 

    public onDestroy: Subject<void> = new Subject<void>();

    ngOnDestroy() {
         this.onDestroy.next();	
    }
}
