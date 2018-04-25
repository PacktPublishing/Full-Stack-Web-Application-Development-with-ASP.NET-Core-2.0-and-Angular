import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from './auth.service';
import { AuthGuard } from './auth.guard';
import { LoginComponent } from './login.component';
import { CoreModule } from '../core/core.module';

import { HttpClientModule } from '@angular/common/http';
import { LoginRedirectService } from './redirect.service';
import { JwtInterceptor } from './jwt.interceptor';
import { SharedModule } from '../shared/shared.module';

const declarations = [
  LoginComponent
];

const providers = [
  AuthGuard,
  AuthService,
  LoginRedirectService
];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule,
    
    CoreModule,
    SharedModule
  ],
  providers,
})
export class UsersModule { }
