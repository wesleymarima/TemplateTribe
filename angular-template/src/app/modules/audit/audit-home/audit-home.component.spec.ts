import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditHomeComponent } from './audit-home.component';

describe('AuditHomeComponent', () => {
  let component: AuditHomeComponent;
  let fixture: ComponentFixture<AuditHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AuditHomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuditHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
