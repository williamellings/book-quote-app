import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BookService } from '../../../services/book.service';
import { AuthService } from '../../../services/auth.service';
import { Book } from '../../../models/book.model';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  private bookService = inject(BookService);
  public authService = inject(AuthService);

  public books = signal<Book[]>([]);
  public isLoading = signal(true);
  public errorMessage = signal<string | null>(null);
  public successMessage = signal<string | null>(null);
  public searchTerm = '';

  // Book to delete modal state
  public bookToDelete = signal<Book | null>(null);
  public isDeleting = signal(false);

  ngOnInit(): void {
    this.loadBooks();
  }

  public loadBooks(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.bookService.getBooks(this.searchTerm).subscribe({
      next: (data) => {
        this.books.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.error?.message || 'Kunde inte ladda böcker. Kontrollera att API:et är igång.');
      }
    });
  }

  public onSearch(): void {
    this.loadBooks();
  }

  public clearSearch(): void {
    this.searchTerm = '';
    this.loadBooks();
  }

  public openDeleteModal(book: Book): void {
    this.bookToDelete.set(book);
  }

  public cancelDelete(): void {
    this.bookToDelete.set(null);
  }

  public confirmDelete(): void {
    const book = this.bookToDelete();
    if (!book) return;

    this.isDeleting.set(true);
    this.bookService.deleteBook(book.id).subscribe({
      next: () => {
        this.isDeleting.set(false);
        this.books.update((list) => list.filter((b) => b.id !== book.id));
        this.bookToDelete.set(null);
        this.successMessage.set(`Boken "${book.title}" har tagits bort.`);
        setTimeout(() => this.successMessage.set(null), 4000);
      },
      error: (err) => {
        this.isDeleting.set(false);
        this.errorMessage.set(err.error?.message || 'Kunde inte ta bort boken.');
      }
    });
  }
}
