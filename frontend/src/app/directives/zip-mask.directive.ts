import { Directive, ElementRef, HostListener } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appZipMask]',
  standalone: true
})
export class ZipMaskDirective {
  constructor(private el: ElementRef<HTMLInputElement>, private control: NgControl) {}

  @HostListener('input')
  onInput(): void {
    const digits = this.el.nativeElement.value.replace(/\D/g, '').slice(0, 9);
    let masked = digits.slice(0, 5);

    if (digits.length > 5) masked += '-' + digits.slice(5, 9);

    this.el.nativeElement.value = masked;
    this.control.control?.setValue(masked, { emitEvent: false });
  }
}
