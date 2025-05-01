import { TestBed } from '@angular/core/testing';

import { PaginatedHttpService } from './paginated-http.service';

describe('PaginatedHttpService', () => {
  let service: PaginatedHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaginatedHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
