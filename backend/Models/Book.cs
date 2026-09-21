using System;

namespace BookQuoteApp.Api.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime? PublishedDate { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relation to User who added/owns the book (nullable if public or system-seeded)
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
