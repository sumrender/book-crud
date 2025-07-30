using Microsoft.AspNetCore.Mvc;
using BooksCrudApi.Models;
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
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(Guid id)
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
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBook = await _bookService.CreateBookAsync(book);
            
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedBook = await _bookService.UpdateBookAsync(id, book);
            
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