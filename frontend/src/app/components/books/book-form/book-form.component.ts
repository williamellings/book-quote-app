import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BookService } from '../../../services/book.service';

@Component({
  selector: 'app-book-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.css']
})
export class BookFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private bookService = inject(BookService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  public bookForm: FormGroup = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    author: ['', [Validators.required, Validators.maxLength(100)]],
    publishedDate: [''],
    genre: [''],
    description: ['', [Validators.maxLength(1000)]]
  });

  public isEditMode = signal(false);
  public bookId = signal<number | null>(null);
  public isLoading = signal(false);
  public isSubmitting = signal(false);
  public errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = parseInt(idParam, 10);
      if (!isNaN(id)) {
        this.isEditMode.set(true);
        this.bookId.set(id);
        this.loadBook(id);
      }
    }
  }

  private loadBook(id: number): void {
    this.isLoading.set(true);
    this.bookService.getBook(id).subscribe({
      next: (book) => {
        this.isLoading.set(false);
        let formattedDate = '';
        if (book.publishedDate) {
          formattedDate = new Date(book.publishedDate).toISOString().split('T')[0];
        }
        this.bookForm.patchValue({
          title: book.title,
          author: book.author,
          publishedDate: formattedDate,
          genre: book.genre || '',
          description: book.description || ''
        });
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.error?.message || 'Kunde inte hämta bokens information.');
      }
    });
  }

  public onSubmit(): void {
    if (this.bookForm.invalid) {
      this.bookForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const formValues = this.bookForm.value;
    const bookPayload = {
      title: formValues.title,
      author: formValues.author,
      publishedDate: formValues.publishedDate ? new Date(formValues.publishedDate).toISOString() : null,
      genre: formValues.genre,
      description: formValues.description
    };

    if (this.isEditMode() && this.bookId()) {
      this.bookService.updateBook(this.bookId()!, bookPayload).subscribe({
        next: () => {
          this.isSubmitting.set(false);
          // Redirect back to home/books list as required
          this.router.navigate(['/books']);
        },
        error: (err) => {
          this.isSubmitting.set(false);
          this.errorMessage.set(err.error?.message || 'Kunde inte uppdatera boken.');
        }
      });
    } else {
      this.bookService.createBook(bookPayload).subscribe({
        next: () => {
          this.isSubmitting.set(false);
          // Redirect back to home/books list as required
          this.router.navigate(['/books']);
        },
        error: (err) => {
          this.isSubmitting.set(false);
          this.errorMessage.set(err.error?.message || 'Kunde inte spara den nya boken.');
        }
      });
    }
  }
}
