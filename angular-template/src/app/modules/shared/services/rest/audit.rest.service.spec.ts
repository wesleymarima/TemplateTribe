import { TestBed } from '@angular/core/testing';

import { AuditRestService } from './audit.rest.service';

describe('AuditRestService', () => {
  let service: AuditRestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuditRestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
