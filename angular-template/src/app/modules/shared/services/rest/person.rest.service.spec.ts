import { TestBed } from '@angular/core/testing';

import { PersonRestService } from './person.rest.service';

describe('PersonRestService', () => {
  let service: PersonRestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PersonRestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
