using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.ModelView;

namespace LibraryProject.Service
{
    public class CrudService : ICrudService
    {
        private readonly ApplicationDbContext _context;
        public CrudService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void InsertBook(BookVM book)
        {
            BookModel model = new()
            {
                BookName = book.BookName,
                Width = book.Width,
                Height = book.Height,
                Genre = book.Genre,
            };
            _context.Books.Add(model);
            _context.SaveChanges();
        }

        public void InsertBooksSet(BooksSetvm book)
        {
            BooksSetModel booksSet = new()
            {
                SetName = book.SetName,
            };
            _context.BooksSets.Add(booksSet);
            _context.SaveChanges();
        }


        public void InsertLibrary(LibraryMV libraryMV)
        {
            LibraryModel library = new()
            {
                Genre = libraryMV.Genre
            };
            _context.Librarys.Add(library);
            _context.SaveChanges();
        }

      
        public void InsertShelf(ShelfVM shelf)
        {
            ShelfModel shelfModel = new()
            {
                Width = shelf.Width,
                Height = shelf.Height,
                LibraryId = shelf.LibraryId,
            };
            _context.Shelves.Add(shelfModel);
            _context.SaveChanges();
        }
    }
}
