import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, RegisterRequest, UserUpdateRequest } from '../models/user.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly apiUrl = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  create(user: RegisterRequest): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  update(id: number, user: UserUpdateRequest): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, user);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  importFromRandomUser(): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/import-random`, {});
  }

  downloadDocument(id: number): void {
    this.http.get(`${this.apiUrl}/document/${id}`, { responseType: 'blob' }).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `policyholder_${id}_demographics.csv`;
        a.click();
        window.URL.revokeObjectURL(url);
      }
    });
  }

  exportAll(): void {
    this.http.get(`${this.apiUrl}/export`, { responseType: 'blob' }).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'PolicyholderRecords.csv';
        a.click();
        window.URL.revokeObjectURL(url);
      }
    });
  }
}
