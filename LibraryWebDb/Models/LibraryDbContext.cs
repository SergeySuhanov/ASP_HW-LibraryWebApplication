using Microsoft.EntityFrameworkCore;

namespace LibraryWebDb.Models
{
	public class LibraryDbContext : DbContext
	{
        public LibraryDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
