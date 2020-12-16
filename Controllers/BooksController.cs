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

        // GET: Books/Details/{id}?authorId={authorId}
        public async Task<IActionResult> Details(int? id, int? authorId)
        {
            if (id == null || authorId == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            var authorByBook = await _context.Authors
                .Where(a => a.Composition.Any(c => c.BookID == id)).ToListAsync();

            var modelView = new ViewModel { Book = book, AuthorID = (int)authorId, Authors = authorByBook };

            return View(modelView);
        }

        // GET: Books/Create/{authorId}
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _context.Authors
                .FirstOrDefault(a => a.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }

            var detailModelView = new ViewModel { Author = author };
            return View(detailModelView);
        }

        // POST: Books/Create/{authorId}
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

                return RedirectToRoute(new
                {
                    controller = "Authors",
                    action = "Details",
                    id = id
                });
            }
            return View(book);
        }

        // GET: Books/Edit/{id}?authorId={authorId}
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

            var viewModel = new ViewModel { Book = book , Author = author};
            return View(viewModel);
        }

        // POST: Books/Edit/{id}?authorId={authorId}
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
            }
            return RedirectToRoute(new
            {
                controller = "Authors",
                action = "Details",
                id = authorId
            });
        }

        // GET: Books/Delete/{id}?authorId={authorId}
        public async Task<IActionResult> Delete(int? id, int? authorId)
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

            var viewModel = new ViewModel { Book = book , Author = author};
            return View(viewModel);
        }

        // POST: Books/Delete/{id}?authorId={authorId}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int authorId)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToRoute(new
            {
                controller = "Authors",
                action = "Details",
                id = authorId
            });
        }

        // GET: Books/AddAuthor/{id}?authorId={authorId}
        public async Task<IActionResult> AddAuthor(int? id, int? authorId)
        {
            if (id == null || authorId == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            var addableAuthors = await _context.Authors
                .Where(a => !a.Composition.Any(c => c.BookID == id)).ToListAsync();

            var author = await _context.Authors.FindAsync(authorId);

            var viewModel = new ViewModel { Book = book, Author = author, Authors = addableAuthors };

            return View(viewModel);
        }

        // POST: Books/AddAuthor/{id}?authorId={authorId}
        [HttpPost, ActionName("AddAuthor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAuthorConfirmed(int id, int authorId)
        {
            var book = await _context.Books.FindAsync(id);
            var author = await _context.Authors.FindAsync(authorId);
            if (book == null || author == null|| _context.Compositions.Any(c => c.AuthorID == authorId && c.BookID == id))
            {
                return NotFound();
            }

            var composition = _context.Compositions.Add(new Composition { AuthorID = authorId, BookID = id });
            await _context.SaveChangesAsync();

            var addableAuthors = await _context.Authors
                .Where(a => !a.Composition.Any(c => c.BookID == id)).ToListAsync();

            var viewModel = new ViewModel { Book = book, Authors = addableAuthors };
            return PartialView("_AddAuthorList", viewModel);
        }

        // POST: Books/AddAuthor/{id}?authorId={authorId}
        [HttpPost, ActionName("RemoveAuthor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAuthorConfirmed(int id, int authorId)
        {
            var book = await _context.Books.FindAsync(id);
            var author = await _context.Authors.FindAsync(authorId);
            if (book == null || author == null || !_context.Compositions.Any(c => c.AuthorID == authorId && c.BookID == id))
            {
                return NotFound();
            }

            var composition = _context.Compositions.Remove(new Composition { AuthorID = authorId, BookID = id });
            await _context.SaveChangesAsync();

            var removableAuthors = await _context.Authors
                .Where(a => a.Composition.Any(c => c.BookID == id)).ToListAsync();

            var viewModel = new ViewModel { Book = book, Authors = removableAuthors };
            return PartialView("_RemoveAuthorList", viewModel);
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
