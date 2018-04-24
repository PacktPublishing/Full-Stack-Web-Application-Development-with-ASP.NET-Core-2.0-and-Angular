import { Component } from "@angular/core";
import { Subject } from 'rxjs';

@Component({
    templateUrl: "./side-nav-item.component.html",
    styleUrls: ["./side-nav-item.component.css"],
    selector: "app-side-nav-item"
})
export class SideNavItemComponent { 

    public onDestroy: Subject<void> = new Subject<void>();

    ngOnDestroy() {
         this.onDestroy.next();	
    }
}
