using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryProject.Data;
using LibraryProject.Models;

namespace LibraryProject.Controllers
{
    public class LibraryModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibraryModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Librarys.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryModel = await _context.Librarys.Include(l => l.Shelves)
                .ThenInclude(s => s.BooksSets)
                .ThenInclude(b => b.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryModel == null)
            {
                return NotFound();
            }

            return View(libraryModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibraryModel libraryModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libraryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libraryModel);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryModel = await _context.Librarys.FindAsync(id);
            if (libraryModel == null)
            {
                return NotFound();
            }
            return View(libraryModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, LibraryModel libraryModel)
        {
            if (id != libraryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(libraryModel);
                await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
            }
            return View(libraryModel);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryModel = await _context.Librarys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryModel == null)
            {
                return NotFound();
            }

            return View(libraryModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var libraryModel = await _context.Librarys.FindAsync(id);
            if (libraryModel != null)
            {
                _context.Librarys.Remove(libraryModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
