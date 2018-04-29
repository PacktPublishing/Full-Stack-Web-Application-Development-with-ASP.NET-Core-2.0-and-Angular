import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { ControlValueAccessor } from "@angular/forms";

@Component({
  templateUrl: "./auto-complete-chip-list.component.html",
  styleUrls: ["./auto-complete-chip-list.component.css"],
  selector: "app-auto-complete-chip-list"
})
export class AutoCompleteChipListComponent implements ControlValueAccessor { 

  writeValue(obj: any): void {

  }
  registerOnChange(fn: any): void {

  }

  registerOnTouched(fn: any): void {

  }

  setDisabledState?(isDisabled: boolean): void {
    
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
