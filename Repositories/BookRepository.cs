using Microsoft.EntityFrameworkCore;
using BooksCrudApi.Data;
using BooksCrudApi.Models;

namespace BooksCrudApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> CreateAsync(Book book)
        {
            book.Id = Guid.NewGuid();
            book.CreatedOn = DateTime.UtcNow;
            book.UpdatedOn = DateTime.UtcNow;
            
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            
            return book;
        }

        public async Task<Book?> UpdateAsync(Guid id, Book book)
        {
            var existingBook = await _context.Books.FindAsync(id);
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

            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }
    }
} 