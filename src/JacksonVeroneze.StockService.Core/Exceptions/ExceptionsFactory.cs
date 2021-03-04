using System;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.DomainObjects.Exceptions;

namespace JacksonVeroneze.StockService.Core.Exceptions
{
    public static class ExceptionsFactory
    {
        public static NotFoundException FactoryNotFoundException<TEntity>(Guid id) where TEntity : Entity
            => new($"{ErrorMessages.ItemNotFound} ({id.ToString()}) em '{nameof(TEntity)}'");

        public static DomainException FactoryDomainException(string message)
            => new(message);
    }
}
