import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedLoadingSpinnerComponent } from './shared-loading-spinner.component';

describe('SharedLoadingSpinnerComponent', () => {
  let component: SharedLoadingSpinnerComponent;
  let fixture: ComponentFixture<SharedLoadingSpinnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SharedLoadingSpinnerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SharedLoadingSpinnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
