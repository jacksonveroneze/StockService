using System;
using System.Linq.Expressions;

namespace JacksonVeroneze.StockService.Core.DomainObjects
{
    public class BaseFilter<T> where T : EntityRoot
    {
        public virtual Expression<Func<T, bool>> ToQuery()
        {
            Expression<Func<T, bool>> expression = order => true;

            return expression;
        }
    }
}
