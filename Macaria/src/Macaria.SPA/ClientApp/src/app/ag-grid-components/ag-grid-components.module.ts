import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgGridModule } from 'ag-grid-angular';
import { MaterialModule } from '../material/material.module';
import { DeleteCellComponent } from './delete-cell.component';

const declarations = [
  DeleteCellComponent
];

@NgModule({
  imports: [
    CommonModule,
    AgGridModule.withComponents([
      ...declarations
    ]),
    MaterialModule
  ],
  declarations,
  exports:[AgGridModule]
})
export class AgGridComponentsModule { }
