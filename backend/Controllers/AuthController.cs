using System.Security.Claims;
using BookQuoteApp.Api.Data;
using BookQuoteApp.Api.DTOs;
using BookQuoteApp.Api.Models;
using BookQuoteApp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookQuoteApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(AppDbContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trimmedUsername = dto.Username.Trim();
            var trimmedEmail = dto.Email.Trim().ToLowerInvariant();

            var usernameExists = await _context.Users.AnyAsync(u => u.Username.ToLower() == trimmedUsername.ToLower());
            if (usernameExists)
            {
                return BadRequest(new { message = "Användarnamnet är redan upptaget." });
            }

            var emailExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == trimmedEmail);
            if (emailExists)
            {
                return BadRequest(new { message = "E-postadressen är redan registrerad." });
            }

            var user = new User
            {
                Username = trimmedUsername,
                Email = trimmedEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var (token, expiresAt) = _jwtTokenService.GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                ExpiresAt = expiresAt
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginInput = dto.Username.Trim();

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username.ToLower() == loginInput.ToLower() || u.Email.ToLower() == loginInput.ToLower());

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Felaktigt användarnamn/e-post eller lösenord." });
            }

            var (token, expiresAt) = _jwtTokenService.GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                ExpiresAt = expiresAt
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserProfileDto>> GetCurrentUser()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Användare hittades inte." });
            }

            return Ok(new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            });
        }
    }
}
