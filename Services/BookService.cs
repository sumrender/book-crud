using BooksCrudApi.Models;

namespace BooksCrudApi.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(Guid id);
        Task<Book> CreateBookAsync(Book book);
        Task<Book?> UpdateBookAsync(Guid id, Book book);
        Task<bool> DeleteBookAsync(Guid id);
    }

    public class BookService : IBookService
    {
        private readonly List<Book> _books = new();
        private readonly object _lock = new();

        public BookService()
        {
            // Seed with some sample data
            SeedBooks();
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            await Task.Delay(1); // Simulate async operation
            lock (_lock)
            {
                return _books.ToList();
            }
        }

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            await Task.Delay(1); // Simulate async operation
            lock (_lock)
            {
                return _books.FirstOrDefault(b => b.Id == id);
            }
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            await Task.Delay(1); // Simulate async operation
            lock (_lock)
            {
                book.Id = Guid.NewGuid();
                book.CreatedOn = DateTime.UtcNow;
                book.UpdatedOn = DateTime.UtcNow;
                _books.Add(book);
                return book;
            }
        }

        public async Task<Book?> UpdateBookAsync(Guid id, Book book)
        {
            await Task.Delay(1); // Simulate async operation
            lock (_lock)
            {
                var existingBook = _books.FirstOrDefault(b => b.Id == id);
                if (existingBook == null)
                    return null;

                existingBook.Title = book.Title;
                existingBook.Description = book.Description;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                existingBook.Publisher = book.Publisher;
                existingBook.PublicationYear = book.PublicationYear;
                existingBook.PageCount = book.PageCount;
                existingBook.Genre = book.Genre;
                existingBook.Language = book.Language;
                existingBook.Price = book.Price;
                existingBook.IsAvailable = book.IsAvailable;
                existingBook.CoverImageUrl = book.CoverImageUrl;
                existingBook.UpdatedOn = DateTime.UtcNow;

                return existingBook;
            }
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            await Task.Delay(1); // Simulate async operation
            lock (_lock)
            {
                var book = _books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                    return false;

                return _books.Remove(book);
            }
        }

        private void SeedBooks()
        {
            var sampleBooks = new List<Book>
            {
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Great Gatsby",
                    Description = "A story of the fabulously wealthy Jay Gatsby and his love for the beautiful Daisy Buchanan.",
                    Author = "F. Scott Fitzgerald",
                    ISBN = "978-0743273565",
                    Publisher = "Scribner",
                    PublicationYear = 1925,
                    PageCount = 180,
                    Genre = "Fiction",
                    Language = "English",
                    Price = 12.99m,
                    IsAvailable = true,
                    CreatedOn = DateTime.UtcNow.AddDays(-30),
                    UpdatedOn = DateTime.UtcNow.AddDays(-30),
                    CoverImageUrl = "https://example.com/gatsby-cover.jpg"
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "To Kill a Mockingbird",
                    Description = "The story of young Scout Finch and her father Atticus in a racially divided Alabama town.",
                    Author = "Harper Lee",
                    ISBN = "978-0446310789",
                    Publisher = "Grand Central Publishing",
                    PublicationYear = 1960,
                    PageCount = 281,
                    Genre = "Fiction",
                    Language = "English",
                    Price = 14.99m,
                    IsAvailable = true,
                    CreatedOn = DateTime.UtcNow.AddDays(-25),
                    UpdatedOn = DateTime.UtcNow.AddDays(-25),
                    CoverImageUrl = "https://example.com/mockingbird-cover.jpg"
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "1984",
                    Description = "A dystopian novel about totalitarianism and surveillance society.",
                    Author = "George Orwell",
                    ISBN = "978-0451524935",
                    Publisher = "Signet",
                    PublicationYear = 1949,
                    PageCount = 328,
                    Genre = "Science Fiction",
                    Language = "English",
                    Price = 11.99m,
                    IsAvailable = true,
                    CreatedOn = DateTime.UtcNow.AddDays(-20),
                    UpdatedOn = DateTime.UtcNow.AddDays(-20),
                    CoverImageUrl = "https://example.com/1984-cover.jpg"
                }
            };

            _books.AddRange(sampleBooks);
        }
    }
} 