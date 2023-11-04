using System.ComponentModel.DataAnnotations;

namespace LibraryWebDb.Models
{
	public class Book
	{
        public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Title { get; set; }

		[Required]
		[MaxLength(255)]
		public string Description { get; set; }

		[Required]
		[MaxLength(50)]
		public string Author { get; set; }

		[Required]
		[Display(Name = "Book Cover Image:")]
		[DataType(DataType.Upload)]
		public string ImageUrl { get; set; }

		public DateTime CreatedDate { get; set;}

		public int CategoryId { get; set; }
		public Category Category { get; set; }
    }
}
