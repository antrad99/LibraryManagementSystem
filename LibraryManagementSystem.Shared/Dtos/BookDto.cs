using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Shared.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }

        [Required]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public int PublicationYear { get; set; }
    }
}
