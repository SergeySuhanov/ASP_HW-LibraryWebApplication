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
		public IActionResult Index(int? categoryId, int? genreId, int page = 1)
		{
			var books = libraryDbContext.Books.Include(b => b.BookGenres).ThenInclude(bg => bg.Genre).Include(b => b.Category).OrderByDescending(b=>b.Id);

			if (categoryId != null)
			{
				books = (IOrderedQueryable<Book>)books.Where(b=>b.CategoryId == categoryId);
			}

            if (genreId != null)
            {
				//var bookArray = libraryDbContext.BookGenres.Where(bg=>bg.GenreId == genreId).Select(bg => bg.BookId);
				//books = (IOrderedQueryable<Book>)books.Where(b => bookArray.Contains(b.Id));
				books = (IOrderedQueryable<Book>)books.Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId));
			}

            var model = new IndexViewModel()
			{
				Books = books,
				Categories = libraryDbContext.Categories,
				Genres = libraryDbContext.Genres,
				RecentBooks = libraryDbContext.Books.OrderByDescending(b => b.Id).Take(3),
				CurrentPages = page,
				TotalPages = 10,
				SelectedCategoryId = categoryId,
				SelectedGenreId = genreId
			};

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
		public IActionResult Delete(int id)
		{
			var book = libraryDbContext.Books.Find(id);
			return View(book);
		}

		[HttpPost]
		[ActionName("Delete")]
		public async Task<IActionResult> ConfirmDelete(int id)
		{
			var book = libraryDbContext.Books.Find(id);
			libraryDbContext.Books.Remove(book);
			await libraryDbContext.SaveChangesAsync();
			TempData["status"] = "Книга удалена";
            return RedirectToAction("Index");
        }

		[HttpGet]
		public IActionResult Details(int id)
		{
			var book = libraryDbContext.Books
				.Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
				.Include(b => b.Category)
				.FirstOrDefault(b => b.Id == id);
			return View(book);
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var book = libraryDbContext.Books.Find(id);

			ViewBag.categories = new SelectList(libraryDbContext.Categories, "Id", "Name");

			var selectedGenresIds = libraryDbContext.BookGenres.Where(bg => bg.BookId == id).Select(bg => bg.GenreId);
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

            return RedirectToAction("Index");
		}
	}
}
