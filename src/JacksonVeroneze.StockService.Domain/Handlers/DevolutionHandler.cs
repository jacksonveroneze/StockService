using System;
using System.Threading;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Events.Devolution;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;
using MediatR;

namespace JacksonVeroneze.StockService.Domain.Handlers
{
    public class DevolutionHandler : INotificationHandler<OutputItemUndoDevolutionEvent>
    {
        private readonly IDevolutionService _devolutionService;
        private readonly IDevolutionRepository _devolutionRepository;
        private readonly IProductRepository _productRepository;

        public DevolutionHandler(IDevolutionService devolutionService,
            IProductRepository productRepository,
            IDevolutionRepository devolutionRepository)
        {
            _devolutionService = devolutionService;
            _productRepository = productRepository;
            _devolutionRepository = devolutionRepository;
        }

        public async Task Handle(OutputItemUndoDevolutionEvent notification, CancellationToken cancellationToken)
        {
            Product product = await _productRepository.FindAsync(notification.ProductId);

            Devolution devolution = new($"DEVOLUÇÃO: {notification.Description}", DateTime.Now);

            await _devolutionService.AddAsync(devolution);

            await _devolutionRepository.UnitOfWork.CommitAsync();

            await _devolutionService.AddItemAsync(devolution,
                new DevolutionItem(notification.Ammount, devolution, product));

            await _devolutionService.CloseAsync(devolution);
        }
    }
}
