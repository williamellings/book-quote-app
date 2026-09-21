import { Injectable, signal, computed, inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { Observable, tap } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest, User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly platformId = inject(PLATFORM_ID);

  private readonly API_URL = 'http://localhost:5000/api/auth';
  private readonly TOKEN_KEY = 'bookquote_jwt_token';
  private readonly USER_KEY = 'bookquote_auth_user';

  public currentToken = signal<string | null>(null);
  public currentUser = signal<User | null>(null);

  public isLoggedIn = computed(() => !!this.currentToken());

  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      const token = localStorage.getItem(this.TOKEN_KEY);
      const userJson = localStorage.getItem(this.USER_KEY);

      if (token && userJson) {
        try {
          const user = JSON.parse(userJson) as User;
          this.currentToken.set(token);
          this.currentUser.set(user);
        } catch {
          this.clearStorage();
        }
      }
    }
  }

  public register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/register`, request).pipe(
      tap((res) => this.handleAuthSuccess(res))
    );
  }

  public login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/login`, request).pipe(
      tap((res) => this.handleAuthSuccess(res))
    );
  }

  public logout(): void {
    this.clearStorage();
    this.currentToken.set(null);
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  public getToken(): string | null {
    return this.currentToken();
  }

  private handleAuthSuccess(response: AuthResponse): void {
    const user: User = {
      id: response.userId,
      username: response.username,
      email: response.email,
      createdAt: new Date().toISOString()
    };

    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.TOKEN_KEY, response.token);
      localStorage.setItem(this.USER_KEY, JSON.stringify(user));
    }

    this.currentToken.set(response.token);
    this.currentUser.set(user);
  }

  private clearStorage(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.TOKEN_KEY);
      localStorage.removeItem(this.USER_KEY);
    }
  }
}
