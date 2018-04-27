import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingsPageComponent } from './settings-page.component';
import { CoreModule } from '../core/core.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';

const declarations = [SettingsPageComponent];

@NgModule({
  imports: [CommonModule, CoreModule, SharedModule],
  declarations
})
export class SettingsModule {}
