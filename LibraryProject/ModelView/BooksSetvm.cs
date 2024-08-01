using LibraryProject.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.ModelView
{
    public class BooksSetvm
    {
        public long Id { get; set; }

        [Required, StringLength(100, MinimumLength = 4)]
        public  string SetName { get; set; } = string.Empty;
        public long ShelfId { get; set; }
        public List<BookModel> Books { get; set; } = [];
    }
}
