import { Component, Input, OnInit } from '@angular/core';
import { PhoneMaskDirective } from '../../../directives/phone-mask.directive';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ZipMaskDirective } from '../../../directives/zip-mask.directive';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'personal-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PhoneMaskDirective, ZipMaskDirective],
  templateUrl: './personal-details.html',
  styleUrl: './personal-details.css',
})
export class PersonalDetails implements OnInit {

  @Input() personalFormDetails!: FormGroup;
  @Input() showPasswordField = false;

  ngOnInit(): void {
    console.log('PersonalDetails component initialized with form:', this.personalFormDetails);
  }

}
