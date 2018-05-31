import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { Logger } from './logger.service';
import { HeaderInterceptor } from './headers.interceptor';
import { HubClient } from './hub-client';
import { HubClientGuard } from './hub-client-guard';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { LanguageService } from './language.service';
import { AuthGuard } from './auth.guard';
import { AuthService } from './auth.service';
import { LoginRedirectService } from './redirect.service';
import { ErrorService } from './error.service';
import { OverlayRefProvider } from './overlay-ref-provider';
import { UnauthorizedResponseInterceptor } from './unauthorized-response.interceptor';
import { CanDeactivateComponentGuard } from './can-deactivate-component.guard';
import { LaunchSettings } from './launch-settings';

const providers = [
  {
    provide: HTTP_INTERCEPTORS,
    useClass: HeaderInterceptor,
    multi: true
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: UnauthorizedResponseInterceptor,
    multi: true
  },

  AuthGuard,
  AuthService,
  CanDeactivateComponentGuard,
  ErrorService,
  HubClient,
  HubClientGuard,
  LanguageService,
  LaunchSettings,
  LocalStorageService,
  LoginRedirectService,
  Logger,
  OverlayRefProvider
];

export function TranslateHttpLoaderFactory(httpClient: HttpClient) {
  return new TranslateHttpLoader(httpClient);
}

@NgModule({
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
  exports: [TranslateModule]
})
export class CoreModule {}
