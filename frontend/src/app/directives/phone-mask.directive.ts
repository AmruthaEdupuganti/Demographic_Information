import { Directive, ElementRef, HostListener } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appPhoneMask]',
  standalone: true
})
export class PhoneMaskDirective {
  constructor(private el: ElementRef<HTMLInputElement>, private control: NgControl) {}

  @HostListener('input')
  onInput(): void {
    const digits = this.el.nativeElement.value.replace(/\D/g, '').slice(0, 10);
    let masked = '';

    if (digits.length > 0) masked = '(' + digits.slice(0, 3);
    if (digits.length >= 3) masked += ') ';
    if (digits.length > 3) masked += digits.slice(3, 6);
    if (digits.length >= 6) masked += '-';
    if (digits.length > 6) masked += digits.slice(6, 10);

    this.el.nativeElement.value = masked;
    this.control.control?.setValue(masked, { emitEvent: false });
  }
}
