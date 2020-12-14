using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class Author
    {
        public int AuthorID { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public string FiscalCode { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Nation { get; set; }
        // Navigation Property
        public ICollection<Composition> Composition { get; set; }
    }
}
