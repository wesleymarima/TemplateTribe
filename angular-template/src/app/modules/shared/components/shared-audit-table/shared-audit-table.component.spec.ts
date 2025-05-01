import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedAuditTableComponent } from './shared-audit-table.component';

describe('SharedAuditTableComponent', () => {
  let component: SharedAuditTableComponent;
  let fixture: ComponentFixture<SharedAuditTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SharedAuditTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SharedAuditTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
