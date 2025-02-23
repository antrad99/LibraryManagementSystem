using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    public class DbInitializer
    {
        public static void InitializeApplicationDbContext(ApplicationDbContext context)
        {         
            //Seed the db in here
            if (!context.Book.Any())
            {
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
            }
        }
    }
}
