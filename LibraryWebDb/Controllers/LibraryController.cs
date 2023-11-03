using LibraryWebDb.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebDb.Controllers
{
	public class LibraryController : Controller
	{
		private readonly LibraryDbContext libraryDbContext;

		public LibraryController(LibraryDbContext libraryDbContext)
		{
			this.libraryDbContext = libraryDbContext;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var books = libraryDbContext.Books;
			return View(books);
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(Book book)
		{
			if (ModelState.IsValid)
			{
				book.CreatedDate = DateTime.Now;
				await libraryDbContext.Books.AddAsync(book);
				await libraryDbContext.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(book);
		}
	}
}
