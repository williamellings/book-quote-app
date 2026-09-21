import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Quote, CreateQuoteRequest, UpdateQuoteRequest } from '../models/quote.model';

@Injectable({
  providedIn: 'root'
})
export class QuoteService {
  private readonly http = inject(HttpClient);
  private readonly API_URL = 'http://localhost:5000/api/quotes';

  public getQuotes(category?: string): Observable<Quote[]> {
    let params = new HttpParams();
    if (category && category.trim()) {
      params = params.set('category', category.trim());
    }
    return this.http.get<Quote[]>(this.API_URL, { params });
  }

  public getQuote(id: number): Observable<Quote> {
    return this.http.get<Quote>(`${this.API_URL}/${id}`);
  }

  public createQuote(quote: CreateQuoteRequest): Observable<Quote> {
    return this.http.post<Quote>(this.API_URL, quote);
  }

  public updateQuote(id: number, quote: UpdateQuoteRequest): Observable<Quote> {
    return this.http.put<Quote>(`${this.API_URL}/${id}`, quote);
  }

  public deleteQuote(id: number): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.API_URL}/${id}`);
  }
}
