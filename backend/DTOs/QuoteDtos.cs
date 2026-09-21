using System;
using System.ComponentModel.DataAnnotations;

namespace BookQuoteApp.Api.DTOs
{
    public class QuoteDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UserId { get; set; }
        public bool IsCustom { get; set; }
    }

    public class CreateQuoteDto
    {
        [Required(ErrorMessage = "Citattext krävs")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Citatet måste vara mellan 3 och 1000 tecken")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Författare / Källa krävs")]
        [StringLength(100, ErrorMessage = "Författarnamnet kan inte vara längre än 100 tecken")]
        public string Author { get; set; } = string.Empty;

        public string? Category { get; set; }
    }

    public class UpdateQuoteDto
    {
        [Required(ErrorMessage = "Citattext krävs")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Citatet måste vara mellan 3 och 1000 tecken")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Författare / Källa krävs")]
        [StringLength(100, ErrorMessage = "Författarnamnet kan inte vara längre än 100 tecken")]
        public string Author { get; set; } = string.Empty;

        public string? Category { get; set; }
    }
}
