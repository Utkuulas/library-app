using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            // Add books to the database
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any books.
                if (context.Books.Any())
                {
                    return;   // DB has been seeded
                }
                context.Books.AddRange(
                        new Book
                        {
                            Title = "The Pragmatic Programmer",
                            Author = "Andrew Hunt & David Thomas",
                            ISBN = "978-0201616224",
                            PublishedYear = 1999,
                            Genre = "Software Engineering",
                            IsAvailable = true
                        },
                        new Book
                        {
                            Title = "Clean Code",
                            Author = "Robert C. Martin",
                            ISBN = "978-0132350884",
                            PublishedYear = 2008,
                            Genre = "Programming",
                            IsAvailable = false
                        },
                        new Book
                        {
                            Title = "1984",
                            Author = "George Orwell",
                            ISBN = "978-0451524935",
                            PublishedYear = 1949,
                            Genre = "Dystopian",
                            IsAvailable = true
                        },
                        new Book
                        {
                            Title = "To Kill a Mockingbird",
                            Author = "Harper Lee",
                            ISBN = "978-0061120084",
                            PublishedYear = 1960,
                            Genre = "Classic",
                            IsAvailable = false
                        },
                        new Book
                        {
                            Title = "The Lord of the Rings",
                            Author = "J.R.R. Tolkien",
                            ISBN = "978-0618640157",
                            PublishedYear = 1954,
                            Genre = "Fantasy",
                            IsAvailable = true
                        }
                );
                context.SaveChanges();
            }
        }
    }
}
