using LibraryManagementSystem.Services;
using LibraryManagementSystem.Shared.Dtos;
using LibraryManagementSystem.Shared.Enums;
using Microsoft.AspNetCore.Mvc;


namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("orderBy/{orderBy}")]
        public async Task<List<BookDto>> GetBookAll(BookOrderByEnum orderBy)
        {
            //Errors are handled by the framework itself: in production it redirects to  the Error page
            //to prevent showing stack info to the user and it saves in the actual error in the Log file.
            return await _bookService.GetBookAll(orderBy);
        }

        [HttpGet("{id}")]
        public async Task<BookDto?> GetBookById(int id)
        {
            return await _bookService.GetBookById(id);
        }

        [HttpPost("")]
        public async Task<BookDto> AddBook([FromBody] BookDto bookDto)
        {
            return await _bookService.AddBook(bookDto);
        }

        [HttpPut("{id}")]
        public async Task<BookDto?> UpdateBook([FromBody] BookDto bookDto, int id)
        {
            bookDto.Id = id;
            return await _bookService.UpdateBook(bookDto);
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<bool> IsbnAlreadyExist(string isbn)
        {
            return await _bookService.IsbnAlreadyExist(isbn);
        }
    }
}
