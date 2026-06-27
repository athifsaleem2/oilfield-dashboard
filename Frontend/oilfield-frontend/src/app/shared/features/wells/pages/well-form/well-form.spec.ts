import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellForm } from './well-form';

describe('WellForm', () => {
  let component: WellForm;
  let fixture: ComponentFixture<WellForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WellForm],
    }).compileComponents();

    fixture = TestBed.createComponent(WellForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
