export interface Quote {
  id: number;
  text: string;
  author: string;
  category?: string | null;
  createdAt: string;
  userId?: number | null;
  isCustom: boolean;
}

export interface CreateQuoteRequest {
  text: string;
  author: string;
  category?: string | null;
}

export interface UpdateQuoteRequest {
  text: string;
  author: string;
  category?: string | null;
}
