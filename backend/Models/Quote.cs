using System;

namespace BookQuoteApp.Api.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relation to User who created or owns this quote (null for default 5 quotes accessible to all)
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
