using LibraryWebDb.Models;

namespace LibraryWebDb.Extensions
{
    public static class DbContextExtensions
    {
        public static void UpdateManyToMany<T,KEY>(this LibraryDbContext libraryDbContext, IEnumerable<T>currentItems, IEnumerable<T> newItems, Func<T,KEY> getKey) where T : class
        {
            if (currentItems != null)
            {
                libraryDbContext.Set<T>().RemoveRange(currentItems.Except(newItems, getKey));
                libraryDbContext.Set<T>().AddRange(newItems.Except(currentItems, getKey));
            }
            else
            {
                libraryDbContext.Set<T>().AddRange(newItems);
            }
        }

        public static IEnumerable<T> Except<T,KEY>(this IEnumerable<T> items, IEnumerable<T> other, Func<T,KEY> getKeyFunc)
        {
            return items.GroupJoin(other, getKeyFunc, getKeyFunc, (item, templateItems) => new { item, templateItems })  // Combine in one chain
                .SelectMany(t => t.templateItems.DefaultIfEmpty(), (t, tmp) => new { t, tmp })                           // Just converting
                .Where(t => (ReferenceEquals(null, t.tmp) || t.tmp.Equals(default(T))))                                    // Filtering
                .Select(t => t.t.item);
        }
    }
}
