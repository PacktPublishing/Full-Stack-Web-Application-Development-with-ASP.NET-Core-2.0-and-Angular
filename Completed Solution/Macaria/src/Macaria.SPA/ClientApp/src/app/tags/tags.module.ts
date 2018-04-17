import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../material/material.module';
import { TagManagementComponent } from './tag-management.component';
import { TagsService } from './tags.service';

const declarations = [
  TagManagementComponent
];

const providers = [
  TagsService
];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,
    
    MaterialModule,
    SharedModule
  ],
  providers,
})
export class TagsModule { }
