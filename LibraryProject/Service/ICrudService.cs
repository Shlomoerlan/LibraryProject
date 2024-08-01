using LibraryProject.Models;
using LibraryProject.ModelView;

namespace LibraryProject.Service
{
    public interface ICrudService
    {
        void InsertBook(BookVM book);
        void InsertBooksSet(BooksSetvm booksSet);
        void InsertShelf(ShelfVM shelf);
        void InsertLibrary(LibraryMV library);
    }
}
