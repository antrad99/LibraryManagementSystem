using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Shared.Dtos;
using LibraryManagementSystem.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace XunitTests
{
    public class BookServiceTests
    {
        private ServiceCollection _services = new ServiceCollection();

        public BookServiceTests()
        {
            _services.AddDbContext<ApplicationDbContext>(o =>
                    o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        }

        [Fact]
        public async void GetBookAll()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    context.Book.Add(new Book
                    {
                        Id = 1,
                        ISBN = "ABC1234567",
                        Title = "Book 1",
                        Author = "Author 1",
                        PublicationYear = 2020
                    });
                    context.Book.Add(new Book
                    {
                        Id = 2,
                        ISBN = "ABC1234568",
                        Title = "Book 2",
                        Author = "Author 2",
                        PublicationYear = 2021
                    });
                    context.SaveChanges();

                    var bookService = new BookService(context);

                    var results = await bookService.GetBookAll(BookOrderByEnum.Title);

                    Assert.Equal(2, results.Count);
                    //It needs to check each single field to make sure the mapping is correct.
                    Assert.Equal(1, results[0].Id);
                    Assert.Equal("ABC1234567", results[0].ISBN);
                    Assert.Equal("Book 1", results[0].Title);
                    Assert.Equal("Author 1", results[0].Author);
                    Assert.Equal(2020, results[0].PublicationYear);
                }
            }
        }

        [Fact]
        public async void GetBookById()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    context.Book.Add(new Book
                    {
                        Id = 1,
                        ISBN = "ABC1234567",
                        Title = "Book 1",
                        Author = "Author 1",
                        PublicationYear = 2020
                    });

                    context.SaveChanges();

                    var bookService = new BookService(context);

                    var result = await bookService.GetBookById(1);

                    Assert.NotNull(result);

                    Assert.Equal(1, result.Id);
                    Assert.Equal("ABC1234567", result.ISBN);
                    Assert.Equal("Book 1", result.Title);
                    Assert.Equal("Author 1", result.Author);
                    Assert.Equal(2020, result.PublicationYear);
                }
            }
        }

        [Fact]
        public async void AddBook()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var bookDto = new BookDto
                    {
                        Id = 1,
                        ISBN = "ABC1234567",
                        Title = "Book 1",
                        Author = "Author 1",
                        PublicationYear = 2020
                    };

                    var bookService = new BookService(context);

                    var result = await bookService.AddBook(bookDto);

                    Assert.NotNull(result);

                    Assert.Equal(1, result.Id);
                    Assert.Equal("ABC1234567", result.ISBN);
                    Assert.Equal("Book 1", result.Title);
                    Assert.Equal("Author 1", result.Author);
                    Assert.Equal(2020, result.PublicationYear);

                    var book = await context.Book.Where(t => t.Id == result.Id).FirstOrDefaultAsync();

                    Assert.NotNull(book);

                    Assert.Equal(1, book.Id);
                    Assert.Equal("ABC1234567", book.ISBN);
                    Assert.Equal("Book 1", book.Title);
                    Assert.Equal("Author 1", book.Author);
                    Assert.Equal(2020, book.PublicationYear);
                }
            }

        }

        [Fact]
        public async void UpdateBook()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var bookDto = new BookDto
                    {
                        Id = 1,
                        ISBN = "ABC1234567",
                        Title = "Book 1",
                        Author = "Author 1",
                        PublicationYear = 2020
                    };

                    var bookService = new BookService(context);

                    await bookService.AddBook(bookDto);

                    bookDto.ISBN = "ABC1234567_";
                    bookDto.Title = "Book 1_";
                    bookDto.Author = "Author 1_";
                    bookDto.PublicationYear = 2021;

                    var result = await bookService.UpdateBook(bookDto);

                    Assert.NotNull(result);

                    var book = await context.Book.Where(t => t.Id == result.Id).FirstOrDefaultAsync();

                    Assert.NotNull(book);

                    Assert.Equal(1, book.Id);
                    Assert.Equal("ABC1234567_", book.ISBN);
                    Assert.Equal("Book 1_", book.Title);
                    Assert.Equal("Author 1_", book.Author);
                    Assert.Equal(2021, book.PublicationYear);
                }
            }

        }

        [Fact]
        public async void IsbnAlreadyExist_True()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var bookDto = new BookDto
                    {
                        Id = 1,
                        ISBN = "ABC1234567",
                        Title = "Book 1",
                        Author = "Author 1",
                        PublicationYear = 2020
                    };

                    var bookService = new BookService(context);

                    await bookService.AddBook(bookDto);
                    var result = await bookService.IsbnAlreadyExist("ABC1234567");


                    Assert.True(result);

                }
            }

        }

        [Fact]
        public async void IsbnAlreadyExist_False()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var bookDto = new BookDto
                    {
                        Id = 1,
                        ISBN = "ABC1234568",
                        Title = "Book 1",
                        Author = "Author 1",
                        PublicationYear = 2020
                    };

                    var bookService = new BookService(context);

                    await bookService.AddBook(bookDto);
                    var result = await bookService.IsbnAlreadyExist("ABC1234567");


                    Assert.False(result);

                }
            }

        }

    }
}
