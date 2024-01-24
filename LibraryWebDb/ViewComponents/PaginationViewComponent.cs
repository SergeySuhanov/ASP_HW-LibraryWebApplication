using LibraryWebDb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebDb.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int currentPage, int totalPages, int limit, int? genreId, int? categoryId)
        {
            PaginationViewModel paginationViewModel = new PaginationViewModel()
            {
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PagesLimit = limit,
                GenreId = genreId,
                CategoryId = categoryId
            };
            return View("Pagination", paginationViewModel);
        }
    }
}
