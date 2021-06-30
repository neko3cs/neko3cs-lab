import { Directive, Input, TemplateRef, ViewContainerRef, OnChanges } from '@angular/core';

@Directive({
  selector: '[appDeadline]'
})
export class DeadlineDirective implements OnChanges {
  @Input('appDeadline') deadline: Date;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef
  ) { }

  ngOnChanges(changes: import("@angular/core").SimpleChanges): void {
    let now = new Date();
    if (this.deadline.getTime() < now.getTime()) {
      this.viewContainer.clear();
    } else {
      this.viewContainer.createEmbeddedView(this.templateRef)
    }
  }

}
