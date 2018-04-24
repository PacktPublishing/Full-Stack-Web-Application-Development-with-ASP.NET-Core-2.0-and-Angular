import { Component } from "@angular/core";
import { Subject } from 'rxjs';

@Component({
    templateUrl: "./master-page.component.html",
    styleUrls: ["./master-page.component.css"],
    selector: "app-master-page"
})
export class MasterPageComponent { 

    public onDestroy: Subject<void> = new Subject<void>();

    ngOnDestroy() {
         this.onDestroy.next();	
    }
}
