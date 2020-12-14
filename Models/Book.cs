using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class Book
    {
        public int BookID { get; set; }

        public string Title { get; set; }
        public string Isbn { get; set; }
        public string Summary { get; set; }
        // Navigation Property
        public ICollection<Composition> Composition { get; set; }
    }
}
