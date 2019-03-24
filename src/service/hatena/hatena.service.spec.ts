import { TestBed } from '@angular/core/testing';

import { HatenaService } from './hatena.service';

describe('HatenaService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HatenaService = TestBed.get(HatenaService);
    expect(service).toBeTruthy();
  });
});
