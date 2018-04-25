import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { LoginComponent } from './login.component';
import { CoreModule } from '../core/core.module';

const declarations = [
  LoginComponent
];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,      
    CoreModule
  ]
})
export class UsersModule { }
