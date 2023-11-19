using LibraryWebDb.Extensions;
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
			var books = libraryDbContext.Books.Include(b => b.BookGenres).ThenInclude(bg => bg.Genre).Include(b => b.Category);
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
			
			book.ImageUrl = await FileUploadHelper.UploadAsync(Image);
			book.CreatedDate = DateTime.Now;
			await libraryDbContext.Books.AddAsync(book);
			await libraryDbContext.SaveChangesAsync();

			libraryDbContext.BookGenres.AddRange(genres.Select(g => new BookGenre() { BookId = book.Id, GenreId = g }));
            await libraryDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Details(int bookId)
		{
			var book = libraryDbContext.Books
				.Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
				.Include(b => b.Category)
				.FirstOrDefault(b => b.Id == bookId);
			return View(book);
		}

		[HttpGet]
		public IActionResult Edit(int bookId)
		{
			var book = libraryDbContext.Books.Find(bookId);

			ViewBag.categories = new SelectList(libraryDbContext.Categories, "Id", "Name");

			var selectedGenresIds = libraryDbContext.BookGenres.Where(bg => bg.BookId == bookId).Select(g => g.GenreId);
			ViewBag.genres = new MultiSelectList(libraryDbContext.Genres, "Id", "Name", selectedGenresIds);
			return View(book);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book, IFormFile Image, int[] genres)
        {
			if (Image != null)
			{
				var path = await FileUploadHelper.UploadAsync(Image);
				book.ImageUrl = path;
			}
			
			book.CreatedDate = DateTime.Now;
			libraryDbContext.Books.Update(book);
			await libraryDbContext.SaveChangesAsync();

			var bookWithGenres = libraryDbContext.Books.Include(b => b.BookGenres).FirstOrDefault(b => b.Id == book.Id);

			libraryDbContext.UpdateManyToMany(
				bookWithGenres.BookGenres,
				genres.Select(gId => new BookGenre { BookId = book.Id, GenreId = gId}),
				gId => gId.GenreId
				);
			await libraryDbContext.SaveChangesAsync();

			///----- Stopped here at 2:23:44

            return RedirectToAction("Index");
		}
	}
}
