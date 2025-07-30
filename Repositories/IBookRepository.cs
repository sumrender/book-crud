using BooksCrudApi.Models;

namespace BooksCrudApi.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(Guid id);
        Task<Book> CreateAsync(Book book);
        Task<Book?> UpdateAsync(Guid id, Book book);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
} 