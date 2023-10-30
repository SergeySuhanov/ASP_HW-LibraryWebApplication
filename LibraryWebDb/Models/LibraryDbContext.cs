using Microsoft.EntityFrameworkCore;

namespace LibraryWebDb.Models
{
	public class LibraryDbContext : DbContext
	{
        public LibraryDbContext(DbContextOptions dbContextOptions)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
