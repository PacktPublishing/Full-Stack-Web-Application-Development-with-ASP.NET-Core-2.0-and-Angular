import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { constants } from './shared/constants';
import { AppRoutingModule } from './app-routing.module';
import { NotesModule } from './notes/notes.module';
import { MaterialModule } from './material/material.module';
import { SharedModule } from './shared/shared.module';
import { TenantsModule } from './tenants/tenants.module';
import { UsersModule } from './users/users.module';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { MasterPageComponent } from './master-page.component';
import { AgGridModule } from "ag-grid-angular";
import { TagsModule } from './tags/tags.module';
import { TagsPageComponent } from './tags/tags-page.component';
import { SettingsModule } from './settings/settings.module';

@NgModule({
  declarations: [
    AppComponent,
    AnonymousMasterPageComponent,
    MasterPageComponent    
  ],
  imports: [
    AgGridModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    
    AppRoutingModule,

    MaterialModule,
    NotesModule,
    SharedModule,
    SettingsModule,
    TagsModule,
    TenantsModule,
    UsersModule
  ],
  providers: [
    { provide: constants.BASE_URL, useValue: "http://localhost:10372/" }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
