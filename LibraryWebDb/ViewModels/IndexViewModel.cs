using LibraryWebDb.Models;

namespace LibraryWebDb.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}
