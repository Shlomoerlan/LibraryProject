using LibraryProject.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.ModelView
{
    public class BookVM
    {
        public long Id { get; set; }

        [Required, StringLength(100, MinimumLength = 4)]
        public string BookName { get; set; } = string.Empty;

        [Required]
        public float Width { get; set; } = 0;

        [Required]
        public float Height { get; set; } = 0;

        [Required, StringLength(100, MinimumLength = 4)]
        public string Genre { get; set; } = string.Empty;
        public long BooksSetId { get; set; }
    }
}
