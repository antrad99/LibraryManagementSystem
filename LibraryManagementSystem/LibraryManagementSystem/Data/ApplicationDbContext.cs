using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public required DbSet<Book> Book { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Book Table
            builder.Entity<Book>().Property(b => b.ISBN).IsRequired();
            builder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();
            builder.Entity<Book>().Property(b => b.Title).IsRequired();
            builder.Entity<Book>().Property(b => b.Author).IsRequired();
            builder.Entity<Book>().Property(b => b.PublicationYear).IsRequired();
         

            base.OnModelCreating(builder);

        }
    }
}
