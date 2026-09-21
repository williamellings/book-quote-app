import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { QuoteService } from '../../services/quote.service';
import { AuthService } from '../../services/auth.service';
import { Quote } from '../../models/quote.model';

@Component({
  selector: 'app-quotes',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './quotes.component.html',
  styleUrls: ['./quotes.component.css']
})
export class QuotesComponent implements OnInit {
  private quoteService = inject(QuoteService);
  public authService = inject(AuthService);
  private fb = inject(FormBuilder);

  public quotes = signal<Quote[]>([]);
  public isLoading = signal(true);
  public isSubmitting = signal(false);
  public isDeleting = signal(false);
  public errorMessage = signal<string | null>(null);
  public successMessage = signal<string | null>(null);
  public selectedCategory = '';

  // Form modal state
  public isModalOpen = signal(false);
  public isEditMode = signal(false);
  public currentQuoteId = signal<number | null>(null);
  public quoteToDelete = signal<Quote | null>(null);

  public quoteForm: FormGroup = this.fb.group({
    text: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(1000)]],
    author: ['', [Validators.required, Validators.maxLength(100)]],
    category: ['']
  });

  ngOnInit(): void {
    this.loadQuotes();
  }

  public loadQuotes(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.quoteService.getQuotes(this.selectedCategory).subscribe({
      next: (data) => {
        this.quotes.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(err.error?.message || 'Kunde inte ladda citat.');
      }
    });
  }

  public onCategoryChange(): void {
    this.loadQuotes();
  }

  public openAddModal(): void {
    this.isEditMode.set(false);
    this.currentQuoteId.set(null);
    this.quoteForm.reset({
      text: '',
      author: '',
      category: 'Inspiration'
    });
    this.isModalOpen.set(true);
  }

  public openEditModal(quote: Quote): void {
    this.isEditMode.set(true);
    this.currentQuoteId.set(quote.id);
    this.quoteForm.patchValue({
      text: quote.text,
      author: quote.author,
      category: quote.category || ''
    });
    this.isModalOpen.set(true);
  }

  public closeModal(): void {
    this.isModalOpen.set(false);
    this.quoteForm.reset();
  }

  public onSubmitQuote(): void {
    if (this.quoteForm.invalid) {
      this.quoteForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const formValues = this.quoteForm.value;
    const payload = {
      text: formValues.text.trim(),
      author: formValues.author.trim(),
      category: formValues.category?.trim() || null
    };

    if (this.isEditMode() && this.currentQuoteId()) {
      this.quoteService.updateQuote(this.currentQuoteId()!, payload).subscribe({
        next: (updatedQuote) => {
          this.isSubmitting.set(false);
          this.closeModal();
          this.quotes.update((list) =>
            list.map((q) => (q.id === updatedQuote.id ? updatedQuote : q))
          );
          this.showSuccess('Citatet har uppdaterats!');
        },
        error: (err) => {
          this.isSubmitting.set(false);
          this.errorMessage.set(err.error?.message || 'Kunde inte uppdatera citatet.');
        }
      });
    } else {
      this.quoteService.createQuote(payload).subscribe({
        next: (createdQuote) => {
          this.isSubmitting.set(false);
          this.closeModal();
          this.quotes.update((list) => [...list, createdQuote]);
          this.showSuccess('Nytt citat har lagts till!');
        },
        error: (err) => {
          this.isSubmitting.set(false);
          this.errorMessage.set(err.error?.message || 'Kunde inte skapa citatet.');
        }
      });
    }
  }

  public openDeleteModal(quote: Quote): void {
    this.quoteToDelete.set(quote);
  }

  public cancelDelete(): void {
    this.quoteToDelete.set(null);
  }

  public confirmDelete(): void {
    const quote = this.quoteToDelete();
    if (!quote) return;

    this.isDeleting.set(true);
    this.quoteService.deleteQuote(quote.id).subscribe({
      next: () => {
        this.isDeleting.set(false);
        this.quotes.update((list) => list.filter((q) => q.id !== quote.id));
        this.quoteToDelete.set(null);
        this.showSuccess('Citatet har raderats!');
      },
      error: (err) => {
        this.isDeleting.set(false);
        this.errorMessage.set(err.error?.message || 'Kunde inte ta bort citatet.');
      }
    });
  }

  public copyToClipboard(text: string, author: string): void {
    const formatted = `"${text}" — ${author}`;
    navigator.clipboard.writeText(formatted).then(() => {
      this.showSuccess('Citat kopierat till urklipp!');
    });
  }

  private showSuccess(msg: string): void {
    this.successMessage.set(msg);
    setTimeout(() => this.successMessage.set(null), 3500);
  }
}
