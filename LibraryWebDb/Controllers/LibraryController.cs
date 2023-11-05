using LibraryWebDb.Helpers;
using LibraryWebDb.Models;
using LibraryWebDb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
			var categories = libraryDbContext.Categories;

			var model = new IndexViewModel() { Books = books, Categories = categories };

			return View(model);
		}

		[HttpGet]
		public IActionResult Add()
		{
			ViewBag.categories = new SelectList(libraryDbContext.Categories, "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(Book book, IFormFile Image)
	 	{
			//if (ModelState.IsValid)
			//{

			//Path.GetExtension(Image.FileName);

			book.ImageUrl = await FileUploadHelper.UploadAsync(Image);
				book.CreatedDate = DateTime.Now;
				await libraryDbContext.Books.AddAsync(book);
				await libraryDbContext.SaveChangesAsync();
				return RedirectToAction("Index");
			//}
			return View(book);
		}
	}
}
