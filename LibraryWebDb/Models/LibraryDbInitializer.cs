namespace LibraryWebDb.Models
{
    public static class LibraryDbInitializer
    {
        static public void seed(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<LibraryDbContext>();

            if (!context.Categories.Any()) 
            {
                context.Categories.Add(new Category() { Name = "Роман" });
                context.Categories.Add(new Category() { Name = "Учебная литература" });
                context.Categories.Add(new Category() { Name = "Для детей" });
            }

            /*if (!context.Books.Any())
            {
                context.Books.Add(new Book() { Title = "Роман" });
                context.Books.Add(new Book() { Title = "Учебная литература" });
                context.Books.Add(new Book() { Title = "Для детей" });
            }*/
        }
    }
}
