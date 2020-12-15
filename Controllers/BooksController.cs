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
            
            var modelView = new ViewModel { BookID = book.BookID, Book = book, Authors = authorByBook };

            return View(modelView);
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
            var detailModelView = new ViewModel { Author = author };
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
        public async Task<IActionResult> Edit(int? id, int? authorId)
        {
            if (id == null || authorId == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            var author = await _context.Authors.FindAsync(authorId);
            if (book == null || author == null || !_context.Compositions.Any(c => c.AuthorID == authorId && c.BookID == id))
            {
                return NotFound();
            }

            var detailModelView = new ViewModel { Book = book , Author = author};
            return View(detailModelView);
            /*return View(book);*/
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int authorId, [Bind("BookID,Title,Isbn,Summary")] Book book)
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
                //return RedirectToAction(nameof(Index));
            }
            //return View(book);
            return RedirectToRoute(new
            {
                controller = "Authors",
                action = "Details",
                id = authorId
            });
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

        // GET: Books/AddAuthor
        public async Task<IActionResult> AddAuthor(int? id)
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

            var authors = await _context.Authors
                .Where(a => !a.Composition.Any(c => c.BookID == id)).ToListAsync();

            /*var authors = await _context.Authors.ToListAsync();*/
            var detailModelView = new ViewModel { BookID = book.BookID, Book = book, Authors = authors };

            return View(detailModelView);
        }

        // POST: Books/AddAuthor/{BookID}?authorId={AuthorID}
        [HttpPost, ActionName("AddAuthor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAuthorConfirmed(int id, int authorId)
        {
            var book = await _context.Books.FindAsync(id);
            var author = await _context.Authors.FindAsync(authorId);

            var composition = _context.Compositions.Add(new Composition { AuthorID = authorId, BookID = id });
            await _context.SaveChangesAsync();

            var notAuthors = await _context.Authors
                .Where(a => !a.Composition.Any(c => c.BookID == id)).ToListAsync();

            var modelView = new ViewModel { BookID = book.BookID, Book = book, Authors = notAuthors };
            return PartialView("_AddAuthorList", modelView);
        }

    }
}
