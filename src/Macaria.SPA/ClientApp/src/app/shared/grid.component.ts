import { Component, Input } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { ColDef } from "ag-grid";
import { Subject } from "rxjs";
import { DeleteCellComponent } from "./delete-cell.component";

@Component({
  templateUrl: "./grid.component.html",
  styleUrls: ["./grid.component.css"],
  selector: "app-grid"
})
export class GridComponent {
  constructor(private _translateService: TranslateService) { }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  @Input()
  public rowData: any;

  @Input()
  public columnDefs: ColDef[];

  public localeText: any = {
    "of": this._translateService.instant("of"),
    noRowsToShow: this._translateService.instant("No Rows To Show"),
    "Page": this._translateService.instant("Page"),
    "to": this._translateService.instant("to")
  };

  public frameworkComponents = {
    deleteRenderer: DeleteCellComponent
  };

  public onGridReady($event) {
    $event.api.sizeColumnsToFit();
  }
}
