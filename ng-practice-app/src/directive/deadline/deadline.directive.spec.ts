import { DeadlineDirective } from './deadline.directive';
import { TemplateRef, ViewContainerRef } from '@angular/core';

describe('DeadlineDirective', () => {
  it('should create an instance', () => {
    let templateRef: TemplateRef<any>;
    let viewContainer: ViewContainerRef;
    const directive = new DeadlineDirective(templateRef, viewContainer);
    expect(directive).toBeTruthy();
  });
});
