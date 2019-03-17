using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using BookShop.Models;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using (var dbContext = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(dbContext);

                var result = RemoveBooks(dbContext);

                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books
                .Where(x => x.AgeRestriction == Enum.Parse<AgeRestriction>(command, true))
                .Select(t => t.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(t => t.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(p => p.Price > 40)
                .Select(x => new
                {
                    Title = x.Title,
                    Price = x.Price
                })
                .OrderByDescending(p => p.Price)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(x => $"{x.Title} - ${x.Price:F2}"));
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(id => id.BookId)
                .Select(t => t.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Where(x => x.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(d => d.ReleaseDate < dateTime)
                .OrderByDescending(d => d.ReleaseDate)
                .Select(x => new
                {
                    Title = x.Title,
                    Type = x.EditionType,
                    Price = x.Price
                })
                .ToList();

            return string.Join(Environment.NewLine, books.Select(x => $"{x.Title} - {x.Type} - ${x.Price:F2}"));
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(n => EF.Functions.Like(n.FirstName, $"%{input}"))
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName
                })
                .OrderBy(x => x.FullName)
                .ToList();

            return string.Join(Environment.NewLine, authors.Select(x => x.FullName));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(t => EF.Functions.Like(t.Title, $"%{input.ToLower()}%"))
                .Select(t => t.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => EF.Functions.Like(x.Author.LastName, $"{input.ToLower()}%"))
                .OrderBy(id => id.BookId)
                .Select(x => new
                {
                    Title = x.Title,
                    FirstName = x.Author.FirstName,
                    LastName = x.Author.LastName
                })
                .ToList();

            return string.Join(Environment.NewLine, books.Select(x => $"{x.Title} ({x.FirstName} {x.LastName})"));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Count(t => t.Title.Length > lengthCheck);
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(x => new
                {
                    Author = x.FirstName + " " + x.LastName,
                    Copies = x.Books.Sum(c => c.Copies)
                })
                .OrderByDescending(c => c.Copies)
                .ToList();

            return string.Join(Environment.NewLine, authors.Select(a => $"{a.Author} - {a.Copies}"));
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    Name = x.Name,
                    Profit = x.CategoryBooks.Sum(p => p.Book.Copies * p.Book.Price)
                })
                .OrderByDescending(p => p.Profit)
                .ThenBy(n => n.Name)

                .ToList();

            return string.Join(Environment.NewLine, categories.Select(x => $"{x.Name} ${x.Profit:F2}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoriesWithBooks = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks
                        .Select(cb => cb.Book)
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3)
                })
                .ToList();

            return String.Join(Environment.NewLine,
                categoriesWithBooks
                    .Select(c => $"--{c.Name}{Environment.NewLine}{String.Join(Environment.NewLine, c.Books.Select(b => $"{b.Title} ({b.ReleaseDate.Value.Year})"))}"));
        }

        public static void IncreasePrices(BookShopContext context)
        {
            context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList()
                .ForEach(b => b.Price += 5);

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(books);

            context.SaveChanges();

            return books.Count();
        }
    }
}

