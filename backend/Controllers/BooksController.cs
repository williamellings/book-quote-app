using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookQuoteApp.Api.Data;
using BookQuoteApp.Api.DTOs;
using BookQuoteApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookQuoteApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out var id) ? id : null;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] string? search)
        {
            var query = _context.Books
                .Include(b => b.User)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(b => b.Title.ToLower().Contains(term) ||
                                         b.Author.ToLower().Contains(term) ||
                                         (b.Genre != null && b.Genre.ToLower().Contains(term)));
            }

            var books = await query
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    PublishedDate = b.PublishedDate,
                    Description = b.Description,
                    Genre = b.Genre,
                    CreatedAt = b.CreatedAt,
                    UserId = b.UserId,
                    AddedByUsername = b.User != null ? b.User.Username : "System"
                })
                .ToListAsync();

            return Ok(books);
        }

        // GET: api/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound(new { message = $"Boken med ID {id} hittades inte." });
            }

            return Ok(new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedDate = book.PublishedDate,
                Description = book.Description,
                Genre = book.Genre,
                CreatedAt = book.CreatedAt,
                UserId = book.UserId,
                AddedByUsername = book.User != null ? book.User.Username : "System"
            });
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();

            var book = new Book
            {
                Title = dto.Title.Trim(),
                Author = dto.Author.Trim(),
                PublishedDate = dto.PublishedDate,
                Description = dto.Description?.Trim(),
                Genre = dto.Genre?.Trim(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var username = User.FindFirstValue(ClaimTypes.Name);

            var createdDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedDate = book.PublishedDate,
                Description = book.Description,
                Genre = book.Genre,
                CreatedAt = book.CreatedAt,
                UserId = book.UserId,
                AddedByUsername = username ?? "Användare"
            };

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, createdDto);
        }

        // PUT: api/books/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> UpdateBook(int id, [FromBody] UpdateBookDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _context.Books.Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new { message = $"Boken med ID {id} hittades inte." });
            }

            book.Title = dto.Title.Trim();
            book.Author = dto.Author.Trim();
            book.PublishedDate = dto.PublishedDate;
            book.Description = dto.Description?.Trim();
            book.Genre = dto.Genre?.Trim();

            await _context.SaveChangesAsync();

            return Ok(new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedDate = book.PublishedDate,
                Description = book.Description,
                Genre = book.Genre,
                CreatedAt = book.CreatedAt,
                UserId = book.UserId,
                AddedByUsername = book.User != null ? book.User.Username : "System"
            });
        }

        // DELETE: api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = $"Boken med ID {id} hittades inte." });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Boken har raderats framgångsrikt." });
        }
    }
}
