import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HatenaComponent } from './hatena.component';

describe('HatenaComponent', () => {
  let component: HatenaComponent;
  let fixture: ComponentFixture<HatenaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HatenaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HatenaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
