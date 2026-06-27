import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellList } from './well-list';

describe('WellList', () => {
  let component: WellList;
  let fixture: ComponentFixture<WellList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WellList],
    }).compileComponents();

    fixture = TestBed.createComponent(WellList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
