import { Component, OnInit, signal, computed } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { User, UserUpdateRequest } from '../../models/user.model';
import { PersonalDetails } from '../shared/personal-details/personal-details';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [DatePipe, ReactiveFormsModule, PersonalDetails],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  users = signal<User[]>([]);
  loading = signal(false);
  error = signal('');
  editingUser = signal<User | null>(null);
  showForm = signal(false);
  importLoading = signal(false);
  toasts = signal<{ id: number; message: string; type: string }[]>([]);

  usersWithPhone = computed(() => this.users().filter(u => !!u.phone).length);
  usersWithAddress = computed(() => this.users().filter(u => !!u.city || !!u.state || !!u.street).length);

  private toastId = 0;
  currentUserName = '';

  userForm: FormGroup;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.currentUserName = this.authService.getCurrentUserName() || 'Agent';
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.minLength(8)]],
      dateOfBirth: ['', [Validators.required]],
      phone: [''],
      street: [''],
      city: [''],
      state: [''],
      zipCode: [''],
      country: ['']
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  showToast(message: string, type: 'success' | 'error' | 'info' = 'success'): void {
    const id = ++this.toastId;
    this.toasts.update(t => [...t, { id, message, type }]);
    setTimeout(() => {
      this.toasts.update(t => t.filter(toast => toast.id !== id));
    }, 3000);
  }

  loadUsers(): void {
    this.loading.set(true);
    this.userService.getAll().subscribe({
      next: (users) => {
        this.users.set(users);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load policyholders');
        this.loading.set(false);
      }
    });
  }

  openCreateForm(): void {
    this.editingUser.set(null);
    this.userForm.reset();
    this.userForm.get('password')?.setValidators([Validators.required, Validators.minLength(8)]);
    this.userForm.get('password')?.updateValueAndValidity();
    this.showForm.set(true);
  }

  openEditForm(user: User): void {
    this.editingUser.set(user);
    this.userForm.patchValue({
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
      dateOfBirth: user.dateOfBirth ? user.dateOfBirth.split('T')[0] : '',
      phone: user.phone || '',
      street: user.street || '',
      city: user.city || '',
      state: user.state || '',
      zipCode: user.zipCode || '',
      country: user.country || ''
    });
    this.userForm.get('password')?.clearValidators();
    this.userForm.get('password')?.updateValueAndValidity();
    this.showForm.set(true);
  }

  cancelForm(): void {
    this.showForm.set(false);
    this.editingUser.set(null);
    this.userForm.reset();
  }

  onSubmit(): void {
    if (this.userForm.invalid) return;

    const editing = this.editingUser();

    if (editing) {
      const updateData: UserUpdateRequest = {
        firstName: this.userForm.value.firstName,
        lastName: this.userForm.value.lastName,
        email: this.userForm.value.email,
        dateOfBirth: this.userForm.value.dateOfBirth || undefined,
        phone: this.userForm.value.phone || undefined,
        street: this.userForm.value.street || undefined,
        city: this.userForm.value.city || undefined,
        state: this.userForm.value.state || undefined,
        zipCode: this.userForm.value.zipCode || undefined,
        country: this.userForm.value.country || undefined
      };

      this.userService.update(editing.id, updateData).subscribe({
        next: () => {
          this.cancelForm();
          this.showToast('Policyholder updated successfully');
          this.loadUsers();
        },
        error: () => this.showToast('Failed to update policyholder', 'error')
      });
    } else {
      const createData = {
        ...this.userForm.value,
        dateOfBirth: this.userForm.value.dateOfBirth || undefined
      };

      this.userService.create(createData).subscribe({
        next: () => {
          this.cancelForm();
          this.showToast('Policyholder created successfully');
          this.loadUsers();
        },
        error: () => this.showToast('Failed to create policyholder', 'error')
      });
    }
  }

  deleteUser(user: User): void {
    if (confirm(`Are you sure you want to remove ${user.firstName} ${user.lastName}?`)) {
      this.userService.delete(user.id).subscribe({
        next: () => {
          this.showToast('Policyholder removed');
          this.loadUsers();
        },
        error: () => this.showToast('Failed to remove policyholder', 'error')
      });
    }
  }

  importRandomUser(): void {
    this.importLoading.set(true);
    this.userService.importFromRandomUser().subscribe({
      next: (user) => {
        this.importLoading.set(false);
        this.showToast(`Imported policyholder ${user.firstName} ${user.lastName}`);
        this.loadUsers();
      },
      error: () => {
        this.showToast('Failed to import policyholder', 'error');
        this.importLoading.set(false);
      }
    });
  }

  formatLocation(user: User): string {
    return [user.city, user.state, user.country].filter(v => !!v).join(', ');
  }

  downloadDocument(user: User): void {
    this.userService.downloadDocument(user.id);
  }

  exportAllRecords(): void {
    this.userService.exportAll();
  }
}
