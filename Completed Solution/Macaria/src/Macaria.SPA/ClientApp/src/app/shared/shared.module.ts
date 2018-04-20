import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { LoggerService } from './logger.service';
import { QuillTextEditorComponent } from './quill-text-editor.component';
import { PrimaryHeaderComponent } from './primary-header.component';
import { SideNavComponent } from "./side-nav.component";
import { SideNavItemComponent } from './side-nav-item.component';
import { SideNavSectionComponent } from './side-nav-section.component';
import { HeaderInterceptor } from './headers.interceptor';
import { HubClient } from './hub-client';
import { NotificationComponent } from './notification.component';
import { Notifications } from './notifications';
import { HubClientGuard } from './hub-client-guard';

const declarations = [
  NotificationComponent,
  PrimaryHeaderComponent,
  QuillTextEditorComponent,
  SideNavComponent,
  SideNavItemComponent,
  SideNavSectionComponent
];

const providers = [
  {
    provide: HTTP_INTERCEPTORS,
    useClass: HeaderInterceptor,
    multi: true
  },
  HubClient,
  HubClientGuard,
  LocalStorageService,
  LoggerService,
  Notifications
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
  exports: declarations,
  entryComponents:[NotificationComponent]
})
export class SharedModule { }
