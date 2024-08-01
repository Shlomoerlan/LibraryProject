using LibraryProject.Models;

namespace LibraryProject.Service
{
	public interface ICatalogService
	{
		IEnumerable<BookModel> GetBooksByGenre(string genre);
	 	Task<IEnumerable<BookModel>> GetBooksBySetName(string setName);
		IEnumerable<BookModel> GetBooks(string bookName);
		IEnumerable<BookModel> GetAllBooks();
		IEnumerable<LibraryModel> GetAllLibrarys();
		IEnumerable<BooksSetModel> GetAllBooksSets();
		IEnumerable<ShelfModel> GetAllShelves(long id);
		IEnumerable<BooksSetModel> GetAllBooksSetByShelf(long id);

	}
}
