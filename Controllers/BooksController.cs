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
    public class BooksController : Controller
    {
        private readonly TestProjectContext _context;

        public BooksController(TestProjectContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return View(await _context.Books.ToListAsync());
            }
            else {
                var bookByAuthor = await _context.Books
                .Where(b => b.Composition.Any(c => c.AuthorID == id)).ToListAsync();

                return View(bookByAuthor);
            }
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            var authorByBook = await _context.Authors
                .Where(a => a.Composition.Any(c => c.BookID == id)).ToListAsync();

            var detailModelView = new DetailViewModel { Book = book, Authors = authorByBook };

            return View(detailModelView);
        }

        // GET: Books/Create/5
        public IActionResult Create(int id)
        {
            var author = _context.Authors
                .FirstOrDefault(m => m.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }
            var detailModelView = new DetailViewModel { Author = author };
            return View(detailModelView);
        }

        // POST: Books/Create/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookID,Title,Isbn,Summary")] Book book, int id)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                _context.SaveChanges();
                _context.Add(new Composition { AuthorID = id, BookID = book.BookID });
                await _context.SaveChangesAsync();

                /*return RedirectToAction(nameof(Index));*/
                return RedirectToRoute(new
                {
                    controller = "Authors",
                    action = "Details",
                    id = id
                });
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,Title,Isbn,Summary")] Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
