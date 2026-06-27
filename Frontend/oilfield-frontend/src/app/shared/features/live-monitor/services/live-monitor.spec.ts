import { TestBed } from '@angular/core/testing';

import { LiveMonitor } from './live-monitor';

describe('LiveMonitor', () => {
  let service: LiveMonitor;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LiveMonitor);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
