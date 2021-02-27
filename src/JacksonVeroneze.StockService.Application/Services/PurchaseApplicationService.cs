using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core;
using JacksonVeroneze.StockService.Core.Data;
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
        private readonly IValidator<AddOrUpdatePurchaseDto> _validatorPurchase;
        private readonly IValidator<AddOrUpdatePurchaseItemDto> _validatorPurchaseItem;

        public PurchaseApplicationService(IMapper mapper, IPurchaseService purchaseService,
            IPurchaseRepository purchaseRepository,
            IProductRepository productRepository,
            IValidator<AddOrUpdatePurchaseDto> validatorPurchase,
            IValidator<AddOrUpdatePurchaseItemDto> validatorPurchaseItem)
        {
            _mapper = mapper;
            _purchaseService = purchaseService;
            _purchaseRepository = purchaseRepository;
            _productRepository = productRepository;
            _validatorPurchase = validatorPurchase;
            _validatorPurchaseItem = validatorPurchaseItem;
        }

        public async Task<PurchaseDto> FindAsync(Guid purchaseId)
            => _mapper.Map<PurchaseDto>(await _purchaseRepository.FindAsync(purchaseId));

        public async Task<IList<PurchaseDto>> FilterAsync(Pagination pagination, PurchaseFilter filter)
            => _mapper.Map<List<PurchaseDto>>(
                await _purchaseRepository.FilterAsync(pagination, filter));

        public async Task<ApplicationDataResult<PurchaseDto>> AddAsync(AddOrUpdatePurchaseDto data)
        {
            ValidationResult validationResult = await _validatorPurchase.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return FactoryFromValidationResult<PurchaseDto>(validationResult);

            Purchase purchase = _mapper.Map<Purchase>(data);

            await _purchaseRepository.AddAsync(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();

            return FactoryResultFromData(_mapper.Map<PurchaseDto>(purchase));
        }

        public async Task<ApplicationDataResult<PurchaseDto>> UpdateAsync(Guid purchaseId, AddOrUpdatePurchaseDto data)
        {
            ValidationResult validationResult = await _validatorPurchase.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return FactoryFromValidationResult<PurchaseDto>(validationResult);

            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(purchaseId);

            purchase.Update(data.Description, data.Date);

            _purchaseRepository.Update(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();

            return FactoryResultFromData(_mapper.Map<PurchaseDto>(purchase));
        }

        public async Task RemoveAsync(Guid purchaseId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(purchaseId);

            _purchaseRepository.Remove(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();
        }

        public async Task CloseAsync(Guid purchaseId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(purchaseId);

            await _purchaseService.CloseAsync(purchase);
        }

        public async Task<PurchaseItemDto> FindItemAsync(Guid purchaseId, Guid purchaseItemId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(purchaseId);

            return _mapper.Map<PurchaseItemDto>(purchase.FindItemById(purchaseItemId));
        }

        public async Task<IList<PurchaseItemDto>> FindItensAsync(Guid purchaseId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(purchaseId);

            return _mapper.Map<IList<PurchaseItemDto>>(purchase.Items);
        }

        public async Task<ApplicationDataResult<PurchaseItemDto>> AddItemAsync(Guid purchaseId,
            AddOrUpdatePurchaseItemDto data)
        {
            ValidationResult validationResult = await _validatorPurchaseItem.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return FactoryFromValidationResult<PurchaseItemDto>(validationResult);

            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(purchaseId);

            Product product = await _productRepository.FindAsync(data.ProductId);

            if (product is null)
                throw ExceptionsFactory.FactoryNotFoundException<Product>(purchaseId);

            PurchaseItem purchaseItem = new(data.Amount, data.Value, purchase, product);

            await _purchaseService.AddItemAsync(purchase, purchaseItem);

            return FactoryResultFromData(_mapper.Map<PurchaseItemDto>(purchaseItem));
        }

        public async Task<ApplicationDataResult<PurchaseItemDto>> UpdateItemAsync(Guid purchaseId, Guid purchaseItemId,
            AddOrUpdatePurchaseItemDto data)
        {
            ValidationResult validationResult = await _validatorPurchaseItem.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return FactoryFromValidationResult<PurchaseItemDto>(validationResult);

            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            PurchaseItem purchaseItem = purchase.FindItemById(purchaseItemId);

            if (purchaseItem is null)
                throw ExceptionsFactory.FactoryNotFoundException<PurchaseItem>(purchaseItemId);

            Product product = await _productRepository.FindAsync(data.ProductId);

            if (product is null)
                throw ExceptionsFactory.FactoryNotFoundException<Product>(purchaseId);

            purchaseItem.Update(data.Amount, data.Value, product);

            await _purchaseService.UpdateItemAsync(purchase, purchaseItem);

            return FactoryResultFromData(_mapper.Map<PurchaseItemDto>(purchaseItem));
        }

        public async Task RemoveItemAsync(Guid purchaseId, Guid purchaseItemId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(purchaseId);

            if (purchase is null)
                throw ExceptionsFactory.FactoryNotFoundException<Purchase>(purchaseId);

            PurchaseItem purchaseItem = purchase.FindItemById(purchaseItemId);

            if (purchaseItem is null)
                throw ExceptionsFactory.FactoryNotFoundException<PurchaseItem>(purchaseItemId);

            await _purchaseService.RemoveItemAsync(purchase, purchaseItem);
        }
    }
}
