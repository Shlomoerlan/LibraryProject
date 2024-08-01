using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Models
{
	public class BooksSetModel
	{
		public long Id { get; set; }

		[Required, StringLength(100, MinimumLength = 4)]
		public required string SetName { get; set; }
		public ShelfModel Shelf { get; set; }
		public long ShelfId {  get; set; }
		public List<BookModel> Books { get; set; } = [];
	}
}