import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';

import { TagsService } from './tags.service';
import { TagsPageComponent } from './tags-page.component';
import { AgGridComponentsModule } from '../ag-grid-components/ag-grid-components.module';
import { AddTagOverlayComponent } from './add-tag-overlay.component';
import { TagStore } from './tag-store';

const declarations = [
  TagsPageComponent,
  AddTagOverlayComponent
];

const providers = [
  TagsService,
  TagStore
];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,

    AgGridComponentsModule,
    SharedModule
  ],
  providers,
  entryComponents: [
    AddTagOverlayComponent
  ]
})
export class TagsModule { }
