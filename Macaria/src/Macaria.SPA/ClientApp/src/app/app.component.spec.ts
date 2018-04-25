//import { async, ComponentFixture, TestBed } from '@angular/core/testing';
//import { AppComponent } from './app.component';
//import { AgGridModule } from 'ag-grid-angular';
//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
//import { BrowserModule } from '@angular/platform-browser';
//import { HttpClientModule } from '@angular/common/http';
//import { AppRoutingModule } from './app-routing.module';
//import { MaterialModule } from './material/material.module';
//import { NotesModule } from './notes';
//import { FormsModule } from '@angular/forms';
//import { SharedModule } from './core/core.module';
//import { SettingsModule } from './settings/settings.module';
//import { TenantsModule } from './tenants/tenants.module';
//import { UsersModule } from './users/users.module';
//import { TagsModule } from './tags/tags.module';
//import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
//import { MasterPageComponent } from './master-page.component';

//describe('AppComponent', () => {
//  let component: AppComponent;
//  let fixture: ComponentFixture<AppComponent>;

//  beforeEach(async(() => {
//    TestBed.configureTestingModule({
//      imports: [
//        AgGridModule,
//        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
//        BrowserAnimationsModule,
//        HttpClientModule,
//        FormsModule,

//        AppRoutingModule,

//        MaterialModule,
//        NotesModule,
//        SharedModule,
//        SettingsModule,
//        TagsModule,
//        TenantsModule,
//        UsersModule
//      ],
//      declarations: [
//        AppComponent,
//        AnonymousMasterPageComponent,
//        MasterPageComponent
//      ]
//    })
//      .compileComponents();
//  }));

//  beforeEach(() => {
//    fixture = TestBed.createComponent(AppComponent);
//    component = fixture.componentInstance;
//    fixture.detectChanges();
//  });

//  it('should display a title', async(() => {
//    //const titleText = fixture.nativeElement.querySelector('h1').textContent;
//    expect(1).toEqual(1);
//  }));

//  //it('should start with count 0, then increments by 1 when clicked', async(() => {
//  //  const countElement = fixture.nativeElement.querySelector('strong');
//  //  expect(countElement.textContent).toEqual('0');

//  //  const incrementButton = fixture.nativeElement.querySelector('button');
//  //  incrementButton.click();
//  //  fixture.detectChanges();
//  //  expect(countElement.textContent).toEqual('1');
//  //}));
//});
