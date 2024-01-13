using Microsoft.AspNetCore.Mvc;

namespace LibraryWebDb.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("Pagination");
        }
    }
}
