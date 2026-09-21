export interface Book {
  id: number;
  title: string;
  author: string;
  publishedDate?: string | null;
  description?: string | null;
  genre?: string | null;
  createdAt: string;
  userId?: number | null;
  addedByUsername?: string | null;
}

export interface CreateBookRequest {
  title: string;
  author: string;
  publishedDate?: string | null;
  description?: string | null;
  genre?: string | null;
}

export interface UpdateBookRequest {
  title: string;
  author: string;
  publishedDate?: string | null;
  description?: string | null;
  genre?: string | null;
}
