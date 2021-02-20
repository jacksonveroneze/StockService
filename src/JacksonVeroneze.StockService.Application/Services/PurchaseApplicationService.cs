using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Domain.Entities;
using JacksonVeroneze.StockService.Domain.Interfaces.Repositories;
using JacksonVeroneze.StockService.Domain.Interfaces.Services;

namespace JacksonVeroneze.StockService.Application.Services
{
    public class PurchaseApplicationService : IPurchaseApplicationService
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

        public async Task<PurchaseDto> FindAsync(Guid id)
            => _mapper.Map<PurchaseDto>(
                await _purchaseRepository.FindAsync(id));

        public async Task<IList<PurchaseDto>> FindAllAsync()
            => _mapper.Map<List<PurchaseDto>>(
                await _purchaseRepository.FindAllAsync());

        public async Task<ApplicationDataResult<PurchaseDto>> AddAsync(AddOrUpdatePurchaseDto data)
        {
            ValidationResult validationResult = await _validatorPurchase.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<PurchaseDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Purchase purchase = _mapper.Map<Purchase>(data);

            await _purchaseRepository.AddAsync(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();

            return new ApplicationDataResult<PurchaseDto>(
                _mapper.Map<PurchaseDto>(purchase));
        }

        public async Task<ApplicationDataResult<PurchaseDto>> UpdateAsync(Guid id, AddOrUpdatePurchaseDto data)
        {
            ValidationResult validationResult = await _validatorPurchase.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<PurchaseDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Purchase purchase = await _purchaseRepository.FindAsync(id);

            if (purchase is null)
                throw new DomainException("Registro não encontrado.");

            purchase.Update(data.Description, data.Date);

            _purchaseRepository.Update(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();

            return new ApplicationDataResult<PurchaseDto>(
                _mapper.Map<PurchaseDto>(purchase));
        }

        public async Task RemoveAsync(Guid id)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            if (purchase is null)
                throw new DomainException("Registro não encontrado.");

            _purchaseRepository.Remove(purchase);

            await _purchaseRepository.UnitOfWork.CommitAsync();
        }

        public async Task CloseAsync(Guid id)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            if (purchase is null)
                throw new DomainException("Registro não encontrado.");

            await _purchaseService.CloseAsync(purchase);
        }

        public async Task<PurchaseItemDto> FindItemAsync(Guid id, Guid itemId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            return _mapper.Map<PurchaseItemDto>(purchase.FindItemById(id));
        }

        public async Task<IList<PurchaseItemDto>> FindItensAsync(Guid id)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            return _mapper.Map<IList<PurchaseItemDto>>(purchase.Items);
        }

        public async Task<ApplicationDataResult<PurchaseItemDto>> AddItemAsync(AddOrUpdatePurchaseItemDto data)
        {
            ValidationResult validationResult = await _validatorPurchaseItem.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<PurchaseItemDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Purchase purchase = await _purchaseRepository.FindAsync(data.PurchaseId);

            Product product = await _productRepository.FindAsync(data.ProductId);

            PurchaseItem purchaseItem = new PurchaseItem(data.Amount, data.Value, purchase, product);

            await _purchaseService.AddItemAsync(purchase, purchaseItem);

            return new ApplicationDataResult<PurchaseItemDto>(
                _mapper.Map<PurchaseItemDto>(purchaseItem));
        }

        public async Task<ApplicationDataResult<PurchaseItemDto>> UpdateItemAsync(AddOrUpdatePurchaseItemDto data)
        {
            ValidationResult validationResult = await _validatorPurchaseItem.ValidateAsync(data);

            if (validationResult.IsValid is false)
                return new ApplicationDataResult<PurchaseItemDto>(
                    validationResult.Errors.Select(x => x.ErrorMessage));

            Purchase purchase = await _purchaseRepository.FindAsync(data.PurchaseId);

            PurchaseItem purchaseItem = _mapper.Map<PurchaseItem>(data);

            await _purchaseService.UpdateItemAsync(purchase, purchaseItem);

            return new ApplicationDataResult<PurchaseItemDto>(
                _mapper.Map<PurchaseItemDto>(purchaseItem));
        }

        public async Task RemoveItemAsync(Guid id, Guid itemId)
        {
            Purchase purchase = await _purchaseRepository.FindAsync(id);

            if (purchase is null)
                throw new DomainException("Registro pai não encontrado.");

            PurchaseItem purchaseItem = purchase.FindItemById(id);

            if (purchaseItem is null)
                throw new DomainException("Registro não encontrado.");

            await _purchaseService.RemoveItemAsync(purchase, purchaseItem);
        }
    }
}
