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
    public class ShelfModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShelfModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Shelves;               
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelfModel = await _context.Shelves
                .Include(s => s.BooksSets)
                .ThenInclude(books => books.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelfModel == null)
            {
                return NotFound();
            }

            return View(shelfModel);
        }

        public IActionResult Create()
        {
            ViewData["LibraryId"] = new SelectList(_context.Librarys, "Id", "Genre");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShelfVM shelfVm)
        {
            if (ModelState.IsValid)
            {
                ShelfModel shelfModel = new() { 
                    Height = shelfVm.Height,
                    Width = shelfVm.Width,
                    LibraryId = shelfVm.LibraryId,
                };
                _context.Shelves.Add(shelfModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelfModel = await _context.Shelves
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelfModel == null)
            {
                return NotFound();
            }

            return View(shelfModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var shelfModel = await _context.Shelves.FindAsync(id);
            if (shelfModel != null)
            {
                _context.Shelves.Remove(shelfModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
