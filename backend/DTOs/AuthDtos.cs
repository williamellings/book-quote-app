using System.ComponentModel.DataAnnotations;

namespace BookQuoteApp.Api.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Användarnamn krävs")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Användarnamnet måste vara mellan 3 och 50 tecken")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-postadress krävs")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lösenord krävs")]
        [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Användarnamn eller e-post krävs")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lösenord krävs")]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
