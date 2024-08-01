using LibraryProject.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.ModelView
{
    public class ShelfVM
    {
        public long Id { get; set; }

        [Required]
        public float Width { get; set; } = 0;

        [Required]
        public float Height { get; set; } = 0;
        public long LibraryId { get; set; }
        public List<BooksSetModel> booksSets { get; set; } = [];
    }
}
