using System;
using System.Collections.Generic;
using System.Linq;
using BookQuoteApp.Api.Models;

namespace BookQuoteApp.Api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Seed default test user if none exists
            if (!context.Users.Any())
            {
                var defaultUser = new User
                {
                    Username = "demo",
                    Email = "demo@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Demo123!"),
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(defaultUser);
                context.SaveChanges();
            }

            var adminUser = context.Users.FirstOrDefault(u => u.Username == "demo");

            // Seed default books if empty
            if (!context.Books.Any())
            {
                var books = new List<Book>
                {
                    new Book
                    {
                        Title = "Kallocain",
                        Author = "Karin Boye",
                        PublishedDate = new DateTime(1940, 1, 1),
                        Genre = "Dystopi",
                        Description = "En klassisk svensk dystopisk roman om ett totalitärt övervakningssamhälle och sanningsserumet Kallocain.",
                        UserId = adminUser?.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Book
                    {
                        Title = "Aniara: En revy om människan i tid och rum",
                        Author = "Harry Martinson",
                        PublishedDate = new DateTime(1956, 1, 1),
                        Genre = "Science Fiction / Poesi",
                        Description = "Ett svenskt rymdepos om rymdskeppet Aniara som kommer ur kurs mot stjärnbilden Lyran.",
                        UserId = adminUser?.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Book
                    {
                        Title = "Pippi Långstrump",
                        Author = "Astrid Lindgren",
                        PublishedDate = new DateTime(1945, 11, 26),
                        Genre = "Barn- & Ungdomslitteratur",
                        Description = "Den tidlösa berättelsen om världens starkaste flicka som bor ensam i Villa Villekulla med sin häst och apa.",
                        UserId = adminUser?.Id,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Books.AddRange(books);
                context.SaveChanges();
            }

            // Seed 5 quotes if quotes are empty
            if (!context.Quotes.Any())
            {
                var quotes = new List<Quote>
                {
                    new Quote
                    {
                        Text = "Ett rum utan böcker är som en kropp utan själ.",
                        Author = "Marcus Tullius Cicero",
                        Category = "Filosofi",
                        UserId = adminUser?.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Quote
                    {
                        Text = "Det finns mer skatter i böcker än i allt piratbyte på Skattkammarön.",
                        Author = "Walt Disney",
                        Category = "Inspiration",
                        UserId = adminUser?.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Quote
                    {
                        Text = "En läsare lever tusen liv innan han dör. Den som aldrig läser lever bara ett.",
                        Author = "George R.R. Martin",
                        Category = "Litteratur",
                        UserId = adminUser?.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Quote
                    {
                        Text = "Gör vad du kan, med vad du har, där du är.",
                        Author = "Theodore Roosevelt",
                        Category = "Motivation",
                        UserId = adminUser?.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Quote
                    {
                        Text = "Kunskap är makt. Information är befriande. Utbildning är premissen för framsteg i varje samhälle.",
                        Author = "Kofi Annan",
                        Category = "Visdom",
                        UserId = adminUser?.Id,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Quotes.AddRange(quotes);
                context.SaveChanges();
            }
        }
    }
}
