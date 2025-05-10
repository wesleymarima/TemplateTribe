import { TestBed } from '@angular/core/testing';

import { BaseSharedService } from './base.shared.service';

describe('BaseSharedService', () => {
  let service: BaseSharedService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BaseSharedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
