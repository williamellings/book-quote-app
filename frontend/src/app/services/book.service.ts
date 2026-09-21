import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book, CreateBookRequest, UpdateBookRequest } from '../models/book.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private readonly http = inject(HttpClient);
  private readonly API_URL = 'http://localhost:5000/api/books';

  public getBooks(search?: string): Observable<Book[]> {
    let params = new HttpParams();
    if (search && search.trim()) {
      params = params.set('search', search.trim());
    }
    return this.http.get<Book[]>(this.API_URL, { params });
  }

  public getBook(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.API_URL}/${id}`);
  }

  public createBook(book: CreateBookRequest): Observable<Book> {
    return this.http.post<Book>(this.API_URL, book);
  }

  public updateBook(id: number, book: UpdateBookRequest): Observable<Book> {
    return this.http.put<Book>(`${this.API_URL}/${id}`, book);
  }

  public deleteBook(id: number): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.API_URL}/${id}`);
  }
}
