using System;
using System.ComponentModel.DataAnnotations;

namespace BookQuoteApp.Api.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime? PublishedDate { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UserId { get; set; }
        public string? AddedByUsername { get; set; }
    }

    public class CreateBookDto
    {
        [Required(ErrorMessage = "Boktitel krävs")]
        [StringLength(200, ErrorMessage = "Titeln kan inte vara längre än 200 tecken")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Författare krävs")]
        [StringLength(100, ErrorMessage = "Författarnamnet kan inte vara längre än 100 tecken")]
        public string Author { get; set; } = string.Empty;

        public DateTime? PublishedDate { get; set; }

        public string? Description { get; set; }

        public string? Genre { get; set; }
    }

    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Boktitel krävs")]
        [StringLength(200, ErrorMessage = "Titeln kan inte vara längre än 200 tecken")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Författare krävs")]
        [StringLength(100, ErrorMessage = "Författarnamnet kan inte vara längre än 100 tecken")]
        public string Author { get; set; } = string.Empty;

        public DateTime? PublishedDate { get; set; }

        public string? Description { get; set; }

        public string? Genre { get; set; }
    }
}
