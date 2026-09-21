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
    public class QuotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuotesController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out var id) ? id : null;
        }

        // GET: api/quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuoteDto>>> GetQuotes([FromQuery] string? category)
        {
            var query = _context.Quotes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(q => q.Category != null && q.Category.ToLower() == category.Trim().ToLower());
            }

            var quotes = await query
                .OrderBy(q => q.Id)
                .Select(q => new QuoteDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Author = q.Author,
                    Category = q.Category,
                    CreatedAt = q.CreatedAt,
                    UserId = q.UserId,
                    IsCustom = q.UserId != null
                })
                .ToListAsync();

            return Ok(quotes);
        }

        // GET: api/quotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuoteDto>> GetQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound(new { message = $"Citatet med ID {id} hittades inte." });
            }

            return Ok(new QuoteDto
            {
                Id = quote.Id,
                Text = quote.Text,
                Author = quote.Author,
                Category = quote.Category,
                CreatedAt = quote.CreatedAt,
                UserId = quote.UserId,
                IsCustom = quote.UserId != null
            });
        }

        // POST: api/quotes
        [HttpPost]
        public async Task<ActionResult<QuoteDto>> CreateQuote([FromBody] CreateQuoteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();

            var quote = new Quote
            {
                Text = dto.Text.Trim(),
                Author = dto.Author.Trim(),
                Category = dto.Category?.Trim(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            var resultDto = new QuoteDto
            {
                Id = quote.Id,
                Text = quote.Text,
                Author = quote.Author,
                Category = quote.Category,
                CreatedAt = quote.CreatedAt,
                UserId = quote.UserId,
                IsCustom = quote.UserId != null
            };

            return CreatedAtAction(nameof(GetQuote), new { id = quote.Id }, resultDto);
        }

        // PUT: api/quotes/5
        [HttpPut("{id}")]
        public async Task<ActionResult<QuoteDto>> UpdateQuote(int id, [FromBody] UpdateQuoteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound(new { message = $"Citatet med ID {id} hittades inte." });
            }

            quote.Text = dto.Text.Trim();
            quote.Author = dto.Author.Trim();
            quote.Category = dto.Category?.Trim();

            await _context.SaveChangesAsync();

            return Ok(new QuoteDto
            {
                Id = quote.Id,
                Text = quote.Text,
                Author = quote.Author,
                Category = quote.Category,
                CreatedAt = quote.CreatedAt,
                UserId = quote.UserId,
                IsCustom = quote.UserId != null
            });
        }

        // DELETE: api/quotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound(new { message = $"Citatet med ID {id} hittades inte." });
            }

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Citatet har raderats framgångsrikt." });
        }
    }
}
