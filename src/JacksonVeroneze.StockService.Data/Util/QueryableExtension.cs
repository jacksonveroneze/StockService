using System.Linq;
using JacksonVeroneze.StockService.Core.Data;

namespace JacksonVeroneze.StockService.Data.Util
{
    public static class QueryableExtension
    {
        public static IQueryable<TSource> ConfigureSkipTakeFromPagination<TSource>(this IQueryable<TSource> queryable,
            Pagination pagination)
        {
            return queryable
                .Skip(pagination.Skip ??= 0)
                .Take(pagination.Take ??= 30);
        }
    }
}
