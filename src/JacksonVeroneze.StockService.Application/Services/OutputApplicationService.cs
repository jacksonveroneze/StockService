using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Application.Validations.Output;
using JacksonVeroneze.StockService.Application.Validations.OutputItem;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.Exceptions;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class OutputApplicationService : ApplicationService, IOutputApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IOutputService _outputService;
        private readonly IOutputRepository _outputRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOutputValidator _outputValidator;
        private readonly IOutputItemValidator _outputItemValidator;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="outputService"></param>
        /// <param name="outputRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="outputValidator"></param>
        /// <param name="outputItemValidator"></param>
        public OutputApplicationService(IMapper mapper, IOutputService outputService,
            IOutputRepository outputRepository,
            IProductRepository productRepository,
            IOutputValidator outputValidator,
            IOutputItemValidator outputItemValidator)
        {
            _mapper = mapper;
            _outputService = outputService;
            _outputRepository = outputRepository;
            _productRepository = productRepository;
            _outputValidator = outputValidator;
            _outputItemValidator = outputItemValidator;
        }

        /// <summary>
        /// Method responsible for find output.
        /// </summary>
        /// <param name="outputId"></param>
        /// <returns></returns>
        public async Task<OutputDto> FindAsync(Guid outputId)
            => _mapper.Map<OutputDto>(await _outputRepository.FindAsync(outputId));

        /// <summary>
        /// Method responsible for find list of outputs.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<Pageable<OutputDto>> FilterAsync(Pagination pagination, OutputFilter filter)
            => _mapper.Map<Pageable<OutputDto>>(
                await _outputRepository.FilterPaginateAsync(pagination, filter));

        /// <summary>
        /// Method responsible for add output.
        /// </summary>
        /// <param name="outputDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<OutputDto>> AddAsync(AddOrUpdateOutputDto outputDto)
        {
            NotificationContext result = await _outputValidator.ValidateCreateAsync(outputDto);

            if (result.HasNotifications)
                return ApplicationDataResult<OutputDto>.FactoryFromNotificationContext(result);

            Output output = _mapper.Map<Output>(outputDto);

            await _outputRepository.AddAsync(output);

            await _outputRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<OutputDto>.FactoryFromData(_mapper.Map<OutputDto>(output));
        }

        /// <summary>
        /// Method responsible for update output.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<OutputDto>> UpdateAsync(Guid outputId,
            AddOrUpdateOutputDto outputDto)
        {
            NotificationContext result = await _outputValidator.ValidateUpdateAsync(outputId, outputDto);

            if (result.HasNotifications)
                return ApplicationDataResult<OutputDto>.FactoryFromNotificationContext(result);

            Output output = await _outputRepository.FindAsync(outputId);

            output.Update(outputDto.Description, outputDto.Date);

            _outputRepository.Update(output);

            await _outputRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<OutputDto>.FactoryFromData(_mapper.Map<OutputDto>(output));
        }

        /// <summary>
        /// Method responsible for remove output.
        /// </summary>
        /// <param name="outputId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<OutputDto>> RemoveAsync(Guid outputId)
        {
            NotificationContext result = await _outputValidator.ValidateRemoveAsync(outputId);

            if (result.HasNotifications)
                return ApplicationDataResult<OutputDto>.FactoryFromNotificationContext(result);

            Output output = await _outputRepository.FindAsync(outputId);

            _outputRepository.Remove(output);

            await _outputRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<OutputDto>.FactoryFromEmpty();
        }

        /// <summary>
        /// Method responsible for close output.
        /// </summary>
        /// <param name="outputId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<OutputDto>> CloseAsync(Guid outputId)
        {
            NotificationContext result = await _outputValidator.ValidateCloseAsync(outputId);

            if (result.HasNotifications)
                return ApplicationDataResult<OutputDto>.FactoryFromNotificationContext(result);

            Output output = await _outputRepository.FindAsync(outputId);

            await _outputService.CloseAsync(output);

            return ApplicationDataResult<OutputDto>.FactoryFromEmpty();
        }

        /// <summary>
        /// Method responsible for find output item.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputItemId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<OutputItemDto> FindItemAsync(Guid outputId, Guid outputItemId)
        {
            Output output = await _outputRepository.FindAsync(outputId);

            if (output is null)
                throw ExceptionsFactory.FactoryNotFoundException<Output>(outputId);

            return _mapper.Map<OutputItemDto>(output.FindItem(outputItemId));
        }

        /// <summary>
        /// Method responsible for find output items.
        /// </summary>
        /// <param name="outputId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<IList<OutputItemDto>> FindItensAsync(Guid outputId)
        {
            Output output = await _outputRepository.FindAsync(outputId);

            if (output is null)
                throw ExceptionsFactory.FactoryNotFoundException<Output>(outputId);

            return _mapper.Map<IList<OutputItemDto>>(await _outputRepository.FindItems(outputId));
        }

        /// <summary>
        /// Method responsible for add outputItem.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputItemDto"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ApplicationDataResult<OutputItemDto>> AddItemAsync(Guid outputId,
            AddOrUpdateOutputItemDto outputItemDto)
        {
            NotificationContext result = await _outputItemValidator.ValidateCreateAsync(outputId, outputItemDto);

            if (result.HasNotifications)
                return ApplicationDataResult<OutputItemDto>.FactoryFromNotificationContext(result);

            Output output = await _outputRepository.FindAsync(outputId);

            Product product = await _productRepository.FindAsync(outputItemDto.ProductId);

            OutputItem outputItem = new(outputItemDto.Amount, output, product);

            await _outputService.AddItemAsync(output, outputItem);

            return ApplicationDataResult<OutputItemDto>.FactoryFromData(_mapper.Map<OutputItemDto>(outputItem));
        }

        /// <summary>
        /// Method responsible for update outputItem.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputItemId"></param>
        /// <param name="outputItemDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<OutputItemDto>> UpdateItemAsync(Guid outputId, Guid outputItemId,
            AddOrUpdateOutputItemDto outputItemDto)
        {
            NotificationContext result =
                await _outputItemValidator.ValidateUpdateAsync(outputId, outputItemId, outputItemDto);

            if (result.HasNotifications)
                return ApplicationDataResult<OutputItemDto>.FactoryFromNotificationContext(result);

            Output output = await _outputRepository.FindAsync(outputId);

            OutputItem outputItem = output.FindItem(outputItemId);

            Product product = await _productRepository.FindAsync(outputItemDto.ProductId);

            outputItem.Update(outputItemDto.Amount, product);

            await _outputService.UpdateItemAsync(output, outputItem);

            return ApplicationDataResult<OutputItemDto>.FactoryFromData(_mapper.Map<OutputItemDto>(outputItem));
        }

        /// <summary>
        /// Method responsible for remove outputItem.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputItemId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<OutputItemDto>> RemoveItemAsync(Guid outputId, Guid outputItemId)
        {
            NotificationContext result = await _outputItemValidator.ValidateRemoveAsync(outputId, outputItemId);

            if (result.HasNotifications)
                return ApplicationDataResult<OutputItemDto>.FactoryFromNotificationContext(result);

            Output output = await _outputRepository.FindAsync(outputId);

            OutputItem outputItem = output.FindItem(outputItemId);

            await _outputService.RemoveItemAsync(output, outputItem);

            return ApplicationDataResult<OutputItemDto>.FactoryFromEmpty();
        }

        /// <summary>
        /// Method responsible for undo outputItem.
        /// </summary>
        /// <param name="outputId"></param>
        /// <param name="outputItemId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<OutputItemDto>> UndoItemAsync(Guid outputId, Guid outputItemId)
        {
            NotificationContext result = await _outputItemValidator.ValidateUndoItemAsync(outputId, outputItemId);

            if (result.HasNotifications)
                return ApplicationDataResult<OutputItemDto>.FactoryFromNotificationContext(result);

            Output output = await _outputRepository.FindAsync(outputId);

            OutputItem outputItem = output.FindItem(outputItemId);

            await _outputService.UndoItemAsync(output, outputItem);

            return ApplicationDataResult<OutputItemDto>.FactoryFromEmpty();
        }
    }
}
