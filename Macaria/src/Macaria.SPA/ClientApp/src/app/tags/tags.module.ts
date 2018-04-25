import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { CoreModule } from '../core/core.module';
import { TagsService } from './tags.service';
import { TagsPageComponent } from './tags-page.component';
import { AddTagOverlayComponent } from './add-tag-overlay.component';
import { TagStore } from './tag-store';
import { SharedModule } from '../shared/shared.module';

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
    HttpClientModule,
    RouterModule,
    CoreModule,
    SharedModule
  ],
  providers,
  entryComponents: [
    AddTagOverlayComponent
  ]
})
export class TagsModule { }
