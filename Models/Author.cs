using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class Author
    {
        public int AuthorID { get; set; }

        [Required, MaxLength(50)]
        public string Surname { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required, StringLength(16)]
        public string FiscalCode { get; set; }
        public string Description { get; set; }
        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(50)]
        public string Nation { get; set; }
        // Navigation Property
        public ICollection<Composition> Composition { get; set; }
    }
}
