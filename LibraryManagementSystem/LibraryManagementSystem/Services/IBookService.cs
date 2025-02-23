using LibraryManagementSystem.Shared.Dtos;
using LibraryManagementSystem.Shared.Enums;


namespace LibraryManagementSystem.Services
{
    public interface IBookService
    {
        Task<List<BookDto>> GetBookAll(BookOrderByEnum orderBy);
        Task<BookDto?> GetBookById(int id);
        Task<BookDto> AddBook(BookDto bookDto);
        Task<BookDto> UpdateBook(BookDto bookDto);
        Task<bool> IsbnAlreadyExist(string ISBN);

    }
}
