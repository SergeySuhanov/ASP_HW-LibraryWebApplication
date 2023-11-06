namespace LibraryWebDb.Models
{
    public static class LibraryDbInitializer
    {
        static public void seed(IApplicationBuilder app)
        {
            var result = app.ApplicationServices.CreateScope();
            var context = result.ServiceProvider.GetRequiredService<LibraryDbContext>();

            if (!context.Categories.Any()) 
            {
                context.Categories.Add(new Category() { Name = "Роман" });
                context.Categories.Add(new Category() { Name = "Учебная литература" });
                context.Categories.Add(new Category() { Name = "Для детей" });
                context.SaveChanges();
            }

            if (!context.Genres.Any())
            {
                context.Genres.Add(new Genre() { Name = "Художественая" });
                context.Genres.Add(new Genre() { Name = "Документальная" });
                context.Genres.Add(new Genre() { Name = "Сказка" });
                context.SaveChanges();
            }
        }
    }
}
