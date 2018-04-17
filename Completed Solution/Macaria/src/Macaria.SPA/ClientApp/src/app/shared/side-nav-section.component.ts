import { Component } from "@angular/core";
import { Subject } from "rxjs/Subject";

@Component({
    templateUrl: "./side-nav-section.component.html",
    styleUrls: ["./side-nav-section.component.css"],
    selector: "app-side-nav-section"
})
export class SideNavSectionComponent { 

    public onDestroy: Subject<void> = new Subject<void>();

    ngOnDestroy() {
         this.onDestroy.next();	
    }
}
