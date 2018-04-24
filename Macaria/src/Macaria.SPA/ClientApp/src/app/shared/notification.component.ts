import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { Notifications } from "./notifications";

@Component({
  templateUrl: "./notification.component.html",
  styleUrls: ["./notification.component.css"],
  selector: "app-notification"
})
export class NotificationComponent { 
  constructor(
    public notifications:Notifications
  ) {

  }
  
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
