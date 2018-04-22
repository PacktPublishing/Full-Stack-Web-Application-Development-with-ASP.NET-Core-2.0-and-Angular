import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
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
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { LanguageService } from './language.service';

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
  LanguageService,
  LocalStorageService,
  LoggerService,
  Notifications
];

export function TranslateHttpLoaderFactory(httpClient: HttpClient) {
  return new TranslateHttpLoader(httpClient);
}

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
	  ReactiveFormsModule,
    RouterModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: TranslateHttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers,
  exports: [TranslateModule,...declarations],
  entryComponents:[NotificationComponent]
})
export class SharedModule { }
