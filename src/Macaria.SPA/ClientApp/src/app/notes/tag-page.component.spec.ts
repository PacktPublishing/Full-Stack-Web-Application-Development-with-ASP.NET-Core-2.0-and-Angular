import { TagPageComponent } from "./tag-page.component";
import { TestBed, ComponentFixture } from "@angular/core/testing";

let fixture: ComponentFixture<TagPageComponent>;
let component: TagPageComponent;

describe('TagPageComponent', () => {

  beforeEach(() => {

    TestBed.configureTestingModule({
      declarations: [TagPageComponent]
    });

    fixture = TestBed.createComponent(TagPageComponent);
    component = fixture.componentInstance;
  });

  it('should be created without errors', () => {
    expect(component).toBeDefined();
  });

});
