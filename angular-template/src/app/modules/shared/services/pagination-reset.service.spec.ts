import { TestBed } from '@angular/core/testing';

import { PaginationResetService } from './pagination-reset.service';

describe('PaginationResetService', () => {
  let service: PaginationResetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaginationResetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
