using LibraryManagementSystem.Models;
using LibraryManagementSystem.Shared.Dtos;

namespace LibraryManagementSystem.Mapping
{
    public class BookMapping
{
    public BookDto MapToDto(Book book)
    {
        BookDto bookDto = new BookDto();
        PropertyCopier<Book, BookDto>.Copy(book, bookDto);

        return bookDto;
    }

    public Book MapFromDto(Book book, BookDto bookDto)
    {
        PropertyCopier<BookDto, Book>.Copy(bookDto, book);
        return book;
    }
}
}
