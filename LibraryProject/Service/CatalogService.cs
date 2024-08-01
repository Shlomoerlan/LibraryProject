using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LibraryProject.Service
{
    public class CatalogService : ICatalogService
	{
		private readonly ApplicationDbContext _context;
		public CatalogService(ApplicationDbContext context)
		{
			_context = context;
		}

        public IEnumerable<BookModel> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BooksSetModel> GetAllBooksSetByShelf(long id)
        {
            return _context.BooksSets.Where(x => x.ShelfId == id).ToImmutableList();
        }

        public IEnumerable<BooksSetModel> GetAllBooksSets()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LibraryModel> GetAllLibrarys()
        {
            ImmutableList<LibraryModel> libraries =
                _context.Librarys.ToImmutableList();
            return libraries;
        }

        public IEnumerable<ShelfModel> GetAllShelves(long id)
        {
            return _context.Shelves.Where(x => x.LibraryId == id).ToImmutableList();
        }

        public IEnumerable<BookModel> GetBooks(string bookName) =>
			_context.Books.Where(book => book.BookName == bookName).ToList();


		public IEnumerable<BookModel> GetBooksByGenre(string genre) =>
			_context.Books.Where(book => book.Genre == genre).ToList();

		public async Task<IEnumerable<BookModel>> GetBooksBySetName(string setName)
		{			
			 var x =  await _context.BooksSets
			.Where(bs => bs.SetName == setName)
			.Include(b => b.Books)
			.SelectMany(b => b.Books).ToListAsync();
			
			return x;
		}
	}
}
