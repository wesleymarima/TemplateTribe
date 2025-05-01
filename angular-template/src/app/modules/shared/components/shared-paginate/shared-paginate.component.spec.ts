import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedPaginateComponent } from './shared-paginate.component';

describe('SharedPaginateComponent', () => {
  let component: SharedPaginateComponent;
  let fixture: ComponentFixture<SharedPaginateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SharedPaginateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SharedPaginateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
