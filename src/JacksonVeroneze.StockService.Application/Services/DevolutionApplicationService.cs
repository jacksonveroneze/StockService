using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Application.DTO.Devolution;
using JacksonVeroneze.StockService.Application.DTO.DevolutionItem;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.Exceptions;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class DevolutionApplicationService : ApplicationService, IDevolutionApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IDevolutionRepository _devolutionRepository;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="devolutionRepository"></param>
        public DevolutionApplicationService(IMapper mapper,
            IDevolutionRepository devolutionRepository)
        {
            _mapper = mapper;
            _devolutionRepository = devolutionRepository;
        }

        /// <summary>
        /// Method responsible for find devolution.
        /// </summary>
        /// <param name="devolutionId"></param>
        /// <returns></returns>
        public async Task<DevolutionDto> FindAsync(Guid devolutionId)
            => _mapper.Map<DevolutionDto>(await _devolutionRepository.FindAsync(devolutionId));

        /// <summary>
        /// Method responsible for find list of devolutions.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<Pageable<DevolutionDto>> FilterAsync(Pagination pagination, DevolutionFilter filter)
            => _mapper.Map<Pageable<DevolutionDto>>(
                await _devolutionRepository.FilterPaginateAsync(pagination, filter));

        /// <summary>
        /// Method responsible for find devolution items.
        /// </summary>
        /// <param name="devolutionId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<IList<DevolutionItemDto>> FindItensAsync(Guid devolutionId)
        {
            Devolution devolution = await _devolutionRepository.FindAsync(devolutionId);

            if (devolution is null)
                throw ExceptionsFactory.FactoryNotFoundException<Devolution>(devolutionId);

            return _mapper.Map<IList<DevolutionItemDto>>(await _devolutionRepository.FindItems(devolutionId));
        }
    }
}
