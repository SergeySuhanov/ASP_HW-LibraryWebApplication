﻿namespace LibraryWebDb.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<BookGenre> BookGenres { get; set; }
    }
}
