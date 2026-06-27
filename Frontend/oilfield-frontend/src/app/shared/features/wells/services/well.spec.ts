import { TestBed } from '@angular/core/testing';

import { Well } from './well';

describe('Well', () => {
  let service: Well;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Well);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
