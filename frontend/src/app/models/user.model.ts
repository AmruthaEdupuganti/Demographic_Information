export interface User extends UserUpdateRequest {
  id: number;
  createdAt: string;
  updatedAt?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest extends UserUpdateRequest {
  password: string;
}

export interface UserUpdateRequest {
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth?: string;
  phone?: string;
  street?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}
