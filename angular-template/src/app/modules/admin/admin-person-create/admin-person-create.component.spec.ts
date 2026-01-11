import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPersonCreateComponent } from './admin-person-create.component';

describe('AdminPersonCreateComponent', () => {
  let component: AdminPersonCreateComponent;
  let fixture: ComponentFixture<AdminPersonCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminPersonCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminPersonCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
