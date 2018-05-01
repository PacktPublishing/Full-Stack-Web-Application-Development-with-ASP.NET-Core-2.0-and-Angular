import { SettingsPageComponent } from './settings-page.component';
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { CommonModule } from '@angular/common';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';

let fixture: ComponentFixture<SettingsPageComponent>;
let component: SettingsPageComponent;

describe('SettingsPageComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SettingsPageComponent],
      imports: [CommonModule, CoreModule, SharedModule]
    }).compileComponents();

    fixture = TestBed.createComponent(SettingsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
