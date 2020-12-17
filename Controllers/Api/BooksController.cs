using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly TestProjectContext _context;

        public BooksController(TestProjectContext context)
        {
            _context = context;
        }

        // POST: /api/Books
        /*
        {
            "Surname": "{AuthorSurname}",
            "Name": "{AuthorName}"
        }
        */
        [HttpPost]
        public async Task<ActionResult<Dictionary<int, List<Book>>>> GetBooks(RequestBody requestBoby)
        {
            var selectedAuthors = await _context.Authors.Where(a => a.Surname.Contains(requestBoby.Surname) && a.Name.Contains(requestBoby.Name)).ToListAsync();
            var response = new Dictionary<int, List<Book>>();
            foreach (Author a in selectedAuthors) {
                response.Add(a.AuthorID, await _context.Books.Where(b => b.Composition.Any(c => c.AuthorID == a.AuthorID)).ToListAsync());
            }

            return response;
        }
    }
}
