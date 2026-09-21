import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  public authService = inject(AuthService);
  public themeService = inject(ThemeService);

  public isMenuCollapsed = true;

  public toggleMenu(): void {
    this.isMenuCollapsed = !this.isMenuCollapsed;
  }

  public closeMenu(): void {
    this.isMenuCollapsed = true;
  }

  public onLogout(): void {
    this.closeMenu();
    this.authService.logout();
  }
}
