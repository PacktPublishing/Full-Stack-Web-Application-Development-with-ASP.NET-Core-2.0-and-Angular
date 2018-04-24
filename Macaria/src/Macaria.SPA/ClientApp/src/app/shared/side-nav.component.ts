import { Component } from "@angular/core";
import { Subject } from 'rxjs';

@Component({
    templateUrl: "./side-nav.component.html",
    styleUrls: ["./side-nav.component.css"],
    selector: "app-side-nav"
})
export class SideNavComponent { 

    public onDestroy: Subject<void> = new Subject<void>();

    ngOnDestroy() {
         this.onDestroy.next();	
    }
}
