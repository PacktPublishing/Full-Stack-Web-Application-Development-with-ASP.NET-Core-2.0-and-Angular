import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from './auth.service';
import { AuthGuard } from './auth.guard';
import { LoginComponent } from './login.component';
import { TenantsModule } from '../tenants/tenants.module';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../material/material.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoginRedirectService } from './redirect.service';
import { JwtInterceptor } from './jwt.interceptor';
import { AuthInterceptor } from './auth.interceptor';

const declarations = [
  LoginComponent
];

const providers = [
  AuthGuard,
  AuthService,
  LoginRedirectService,
  {
    provide: HTTP_INTERCEPTORS,
    useClass: JwtInterceptor,
    multi: true
  }
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
    SharedModule,
    TenantsModule
  ],
  providers,
})
export class UsersModule { }
