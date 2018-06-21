import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { IAfterGuiAttachedParams, ICellRendererParams } from 'ag-grid';

@Component({
  templateUrl: './un-delete-cell.component.html',
  selector: 'app-un-delete-cell'
})
export class UnDeleteCellComponent implements ICellRendererAngularComp {
  refresh(params: any): boolean {
    return true;
  }

  agInit(params: ICellRendererParams): void { }

  afterGuiAttached?(params?: IAfterGuiAttachedParams): void { }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
