import { Directive, Input, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appColored]'
})
export class ColoredDirective {
  @Input('appColored') color = '#ffc';

  constructor(private el: ElementRef) { }

  @HostListener('mouseenter') onmouseenter() {
    this.el.nativeElement.style.backgroundColor = this.color;
  }

  @HostListener('mouseleave') onmouseleave() {
    this.el.nativeElement.style.backgroundColor = '';
  }

}
