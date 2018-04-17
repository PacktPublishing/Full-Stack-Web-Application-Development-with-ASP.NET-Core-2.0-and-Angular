import { Component } from "@angular/core";
import { Subject } from "rxjs/Subject";

@Component({
    templateUrl: "./anonymous-master-page.component.html",
    styleUrls: ["./anonymous-master-page.component.css"],
    selector: "app-anonymous-master-page"
})
export class AnonymousMasterPageComponent { 

    public onDestroy: Subject<void> = new Subject<void>();

    ngOnDestroy() {
         this.onDestroy.next();	
    }
}
