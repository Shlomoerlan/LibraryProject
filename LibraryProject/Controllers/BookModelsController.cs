using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.ModelView;
using System.Linq.Expressions;

namespace LibraryProject.Controllers
{
    public class BookModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(long id)
        {
            var applicationDbContext = _context.BooksSets.Include(l => l.Books).Where(b => b.Id == id).FirstOrDefault().Books;
                ;
            return View( applicationDbContext);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books
                .Include(b => b.BooksSet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        public IActionResult Create(long id)
        {
            return View(new BookVM { BooksSetId = id});
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( BookVM bookVM )
        {
            var bs = _context.BooksSets.Where(b => b.Id == bookVM.BooksSetId).FirstOrDefault();
            if (bs == null) { return RedirectToAction("Index"); }
            var s = _context.Shelves.Where(s => s.Id == bs.ShelfId).FirstOrDefault();
            if (s == null) { return RedirectToAction("Index"); }
            var l = _context.Librarys.Where(l => l.Id == s.LibraryId).FirstOrDefault();
            bookVM.Genre = l.Genre;

                BookModel bookModel = new()
                {
                    BookName = bookVM.BookName,
                    Width = bookVM.Width,
                    Height = bookVM.Height,
                    Genre = bookVM.Genre,
                    BooksSetId = bookVM.BooksSetId,
                };
            try 
            {
                if (!CheckerHeightShelf(bookModel))
                {
                    ViewBag.Short = "For your Attention: The book is too short";
                }
                CheckerWidthAndHyShelf(bookModel);
                _context.Add(bookModel);
                await _context.SaveChangesAsync();
                ViewBag.Suc = "The Book Insert Succefulli";
                return View(new BookVM { BooksSetId = bookModel.BooksSetId });
            }
            catch (Exception ex) 
            {
                ViewBag.ex = ex.Message;
                return View(bookVM);
            }    
        }
 

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books
                .Include(b => b.BooksSet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var bookModel = await _context.Books.FindAsync(id);
            if (bookModel != null)
            {
                _context.Books.Remove(bookModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool CheckerWidthAndHyShelf(BookModel newBook)
        {
            BooksSetModel? nwSetModel = _context.BooksSets
                .Include(bs => bs.Books)
                .Where(b => b.Id != newBook.Id)
                .FirstOrDefault(bs => bs.Id == newBook.BooksSetId);
            if (nwSetModel == null)
                throw new Exception(ErrorNotFoundMessage("set"));
            float shelf = _context.BooksSets.
                Include(bs => bs.Books)
                .Where(bs => bs.ShelfId == nwSetModel.ShelfId)
                .SelectMany(bs => bs.Books)
                .Sum(b => b.Width); ;
            if (newBook.Height > _context.Shelves.Find(nwSetModel.ShelfId)!.Height)
            {
                throw new Exception(ErrorHighMessage());
            }
            if (shelf + nwSetModel!.Books.Sum(b => b.Width) + newBook.Width
                >= _context.Shelves.Find(nwSetModel.ShelfId)!.Width)
                throw new Exception(ErrorWidthMessage());
            return true;
        }
        public string ErrorHighMessage()
        {
            return "The height of the shelf is lower " +
                    "than the height of the requested book If you still want to add, " +
                    "please move the set to another shelf that is high enough, then add again.";
        }
        public string ErrorWidthMessage()
        {
            return $"There is no room on the shelf to add the requested book." +
                    $" If you still want to add, please move the set to " +
                    $"another shelf that has room, then add again. ";
        }
        public string ErrorNotFoundMessage(string wanted)
        {
            return $"The corect {wanted} not faund";
        }














        public bool CheckerWidthShelf(BookModel newBook)
        {
            var nwSetModel = _context.BooksSets
                .Include(bs => bs.Books)
                .Where(b => b.Id != newBook.Id)
                .FirstOrDefault(bs => bs.Id == newBook.BooksSetId);
            if (nwSetModel == null) throw new Exception(
                    $"The corect Shelf not faund"
                );
            var shelf = _context.Shelves.
                Include(sh => sh.BooksSets)
                .ThenInclude(bs => bs.Books)
                .FirstOrDefault(sh => sh.Id == nwSetModel.ShelfId);
            if (shelf == null) return false;
            var res1 = shelf.BooksSets.Where(bs => bs.Id != nwSetModel.Id)
                .SelectMany(bs => bs.Books)
                .Sum(b => b.Width);
            if (res1 + nwSetModel!.Books.Sum(b => b.Width) + newBook.Height >= shelf.Width)
                throw new Exception(
                    $"There is no room on the shelf to add the requested book." +
                    $" If you still want to add, please move the set to " +
                    $"another shelf that has room, then add again. "
                );
            return true;
        }

        public bool CheckerHeightShelf(BookModel newBook)
        {
            var nwSetModel = _context.BooksSets
                .Find(newBook.BooksSetId);
            var shelf = _context.Shelves.Find(nwSetModel.ShelfId);
            return !(shelf.Height - newBook.Height < 10);
        }
    }
}
