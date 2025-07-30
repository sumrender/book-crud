using BooksCrudApi.Models;
using BooksCrudApi.Models.DTOs;
using BooksCrudApi.Repositories;

namespace BooksCrudApi.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();
        Task<BookResponseDto?> GetBookByIdAsync(Guid id);
        Task<BookResponseDto> CreateBookAsync(BookRequestDto bookDto);
        Task<BookResponseDto?> UpdateBookAsync(Guid id, BookUpdateDto bookDto);
        Task<bool> DeleteBookAsync(Guid id);
    }

    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Select(MapToResponseDto);
        }

        public async Task<BookResponseDto?> GetBookByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return book != null ? MapToResponseDto(book) : null;
        }

        public async Task<BookResponseDto> CreateBookAsync(BookRequestDto bookDto)
        {
            var book = MapToEntity(bookDto);
            var createdBook = await _bookRepository.CreateAsync(book);
            return MapToResponseDto(createdBook);
        }

        public async Task<BookResponseDto?> UpdateBookAsync(Guid id, BookUpdateDto bookDto)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);
            if (existingBook == null)
                return null;

            UpdateEntityFromDto(existingBook, bookDto);
            existingBook.UpdatedOn = DateTime.UtcNow;
            
            var updatedBook = await _bookRepository.UpdateAsync(id, existingBook);
            return MapToResponseDto(updatedBook);
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            return await _bookRepository.DeleteAsync(id);
        }

        private static Book MapToEntity(BookRequestDto dto)
        {
            return new Book
            {
                Title = dto.Title,
                Description = dto.Description,
                Author = dto.Author,
                ISBN = dto.ISBN,
                Publisher = dto.Publisher,
                PublicationYear = dto.PublicationYear,
                PageCount = dto.PageCount,
                Genre = dto.Genre,
                Language = dto.Language,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                CoverImageUrl = dto.CoverImageUrl,
                CreatedOn = DateTime.UtcNow
            };
        }

        private static void UpdateEntityFromDto(Book entity, BookUpdateDto dto)
        {
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.Author != null) entity.Author = dto.Author;
            if (dto.ISBN != null) entity.ISBN = dto.ISBN;
            if (dto.Publisher != null) entity.Publisher = dto.Publisher;
            if (dto.PublicationYear.HasValue) entity.PublicationYear = dto.PublicationYear.Value;
            if (dto.PageCount.HasValue) entity.PageCount = dto.PageCount.Value;
            if (dto.Genre != null) entity.Genre = dto.Genre;
            if (dto.Language != null) entity.Language = dto.Language;
            if (dto.Price.HasValue) entity.Price = dto.Price.Value;
            if (dto.IsAvailable.HasValue) entity.IsAvailable = dto.IsAvailable.Value;
            if (dto.CoverImageUrl != null) entity.CoverImageUrl = dto.CoverImageUrl;
        }

        private static BookResponseDto MapToResponseDto(Book entity)
        {
            return new BookResponseDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Author = entity.Author,
                ISBN = entity.ISBN,
                Publisher = entity.Publisher,
                PublicationYear = entity.PublicationYear,
                PageCount = entity.PageCount,
                Genre = entity.Genre,
                Language = entity.Language,
                Price = entity.Price,
                IsAvailable = entity.IsAvailable,
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn,
                CoverImageUrl = entity.CoverImageUrl
            };
        }
    }
} 