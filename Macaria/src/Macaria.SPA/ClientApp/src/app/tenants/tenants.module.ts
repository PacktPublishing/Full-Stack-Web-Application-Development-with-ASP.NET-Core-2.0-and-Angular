import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../material/material.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SetTenantComponent } from './set-tenant.component';
import { TenantGuard } from './tenant.guard';
import { TenantsService } from './tenants.service';
import { RedirectService } from './redirect.service';

const declarations = [
  SetTenantComponent,
];

const providers = [
  RedirectService,
  TenantGuard,
  TenantsService
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
export class TenantsModule { }
