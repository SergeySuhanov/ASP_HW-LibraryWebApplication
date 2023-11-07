using LibraryWebDb.Helpers;
using LibraryWebDb.Models;
using LibraryWebDb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
			var genres = libraryDbContext.Genres;

			var model = new IndexViewModel() { Books = books, Categories = categories, Genres = genres };

			return View(model);
		}

		[HttpGet]
		public IActionResult Add()
		{
			ViewBag.categories = new SelectList(libraryDbContext.Categories, "Id", "Name");
			ViewBag.genres = new MultiSelectList(libraryDbContext.Genres, "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(Book book, IFormFile Image, int[] genres)
	 	{
			//if (ModelState.IsValid)
			//{
			book.ImageUrl = await FileUploadHelper.UploadAsync(Image);
			book.CreatedDate = DateTime.Now;
			await libraryDbContext.Books.AddAsync(book);
			await libraryDbContext.SaveChangesAsync();

			libraryDbContext.BookGenres.AddRange(genres.Select(g => new BookGenre() { BookId = book.Id, GenreId = g }));
            await libraryDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
			//}
			return View(book);
		}

		[HttpGet]
		public IActionResult Edit(int bookId)
		{
			var book = libraryDbContext.Books.Include(b => b.Category).Include(b => b.BookGenres).FirstOrDefault(b => b.Id == bookId);
			return View(book);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Book book, IFormFile Image, int[] genres)
		{
			return RedirectToAction("Index");
		}
	}
}
