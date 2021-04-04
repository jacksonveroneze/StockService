using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Models;

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
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<Pageable<MovementModel>> FilterAsync(MovementFilter filter)
            => await _movementRepository.ReportFilterAsync(filter);
    }
}
