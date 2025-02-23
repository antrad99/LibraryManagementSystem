using LibraryManagementSystem.Data;
using LibraryManagementSystem.Mapping;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Shared.Enums;


namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookDto>> GetBookAll(BookOrderByEnum orderBy)
        {
           
            IQueryable<Book> tisQuery = _context.Book;

            if (orderBy == BookOrderByEnum.Title)
                tisQuery = tisQuery.OrderBy(a => a.Title);
            else
                tisQuery = tisQuery.OrderBy(a => a.PublicationYear);
                    
                    
            var books = await tisQuery.ToListAsync();

            var booksDto = new List<BookDto>();
            var bookMapping = new BookMapping();

            foreach (var book in books)
                booksDto.Add(bookMapping.MapToDto(book));

            return booksDto;

        }

        public async Task<BookDto?> GetBookById(int id)
        {
            var book = await _context.Book
                            .Where(t => t.Id == id).FirstOrDefaultAsync();

            if (book == null)
                return null;

            var bookMapping = new BookMapping();
            var bookDto = bookMapping.MapToDto(book);

            return bookDto;
        }

        public async Task<BookDto> AddBook(BookDto bookDto)
        {
            //Check Pub Year
            if (bookDto.PublicationYear > DateTime.Today.Year)
                throw new ArgumentException("Publication year cannot be in the future", nameof(bookDto));

            //No need to check ISBN is unique, this field must be unique

            var bookMapping = new BookMapping();
            var book = bookMapping.MapFromDto(new Book(), bookDto);

            await _context.Book.AddAsync(book);

            await _context.SaveChangesAsync();

            bookDto.Id = book.Id;

            return bookDto;
        }

        public async Task<BookDto> UpdateBook(BookDto bookDto)
        {
            //Check Pub Year
            if (bookDto.PublicationYear > DateTime.Today.Year)
                throw new ArgumentException("Publication year cannot be in the future", nameof(bookDto));

            //No need to check ISBN is unique, this field must be unique

            //Check Book
            var book = _context.Book.Where(t => t.Id == bookDto.Id).FirstOrDefault();
            if (book == null)
                throw new ArgumentException("Book Id not found", nameof(bookDto));
     

            var bookMapping = new BookMapping();
            //Map book
            book = bookMapping.MapFromDto(book, bookDto);

            await _context.SaveChangesAsync();

            return bookDto;

        }

        public async Task<bool> IsbnAlreadyExist(string ISBN)
        {
            var book = _context.Book.Where(t => t.ISBN == ISBN).FirstOrDefault();
            if (book == null)
                return false;
            else
                return true;
        }

    }
}
