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

namespace LibraryProject.Controllers
{
    public class BooksSetModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksSetModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BooksSets.Include(b => b.Shelf);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            var booksSetModel = await _context.BooksSets
                .Include(bs => bs.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booksSetModel == null)
            {
                return NotFound();
            }
            return View(booksSetModel);
        }


        public IActionResult Create()
        {
            var x = _context.Librarys.Select(l => l.Genre).ToList();
            var y = _context.Shelves.Select(s => s.LibraryId).ToList();
            var z = _context.Shelves;
            ViewData["ShelfId"] = new SelectList(_context.Shelves, "Id", "Id");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BooksSetvm booksSetVm)
        {
            if (ModelState.IsValid)
            {
                BooksSetModel booksSetModel = new()
                {
                    SetName = booksSetVm.SetName,
                    ShelfId = booksSetVm.ShelfId,
                };
                _context.Add(booksSetModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ShelfId"] = new SelectList(_context.Shelves, "Id", "Id", booksSetVm.ShelfId);
            return View();
        }

          
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booksSetModel = await _context.BooksSets
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booksSetModel == null)
            {
                return NotFound();
            }

            return View(booksSetModel);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var booksSetModel = await _context.BooksSets.FindAsync(id);
            if (booksSetModel != null)
            {
                _context.BooksSets.Remove(booksSetModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
