using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Application.Validations.Adjustment;
using JacksonVeroneze.StockService.Application.Validations.AdjustmentItem;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.Exceptions;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class AdjustmentApplicationService : ApplicationService, IAdjustmentApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IAdjustmentService _adjustmentService;
        private readonly IAdjustmentRepository _adjustmentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAdjustmentValidator _adjustmentValidator;
        private readonly IAdjustmentItemValidator _adjustmentItemValidator;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="adjustmentService"></param>
        /// <param name="adjustmentRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="adjustmentValidator"></param>
        /// <param name="adjustmentItemValidator"></param>
        public AdjustmentApplicationService(IMapper mapper, IAdjustmentService adjustmentService,
            IAdjustmentRepository adjustmentRepository,
            IProductRepository productRepository,
            IAdjustmentValidator adjustmentValidator,
            IAdjustmentItemValidator adjustmentItemValidator)
        {
            _mapper = mapper;
            _adjustmentService = adjustmentService;
            _adjustmentRepository = adjustmentRepository;
            _productRepository = productRepository;
            _adjustmentValidator = adjustmentValidator;
            _adjustmentItemValidator = adjustmentItemValidator;
        }

        /// <summary>
        /// Method responsible for find adjustment.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <returns></returns>
        public async Task<AdjustmentDto> FindAsync(Guid adjustmentId)
            => _mapper.Map<AdjustmentDto>(await _adjustmentRepository.FindAsync(adjustmentId));

        /// <summary>
        /// Method responsible for find list of adjustments.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<Pageable<AdjustmentDto>> FilterAsync(Pagination pagination, AdjustmentFilter filter)
            => _mapper.Map<Pageable<AdjustmentDto>>(
                await _adjustmentRepository.FilterPaginateAsync(pagination, filter));

        /// <summary>
        /// Method responsible for add adjustment.
        /// </summary>
        /// <param name="adjustmentDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<AdjustmentDto>> AddAsync(AddOrUpdateAdjustmentDto adjustmentDto)
        {
            NotificationContext result = await _adjustmentValidator.ValidateCreateAsync(adjustmentDto);

            if (result.HasNotifications)
                return ApplicationDataResult<AdjustmentDto>.FactoryFromNotificationContext(result);

            Adjustment adjustment = _mapper.Map<Adjustment>(adjustmentDto);

            await _adjustmentService.AddAsync(adjustment);

            return ApplicationDataResult<AdjustmentDto>.FactoryFromData(_mapper.Map<AdjustmentDto>(adjustment));
        }

        /// <summary>
        /// Method responsible for update adjustment.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<AdjustmentDto>> UpdateAsync(Guid adjustmentId,
            AddOrUpdateAdjustmentDto adjustmentDto)
        {
            NotificationContext result = await _adjustmentValidator.ValidateUpdateAsync(adjustmentId, adjustmentDto);

            if (result.HasNotifications)
                return ApplicationDataResult<AdjustmentDto>.FactoryFromNotificationContext(result);

            Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            adjustment.Update(adjustmentDto.Description, adjustmentDto.Date);

            _adjustmentRepository.Update(adjustment);

            await _adjustmentRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<AdjustmentDto>.FactoryFromData(_mapper.Map<AdjustmentDto>(adjustment));
        }

        /// <summary>
        /// Method responsible for remove adjustment.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<AdjustmentDto>> RemoveAsync(Guid adjustmentId)
        {
            NotificationContext result = await _adjustmentValidator.ValidateRemoveAsync(adjustmentId);

            if (result.HasNotifications)
                return ApplicationDataResult<AdjustmentDto>.FactoryFromNotificationContext(result);

            Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            _adjustmentRepository.Remove(adjustment);

            await _adjustmentRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<AdjustmentDto>.FactoryFromEmpty();
        }

        /// <summary>
        /// Method responsible for close adjustment.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<AdjustmentDto>> CloseAsync(Guid adjustmentId)
        {
            NotificationContext result = await _adjustmentValidator.ValidateCloseAsync(adjustmentId);

            if (result.HasNotifications)
                return ApplicationDataResult<AdjustmentDto>.FactoryFromNotificationContext(result);

            Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            await _adjustmentService.CloseAsync(adjustment);

            return ApplicationDataResult<AdjustmentDto>.FactoryFromEmpty();
        }

        /// <summary>
        /// Method responsible for find adjustment item.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentItemId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<AdjustmentItemDto> FindItemAsync(Guid adjustmentId, Guid adjustmentItemId)
        {
            Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            if (adjustment is null)
                throw ExceptionsFactory.FactoryNotFoundException<Adjustment>(adjustmentId);

            return _mapper.Map<AdjustmentItemDto>(adjustment.FindItem(adjustmentItemId));
        }

        /// <summary>
        /// Method responsible for find adjustment items.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<IList<AdjustmentItemDto>> FindItensAsync(Guid adjustmentId)
        {
            Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            if (adjustment is null)
                throw ExceptionsFactory.FactoryNotFoundException<Adjustment>(adjustmentId);

            return _mapper.Map<IList<AdjustmentItemDto>>(await _adjustmentRepository.FindItems(adjustmentId));
        }

        /// <summary>
        /// Method responsible for add adjustmentItem.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentItemDto"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ApplicationDataResult<AdjustmentItemDto>> AddItemAsync(Guid adjustmentId,
            AddOrUpdateAdjustmentItemDto adjustmentItemDto)
        {
            NotificationContext result =
                await _adjustmentItemValidator.ValidateCreateAsync(adjustmentId, adjustmentItemDto);

            if (result.HasNotifications)
                return ApplicationDataResult<AdjustmentItemDto>.FactoryFromNotificationContext(result);

            Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            Product product = await _productRepository.FindAsync(adjustmentItemDto.ProductId);

            AdjustmentItem adjustmentItem = new(adjustmentItemDto.Amount, adjustment, product);

            await _adjustmentService.AddItemAsync(adjustment, adjustmentItem);

            return ApplicationDataResult<AdjustmentItemDto>.FactoryFromData(
                _mapper.Map<AdjustmentItemDto>(adjustmentItem));
        }

        /// <summary>
        /// Method responsible for update adjustmentItem.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentItemId"></param>
        /// <param name="adjustmentItemDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<AdjustmentItemDto>> UpdateItemAsync(Guid adjustmentId,
            Guid adjustmentItemId,
            AddOrUpdateAdjustmentItemDto adjustmentItemDto)
        {
            NotificationContext result =
                await _adjustmentItemValidator.ValidateUpdateAsync(adjustmentId, adjustmentItemId, adjustmentItemDto);

            if (result.HasNotifications)
                return ApplicationDataResult<AdjustmentItemDto>.FactoryFromNotificationContext(result);

            Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            AdjustmentItem adjustmentItem = adjustment.FindItem(adjustmentItemId);

            Product product = await _productRepository.FindAsync(adjustmentItemDto.ProductId);

            adjustmentItem.Update(adjustmentItemDto.Amount, product);

            await _adjustmentService.UpdateItemAsync(adjustment, adjustmentItem);

            return ApplicationDataResult<AdjustmentItemDto>.FactoryFromData(
                _mapper.Map<AdjustmentItemDto>(adjustmentItem));
        }

        /// <summary>
        /// Method responsible for remove adjustmentItem.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="adjustmentItemId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<AdjustmentItemDto>> RemoveItemAsync(Guid adjustmentId,
            Guid adjustmentItemId)
        {
            NotificationContext result =
                await _adjustmentItemValidator.ValidateRemoveAsync(adjustmentId, adjustmentItemId);

            if (result.HasNotifications)
                return ApplicationDataResult<AdjustmentItemDto>.FactoryFromNotificationContext(result);

            Adjustment adjustment = await _adjustmentRepository.FindAsync(adjustmentId);

            AdjustmentItem adjustmentItem = adjustment.FindItem(adjustmentItemId);

            await _adjustmentService.RemoveItemAsync(adjustment, adjustmentItem);

            return ApplicationDataResult<AdjustmentItemDto>.FactoryFromEmpty();
        }
    }
}
