using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Models;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class MovementApplicationService : ApplicationService, IMovementApplicationService
    {
        private readonly IMovementRepository _movementRepository;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="movementRepository"></param>
        public MovementApplicationService(IMovementRepository movementRepository)
            => _movementRepository = movementRepository;

        /// <summary>
        /// Method responsible for filter data.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<Pageable<MovementModel>> FilterAsync(MovementFilter filter)
            => await _movementRepository.ReportFilterAsync(filter);
    }
}
