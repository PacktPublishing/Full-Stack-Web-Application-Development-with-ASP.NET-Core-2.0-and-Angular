import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { baseUrl } from './core/constants';
import { AppRoutingModule } from './app-routing.module';
import { NotesModule } from './notes/notes.module';
import { CoreModule } from './core/core.module';
import { UsersModule } from './users/users.module';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { MasterPageComponent } from './master-page.component';
import { AgGridModule } from "ag-grid-angular";
import { TagsModule } from './tags/tags.module';
import { TagsPageComponent } from './tags/tags-page.component';
import { SettingsModule } from './settings/settings.module';
import { SharedModule } from './shared/shared.module';

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
    
    AppRoutingModule,

    CoreModule,
    NotesModule,
    SettingsModule,
    SharedModule,
    TagsModule,
    UsersModule
  ],
  providers: [
    { provide: baseUrl, useValue: "http://localhost:54193/" }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
