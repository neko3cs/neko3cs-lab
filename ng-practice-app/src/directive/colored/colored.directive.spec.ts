import { ColoredDirective } from './colored.directive';
import { ElementRef } from '@angular/core';

describe('ColoredDirective', () => {
  it('should create an instance', () => {
    let el: ElementRef;
    const directive = new ColoredDirective(el);
    expect(directive).toBeTruthy();
  });
});
