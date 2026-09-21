import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { BookListComponent } from './components/books/book-list/book-list.component';
import { BookFormComponent } from './components/books/book-form/book-form.component';
import { QuotesComponent } from './components/quotes/quotes.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'books'
  },
  {
    path: 'books',
    component: BookListComponent,
    canActivate: [authGuard]
  },
  {
    path: 'books/new',
    component: BookFormComponent,
    canActivate: [authGuard]
  },
  {
    path: 'books/edit/:id',
    component: BookFormComponent,
    canActivate: [authGuard]
  },
  {
    path: 'quotes',
    component: QuotesComponent,
    canActivate: [authGuard]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: '**',
    redirectTo: 'books'
  }
];
