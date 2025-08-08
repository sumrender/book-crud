using Microsoft.AspNetCore.Mvc;
using BooksCrudApi.Models.DTOs;
using BooksCrudApi.Services;

namespace BooksCrudApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<PaginatedResponseDto<BookResponseDto>>> GetBooks(
            [FromQuery] int skip = 0, 
            [FromQuery] int take = 10)
        {
            if (take <= 0)
            {
                return BadRequest("Take parameter must be greater than 0");
            }
            
            if (skip < 0)
            {
                return BadRequest("Skip parameter must be greater than or equal to 0");
            }
            
            var paginatedBooks = await _bookService.GetPaginatedBooksAsync(skip, take);
            return Ok(paginatedBooks);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponseDto>> GetBook(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<BookResponseDto>> CreateBook(BookRequestDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBook = await _bookService.CreateBookAsync(bookDto);
            
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<BookResponseDto>> UpdateBook(Guid id, BookUpdateDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedBook = await _bookService.UpdateBookAsync(id, bookDto);
            
            if (updatedBook == null)
            {
                return NotFound();
            }

            return Ok(updatedBook);
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var deleted = await _bookService.DeleteBookAsync(id);
            
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
} 