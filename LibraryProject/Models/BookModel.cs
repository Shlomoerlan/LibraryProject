using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Models
{
	public class BookModel
	{
		public long Id { get; set; }

		[Required, StringLength(100, MinimumLength = 4)]
		public required string BookName { get; set; }

		[Required]
		public required float Width { get; set; }

		[Required]
		public required float Height { get; set; }

		[Required, StringLength(100, MinimumLength = 4)]
		public required string Genre { get; set; }
		public BooksSetModel BooksSet { get; set; }
		public long BooksSetId {  get; set; }
	}
}