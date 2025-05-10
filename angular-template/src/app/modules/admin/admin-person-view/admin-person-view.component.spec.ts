import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPersonViewComponent } from './admin-person-view.component';

describe('AdminPersonViewComponent', () => {
  let component: AdminPersonViewComponent;
  let fixture: ComponentFixture<AdminPersonViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminPersonViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPersonViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
