using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JacksonVeroneze.StockService.Application.DTO.Movement;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class MovementApplicationService : ApplicationService, IMovementApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IMovementRepository _movementRepository;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="movementRepository"></param>
        public MovementApplicationService(IMapper mapper, IMovementRepository movementRepository)
        {
            _mapper = mapper;
            _movementRepository = movementRepository;
        }

        /// <summary>
        /// Method responsible for filter data.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IList<MovementDto>> FilterAsync(Pagination pagination, MovementFilter filter)
            => _mapper.Map<List<MovementDto>>(
                await _movementRepository.FilterAsync(pagination, filter));

    }
}
