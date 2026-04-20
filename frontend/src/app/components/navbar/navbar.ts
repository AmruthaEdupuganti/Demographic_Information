import { Component, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  mobileMenuOpen = signal(false);

  constructor(
    public authService: AuthService,
    private router: Router
  ) {}


  logout(): void {
    this.authService.logout();
    this.mobileMenuOpen.set(false);
    this.router.navigate(['/login']);
  }
}
