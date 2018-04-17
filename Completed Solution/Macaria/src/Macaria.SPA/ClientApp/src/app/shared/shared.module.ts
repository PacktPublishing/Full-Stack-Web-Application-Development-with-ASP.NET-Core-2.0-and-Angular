import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { LoggerService } from './logger.service';
import { TenantInterceptor } from '../tenants/tenant.interceptor';
import { QuillTextEditorComponent } from './quill-text-editor.component';
import { PrimaryHeaderComponent } from './primary-header.component';
import { SideNavComponent } from "./side-nav.component";
import { SideNavItemComponent } from './side-nav-item.component';
import { SideNavSectionComponent } from './side-nav-section.component';

const declarations = [
  PrimaryHeaderComponent,
  QuillTextEditorComponent,
  SideNavComponent,
  SideNavItemComponent,
  SideNavSectionComponent
];

const providers = [
  {
    provide: HTTP_INTERCEPTORS,
    useClass: TenantInterceptor,
    multi: true
  },
  LocalStorageService,
  LoggerService,
];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
	  ReactiveFormsModule,
	  RouterModule	
  ],
  providers,
  exports:declarations
})
export class SharedModule { }
