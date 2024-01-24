using LibraryWebDb.Models;

namespace LibraryWebDb.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Book> RecentBooks { get; set; }
        public int CurrentPages { get; set; }
        public int? SelectedCategoryId { get; set; }
        public int? SelectedGenreId { get; set; }
        public int TotalPages { get; set; }
        public int LimitPage { get; set; } = 2;
    }
}
