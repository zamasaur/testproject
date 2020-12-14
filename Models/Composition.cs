using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Models
{
    public class Composition
    {
        /*public int CompositionID { get; set; }*/
        public int AuthorID { get; set; }
        public int BookID { get; set; }
        public Author Author { get; set; }
        public Book Book { get; set; }
    }
}
