using TestProject.Models;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Data
{
    public class TestProjectContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Book> Books { get; set; }

        public TestProjectContext(DbContextOptions<TestProjectContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API
            modelBuilder.Entity<Composition>()
                .HasKey(c => new { c.AuthorID, c.BookID });

            modelBuilder.Entity<Composition>()
               .HasOne(c => c.Author)
               .WithMany(a => a.Composition)
               .HasForeignKey(c => c.AuthorID);

            modelBuilder.Entity<Composition>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Composition)
                .HasForeignKey(c => c.BookID);
        }
    }
}
