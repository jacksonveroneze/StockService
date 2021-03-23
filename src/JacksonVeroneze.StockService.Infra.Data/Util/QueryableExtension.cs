using System.Linq;
using JacksonVeroneze.StockService.Core.Data;

namespace JacksonVeroneze.StockService.Infra.Data.Util
{
    public static class QueryableExtension
    {
        public static IQueryable<TSource> ConfigureSkipTakeFromPagination<TSource>(this IQueryable<TSource> queryable,
            Pagination pagination)
        {
            int skip = pagination.Skip ??= 0;

            if (skip < 0) skip = 0;

            if (skip > 0) skip--;

            int take = pagination.Take ??= 30;

            return queryable
                .Skip(skip * take)
                .Take(take);
        }
    }
}
