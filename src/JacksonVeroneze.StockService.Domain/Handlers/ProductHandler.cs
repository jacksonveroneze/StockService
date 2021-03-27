using System.Threading;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Product;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using MediatR;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public class ProductHandler : INotificationHandler<ProductAddedEvent>
    {
        private readonly IProductRepository _productRepository;

        public ProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(ProductAddedEvent notification, CancellationToken cancellationToken)
        {
            Product product = await _productRepository.FindAsync(notification.AggregateId);
        }
    }
}
