import {ComponentFixture, TestBed} from '@angular/core/testing';

import {AdminPersonsHomeComponent} from './admin-persons-home.component';

describe('AdminUsersHomeComponent', () => {
  let component: AdminPersonsHomeComponent;
  let fixture: ComponentFixture<AdminPersonsHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminPersonsHomeComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(AdminPersonsHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
