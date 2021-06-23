using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Application.Validations.Purchase;
using JacksonVeroneze.StockService.Application.Validations.PurchaseItem;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Core.Exceptions;
using JacksonVeroneze.StockService.Core.Notifications;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class PurchaseApplicationService : ApplicationService, IPurchaseApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseService _purchaseService;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseValidator _purchaseValidator;
        private readonly IPurchaseItemValidator _purchaseItemValidator;

        /// <summary>
        /// Method responsible for initialize service.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="purchaseService"></param>
        /// <param name="purchaseRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="purchaseValidator"></param>
        /// <param name="purchaseItemValidator"></param>
        public PurchaseApplicationService(IMapper mapper, IPurchaseService purchaseService,
            IPurchaseRepository purchaseRepository,
            IProductRepository productRepository,
            IPurchaseValidator purchaseValidator,
            IPurchaseItemValidator purchaseItemValidator)
        {
            _mapper = mapper;
            _purchaseService = purchaseService;
            _purchaseRepository = purchaseRepository;
            _productRepository = productRepository;
            _purchaseValidator = purchaseValidator;
            _purchaseItemValidator = purchaseItemValidator;
        }

        /// <summary>
        /// Method responsible for find purchase.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public async Task<PurchaseDto> FindAsync(Guid purchaseId)
            => _mapper.Map<PurchaseDto>(await _purchaseRepository.FindAsync(purchaseId));

        /// <summary>
        /// Method responsible for find list of purchases.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<Pageable<PurchaseDto>> FilterAsync(Pagination pagination, PurchaseFilter filter)
            => _mapper.Map<Pageable<PurchaseDto>>(
                await _purchaseRepository.FilterPaginateAsync(pagination, filter));

        /// <summary>
        /// Method responsible for add purchase.
        /// </summary>
        /// <param name="purchaseDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<PurchaseDto>> AddAsync(AddOrUpdatePurchaseDto purchaseDto)
        {
            NotificationContext result = await _purchaseValidator.ValidateCreateAsync(purchaseDto);

            if (result.HasNotifications)
                return ApplicationDataResult<PurchaseDto>.FactoryFromNotificationContext(result);

            Purchase purchase = _mapper.Map<Purchase>(purchaseDto);

            await _purchaseRepository.AddAsync(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<PurchaseDto>.FactoryFromData(_mapper.Map<PurchaseDto>(purchase));
        }

        /// <summary>
        /// Method responsible for update purchase.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<PurchaseDto>> UpdateAsync(Guid purchaseId,
            AddOrUpdatePurchaseDto purchaseDto)
        {
            NotificationContext result = await _purchaseValidator.ValidateUpdateAsync(purchaseId, purchaseDto);

            if (result.HasNotifications)
                return ApplicationDataResult<PurchaseDto>.FactoryFromNotificationContext(result);

            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            purchase.Update(purchaseDto.Description, purchaseDto.Date);

            _purchaseRepository.Update(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<PurchaseDto>.FactoryFromData(_mapper.Map<PurchaseDto>(purchase));
        }

        /// <summary>
        /// Method responsible for remove purchase.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<PurchaseDto>> RemoveAsync(Guid purchaseId)
        {
            NotificationContext result = await _purchaseValidator.ValidateRemoveAsync(purchaseId);

            if (result.HasNotifications)
                return ApplicationDataResult<PurchaseDto>.FactoryFromNotificationContext(result);

            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            _purchaseRepository.Remove(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();

            return ApplicationDataResult<PurchaseDto>.FactoryFromEmpty();
        }

        /// <summary>
        /// Method responsible for close purchase.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<PurchaseDto>> CloseAsync(Guid purchaseId)
        {
            NotificationContext result = await _purchaseValidator.ValidateCloseAsync(purchaseId);

            if (result.HasNotifications)
                return ApplicationDataResult<PurchaseDto>.FactoryFromNotificationContext(result);

            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            await _purchaseService.CloseAsync(purchase);

            return ApplicationDataResult<PurchaseDto>.FactoryFromEmpty();
        }

        /// <summary>
        /// Method responsible for find purchase item.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItemId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<PurchaseItemDto> FindItemAsync(Guid purchaseId, Guid purchaseItemId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(purchaseId);

            return _mapper.Map<PurchaseItemDto>(purchase.FindItem(purchaseItemId));
        }

        /// <summary>
        /// Method responsible for find purchase items.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<IList<PurchaseItemDto>> FindItensAsync(Guid purchaseId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(purchaseId);

            return _mapper.Map<IList<PurchaseItemDto>>(await _purchaseRepository.FindItems(purchaseId));
        }

        /// <summary>
        /// Method responsible for add purchaseItem.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItemDto"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ApplicationDataResult<PurchaseItemDto>> AddItemAsync(Guid purchaseId,
            AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            NotificationContext result = await _purchaseItemValidator.ValidateCreateAsync(purchaseId, purchaseItemDto);

            if (result.HasNotifications)
                return ApplicationDataResult<PurchaseItemDto>.FactoryFromNotificationContext(result);

            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            Product product = await _productRepository.FindAsync(purchaseItemDto.ProductId);

            PurchaseItem purchaseItem = new(purchaseItemDto.Amount, purchaseItemDto.Value, purchase, product);

            await _purchaseService.AddItemAsync(purchase, purchaseItem);

            return ApplicationDataResult<PurchaseItemDto>.FactoryFromData(_mapper.Map<PurchaseItemDto>(purchaseItem));
        }

        /// <summary>
        /// Method responsible for update purchaseItem.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItemId"></param>
        /// <param name="purchaseItemDto"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<PurchaseItemDto>> UpdateItemAsync(Guid purchaseId, Guid purchaseItemId,
            AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            NotificationContext result =
                await _purchaseItemValidator.ValidateUpdateAsync(purchaseId, purchaseItemId, purchaseItemDto);

            if (result.HasNotifications)
                return ApplicationDataResult<PurchaseItemDto>.FactoryFromNotificationContext(result);

            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            PurchaseItem purchaseItem = purchase.FindItem(purchaseItemId);

            Product product = await _productRepository.FindAsync(purchaseItemDto.ProductId);

            purchaseItem.Update(purchaseItemDto.Amount, purchaseItemDto.Value, product);

            await _purchaseService.UpdateItemAsync(purchase, purchaseItem);

            return ApplicationDataResult<PurchaseItemDto>.FactoryFromData(_mapper.Map<PurchaseItemDto>(purchaseItem));
        }

        /// <summary>
        /// Method responsible for remove purchaseItem.
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItemId"></param>
        /// <returns></returns>
        public async Task<ApplicationDataResult<PurchaseItemDto>> RemoveItemAsync(Guid purchaseId, Guid purchaseItemId)
        {
            NotificationContext result = await _purchaseItemValidator.ValidateRemoveAsync(purchaseId, purchaseItemId);

            if (result.HasNotifications)
                return ApplicationDataResult<PurchaseItemDto>.FactoryFromNotificationContext(result);

            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            PurchaseItem purchaseItem = purchase.FindItem(purchaseItemId);

            await _purchaseService.RemoveItemAsync(purchase, purchaseItem);

            return ApplicationDataResult<PurchaseItemDto>.FactoryFromEmpty();
        }
    }
}
