using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;
using TestProject.ViewModels;

namespace TestProject.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly TestProjectContext _context;

        public AuthorsController(TestProjectContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            var allAuthors = await _context.Authors.ToListAsync();
            var allComposition = await _context.Compositions.ToListAsync();

            var viewModel = new ViewModel {Author = new Author { }, Authors = allAuthors, Compositions = allComposition };

            return View(viewModel);
        }

        // GET: Authors/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }

            var bookByAuthor = await _context.Books
                .Where(b => b.Composition.Any(c => c.AuthorID == id)).ToListAsync();

            var viewModel = new ViewModel { Author = author, Books = bookByAuthor };

            return View(viewModel);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorID,Surname,Name,BirthDate,FiscalCode,Description,PhoneNumber,Nation")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/{id}
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorID,Surname,Name,BirthDate,FiscalCode,Description,PhoneNumber,Nation")] Author author)
        {
            if (id != author.AuthorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // POST: Authors/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // avoid book without author
            if (!_context.Compositions.Any(c => c.AuthorID == id)) {
                var author = await _context.Authors.FindAsync(id);
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }

            var allAuthors = await _context.Authors.ToListAsync();
            var allComposition = await _context.Compositions.ToListAsync();

            var viewModel = new ViewModel { Author = new Author { }, Authors = allAuthors, Compositions = allComposition };

            return PartialView("_AuthorList", viewModel);
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorID == id);
        }
    }
}
