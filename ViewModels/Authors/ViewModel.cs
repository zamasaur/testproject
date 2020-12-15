using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;

namespace TestProject.ViewModels
{
    public class ViewModel
    {
        public int AuthorID { get; set; }
        public int BookID { get; set; }
        public Author Author { get; set; }
        public Book Book { get; set; }
        public List<Author> Authors { get; set; }
        public List<Book> Books { get; set; }
    }
}
