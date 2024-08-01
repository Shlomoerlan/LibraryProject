using LibraryProject.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.ModelView
{
    public class LibraryMV
    {
        [Required, StringLength(100, MinimumLength = 4)]
        public string Genre { get; set; } = string.Empty;
        public List<ShelfModel> Shelves { get; set; } = [];
    }
}
