using LibraryWebDb.Models;

namespace LibraryWebDb.Extensions
{
    public static class DbContextExtensions
    {
        public static void UpdateManyToMany<T,KEY>(this LibraryDbContext libraryDbContext, IEnumerable<T>currentItems, IEnumerable<T> newItems, Func<T,KEY> getKey) where T : class
        {
            if (currentItems == null)
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
            return items.GroupJoin(items, getKeyFunc, getKeyFunc, (item, templateItems) => new { item, templateItems })
                .SelectMany(t => t.templateItems.DefaultIfEmpty(), (t, tmp) => new { t, tmp })
                .Where(t => ReferenceEquals(null, t.tmp) || t.tmp.Equals(default(T)))
                .Select(t => t.t.item);
        }
    }
}
